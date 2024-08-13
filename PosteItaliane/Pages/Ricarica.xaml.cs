using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PosteItaliane.Pages
{
    public partial class Ricarica : Page
    {
        public Ricarica()
        {
            InitializeComponent();
            LoadCards();
        }

        private void LoadCards()
        {
            string connectionString = "server=localhost;uid=root;pwd=;database=PosteItalianeDatabase";
            string query = "SELECT Iban FROM CARTA WHERE CF = @CF AND Tipo = 'PostePay'";
            string numeroIdentificativo = UserSession.Instance.NumeroIdentificativo;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CF", UserSession.Instance.CF);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            TipoBonificoComboBox.Items.Clear(); // Pulisci gli elementi esistenti
                            while (reader.Read())
                            {
                                // Usare ?? "" per garantire che non ci siano valori nulli
                                string iban = reader["Iban"].ToString() ?? "";
                                ComboBoxItem item = new ComboBoxItem
                                {
                                    Content = iban
                                };
                                TipoBonificoComboBox.Items.Add(item);
                            }

                            if (TipoBonificoComboBox.Items.Count == 0)
                            {
                                MessageBox.Show("Non hai carte PostePay disponibili.", "Informazione", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante il caricamento delle carte: {ex.Message}", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new Home());
        }

        private void btnRicarica_Click(object sender, RoutedEventArgs e)
        {
            if (TipoBonificoComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string iban = selectedItem.Content.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(iban))
                {
                    MessageBox.Show("Seleziona un IBAN valido.", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Converti direttamente in decimal
                if (!decimal.TryParse(txtImporto.Text, out decimal importo) || importo <= 0)
                {
                    MessageBox.Show("Inserisci un importo valido e maggiore di zero.", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Esegui la ricarica
                if (MakeRicarica(importo, iban))
                {
                    MessageBox.Show($"Ricarica di {importo:C} effettuata sulla carta con IBAN: {iban}", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
            else
            {
                MessageBox.Show("Seleziona una carta PostePay.", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool MakeRicarica(decimal importo, string iban)
        {
            string connectionString = "server=localhost;uid=root;pwd=;database=PosteItalianeDatabase";
            float commissione = 0;
            string ente = "Poste Italiane";
            string tipologiaPagamento = "online";
            string numeroIdentificativo = UserSession.Instance.NumeroIdentificativo;
            string tipo = "Ricarica";
            string causale = "Ricarica carta PostePay";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Start a transaction to ensure data consistency
                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Check if the IBAN selected for PostePay exists
                            string checkPostePayQuery = "SELECT Saldo FROM CARTA WHERE Iban = @Iban AND Tipo = 'PostePay'";
                            using (MySqlCommand checkPostePayCommand = new MySqlCommand(checkPostePayQuery, connection, transaction))
                            {
                                checkPostePayCommand.Parameters.AddWithValue("@Iban", iban);
                                object result = checkPostePayCommand.ExecuteScalar();

                                if (result == null)
                                {
                                    MessageBox.Show("IBAN PostePay non trovato.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return false;
                                }

                                // Check if the BancoPosta account has sufficient funds
                                string userCF = UserSession.Instance.CF;
                                string checkBancoPostaQuery = "SELECT Saldo FROM CARTA WHERE CF = @CF AND Tipo = 'BancoPosta'";
                                using (MySqlCommand checkBancoPostaCommand = new MySqlCommand(checkBancoPostaQuery, connection, transaction))
                                {
                                    checkBancoPostaCommand.Parameters.AddWithValue("@CF", userCF);
                                    object bancoPostaResult = checkBancoPostaCommand.ExecuteScalar();

                                    if (bancoPostaResult == null)
                                    {
                                        MessageBox.Show("Account BancoPosta non trovato.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                                        return false;
                                    }

                                    decimal saldoBancoPosta = Convert.ToDecimal(bancoPostaResult);

                                    if (saldoBancoPosta < importo)
                                    {
                                        MessageBox.Show("Saldo BancoPosta insufficiente.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                                        return false;
                                    }

                                    // Deduct the amount from BancoPosta
                                    decimal nuovoSaldoBancoPosta = saldoBancoPosta - importo;
                                    string updateBancoPostaQuery = "UPDATE CARTA SET Saldo = @NuovoSaldo WHERE CF = @CF AND Tipo = 'BancoPosta'";

                                    using (MySqlCommand updateBancoPostaCommand = new MySqlCommand(updateBancoPostaQuery, connection, transaction))
                                    {
                                        updateBancoPostaCommand.Parameters.AddWithValue("@NuovoSaldo", nuovoSaldoBancoPosta);
                                        updateBancoPostaCommand.Parameters.AddWithValue("@CF", userCF);
                                        updateBancoPostaCommand.ExecuteNonQuery();
                                    }

                                    // Add the amount to PostePay
                                    decimal saldoPostePay = Convert.ToDecimal(result);
                                    decimal nuovoSaldoPostePay = saldoPostePay + importo;
                                    string updatePostePayQuery = "UPDATE CARTA SET Saldo = @NuovoSaldo WHERE Iban = @Iban AND Tipo = 'PostePay'";

                                    using (MySqlCommand updatePostePayCommand = new MySqlCommand(updatePostePayQuery, connection, transaction))
                                    {
                                        updatePostePayCommand.Parameters.AddWithValue("@NuovoSaldo", nuovoSaldoPostePay);
                                        updatePostePayCommand.Parameters.AddWithValue("@Iban", iban);
                                        updatePostePayCommand.ExecuteNonQuery();
                                    }

                                    //add the transaction to the database
                                    string codTransazione = Guid.NewGuid().ToString();
                                    string queryTransazione = "INSERT INTO TRANSAZIONE (CodTransazione, Importo, Data, NumeroIdentificativo) VALUES (@CodTransazione, @Importo, @Data, @NumeroIdentificativo)";
                                    using (MySqlCommand commandTransazione = new MySqlCommand(queryTransazione, connection, transaction))
                                    {
                                        commandTransazione.Parameters.AddWithValue("@CodTransazione", codTransazione);
                                        commandTransazione.Parameters.AddWithValue("@Importo", importo);
                                        commandTransazione.Parameters.AddWithValue("@Data", DateTime.Now);
                                        commandTransazione.Parameters.AddWithValue("@NumeroIdentificativo", numeroIdentificativo);

                                        commandTransazione.ExecuteNonQuery();
                                        Console.WriteLine("Query TRANSAZIONE eseguita con successo.");
                                    }
                                    string queryTipoTransazione = "INSERT INTO TIPO_TRANSAZIONE (CodTransazione, Tipo, IbanDestinatario, Causale, Ente, Commissione, TipologiaPagamento, NumeroIdentificativo) " +
                       "VALUES (@CodTransazione, @Tipo, @IbanDestinatario, @Causale, @Ente, @Commissione, @TipologiaPagamento, @NumeroIdentificativo)";
                                    using (MySqlCommand commandTipoTransazione = new MySqlCommand(queryTipoTransazione, connection, transaction))
                                    {
                                        commandTipoTransazione.Parameters.AddWithValue("@CodTransazione", codTransazione); // use the same CodTransazione
                                        commandTipoTransazione.Parameters.AddWithValue("@Tipo", tipo);
                                        commandTipoTransazione.Parameters.AddWithValue("@IbanDestinatario", iban);
                                        commandTipoTransazione.Parameters.AddWithValue("@Causale", causale);
                                        commandTipoTransazione.Parameters.AddWithValue("@Ente", ente);
                                        commandTipoTransazione.Parameters.AddWithValue("@Commissione", commissione);
                                        commandTipoTransazione.Parameters.AddWithValue("@TipologiaPagamento", tipologiaPagamento);
                                        commandTipoTransazione.Parameters.AddWithValue("@NumeroIdentificativo", numeroIdentificativo);

                                        commandTipoTransazione.ExecuteNonQuery();
                                        Console.WriteLine("Query TIPO_TRANSAZIONE eseguita con successo.");
                                    }
                                    string CF = UserSession.Instance.CF;
                                    bool Letta = false;
                                    string Testo = $"Ricarica di {importo:C} effettuata sulla carta con IBAN: {iban}";
                                    string Titolo = $"Ricarica Effettuata";
                                    string queryNotifica = "INSERT INTO notifica (Titolo, Testo, Letta, CF) " +
                                        "VALUES (@Titolo, @Testo, @Letta, @CF)";
                                    using (MySqlCommand commandTipoTransazione = new MySqlCommand(queryNotifica, connection, transaction))
                                    {
                                        commandTipoTransazione.Parameters.AddWithValue("@Titolo", Titolo);
                                        commandTipoTransazione.Parameters.AddWithValue("@Testo", Testo);
                                        commandTipoTransazione.Parameters.AddWithValue("@Letta", Letta);
                                        commandTipoTransazione.Parameters.AddWithValue("@CF", CF);

                                        commandTipoTransazione.ExecuteNonQuery();
                                    }
                                }
                            }

                            // Commit the transaction
                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            // Rollback in case of error
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante la ricarica: {ex.Message}", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

    }
}