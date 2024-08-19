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
            string connectionString = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";
            string query = "SELECT Iban FROM CARTA WHERE CF = @CF AND NumeroIdentificativo != @NumeroIdentificativo";
            string numeroIdentificativo = UserSession.Instance.NumeroIdentificativo;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CF", UserSession.Instance.CF);
                        command.Parameters.AddWithValue("@NumeroIdentificativo", numeroIdentificativo); // Aggiungi questo parametro

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            TipoBonificoComboBox.Items.Clear(); // Pulisci gli elementi esistenti
                            while (reader.Read())
                            {
                                string iban = reader["Iban"].ToString() ?? "";
                                ComboBoxItem item = new ComboBoxItem
                                {
                                    Content = iban
                                };
                                TipoBonificoComboBox.Items.Add(item);
                            }

                            if (TipoBonificoComboBox.Items.Count == 0)
                            {
                                MessageBox.Show("Non hai carte disponibili.", "Informazione", MessageBoxButton.OK, MessageBoxImage.Information);
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
            string connectionString = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";
            float commissione = 1;
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

                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Verifica se la carta PostePay esiste e non è bloccata
                            string checkPostePayQuery = "SELECT Saldo, BloccoCarta FROM CARTA WHERE Iban = @Iban";
                            decimal saldoPostePay;
                            bool isPostePayBloccata;

                            using (MySqlCommand checkPostePayCommand = new MySqlCommand(checkPostePayQuery, connection, transaction))
                            {
                                checkPostePayCommand.Parameters.AddWithValue("@Iban", iban);

                                using (MySqlDataReader reader = checkPostePayCommand.ExecuteReader())
                                {
                                    if (!reader.Read())
                                    {
                                        MessageBox.Show("IBAN PostePay non trovato.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                                        return false;
                                    }

                                    isPostePayBloccata = reader.GetBoolean(reader.GetOrdinal("BloccoCarta"));
                                    saldoPostePay = reader.GetDecimal(reader.GetOrdinal("Saldo"));
                                }
                            }

                            if (isPostePayBloccata)
                            {
                                MessageBox.Show("La carta PostePay selezionata è bloccata.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                                return false;
                            }

                            // Verifica se l'account BancoPosta esiste e non è bloccato
                            string userCF = UserSession.Instance.CF;
                            decimal saldoBancoPosta;
                            bool isBancoPostaBloccata;
                            string checkBancoPostaQuery = "SELECT Saldo, BloccoCarta FROM CARTA WHERE CF = @CF AND Tipo = 'BancoPosta' AND NumeroIdentificativo = @NumeroIdentificativo";

                            using (MySqlCommand checkBancoPostaCommand = new MySqlCommand(checkBancoPostaQuery, connection, transaction))
                            {
                                checkBancoPostaCommand.Parameters.AddWithValue("@CF", userCF);
                                checkBancoPostaCommand.Parameters.AddWithValue("@NumeroIdentificativo", numeroIdentificativo);

                                using (MySqlDataReader reader = checkBancoPostaCommand.ExecuteReader())
                                {
                                    if (!reader.Read())
                                    {
                                        MessageBox.Show("Account BancoPosta non trovato.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                                        return false;
                                    }

                                    isBancoPostaBloccata = reader.GetBoolean(reader.GetOrdinal("BloccoCarta"));
                                    saldoBancoPosta = reader.GetDecimal(reader.GetOrdinal("Saldo"));
                                }
                            }

                            if (isBancoPostaBloccata)
                            {
                                MessageBox.Show("L'account BancoPosta selezionato è bloccato.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                                return false;
                            }

                            if (saldoBancoPosta < importo)
                            {
                                MessageBox.Show("Saldo BancoPosta insufficiente.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                                return false;
                            }

                            // Deduzione dell'importo dal saldo BancoPosta
                            decimal nuovoSaldoBancoPosta = saldoBancoPosta - (importo + 1);
                            string updateBancoPostaQuery = "UPDATE CARTA SET Saldo = @NuovoSaldo WHERE CF = @CF AND Tipo = 'BancoPosta' AND NumeroIdentificativo = @NumeroIdentificativo";

                            using (MySqlCommand updateBancoPostaCommand = new MySqlCommand(updateBancoPostaQuery, connection, transaction))
                            {
                                updateBancoPostaCommand.Parameters.AddWithValue("@NuovoSaldo", nuovoSaldoBancoPosta);
                                updateBancoPostaCommand.Parameters.AddWithValue("@CF", userCF);
                                updateBancoPostaCommand.Parameters.AddWithValue("@NumeroIdentificativo", numeroIdentificativo);
                                updateBancoPostaCommand.ExecuteNonQuery();
                            }

                            // Aggiunta dell'importo al saldo PostePay
                            decimal nuovoSaldoPostePay = saldoPostePay + importo;
                            string updatePostePayQuery = "UPDATE CARTA SET Saldo = @NuovoSaldo WHERE Iban = @Iban AND Tipo = 'PostePay'";

                            using (MySqlCommand updatePostePayCommand = new MySqlCommand(updatePostePayQuery, connection, transaction))
                            {
                                updatePostePayCommand.Parameters.AddWithValue("@NuovoSaldo", nuovoSaldoPostePay);
                                updatePostePayCommand.Parameters.AddWithValue("@Iban", iban);
                                updatePostePayCommand.ExecuteNonQuery();
                            }

                            // Inserimento della transazione
                            string codTransazione = Guid.NewGuid().ToString();
                            string queryTransazione = "INSERT INTO TRANSAZIONE (CodTransazione, Importo, Data, NumeroIdentificativo) VALUES (@CodTransazione, @Importo, @Data, @NumeroIdentificativo)";

                            using (MySqlCommand commandTransazione = new MySqlCommand(queryTransazione, connection, transaction))
                            {
                                commandTransazione.Parameters.AddWithValue("@CodTransazione", codTransazione);
                                commandTransazione.Parameters.AddWithValue("@Importo", importo);
                                commandTransazione.Parameters.AddWithValue("@Data", DateTime.Now);
                                commandTransazione.Parameters.AddWithValue("@NumeroIdentificativo", numeroIdentificativo);
                                commandTransazione.ExecuteNonQuery();
                            }

                            // Inserimento nel tipo di transazione
                            string queryTipoTransazione = "INSERT INTO TIPO_TRANSAZIONE (CodTransazione, Tipo, IbanDestinatario, Causale, Ente, Commissione, TipologiaPagamento, NumeroIdentificativo) " +
                                                          "VALUES (@CodTransazione, @Tipo, @IbanDestinatario, @Causale, @Ente, @Commissione, @TipologiaPagamento, @NumeroIdentificativo)";

                            using (MySqlCommand commandTipoTransazione = new MySqlCommand(queryTipoTransazione, connection, transaction))
                            {
                                commandTipoTransazione.Parameters.AddWithValue("@CodTransazione", codTransazione);
                                commandTipoTransazione.Parameters.AddWithValue("@Tipo", tipo);
                                commandTipoTransazione.Parameters.AddWithValue("@IbanDestinatario", iban);
                                commandTipoTransazione.Parameters.AddWithValue("@Causale", causale);
                                commandTipoTransazione.Parameters.AddWithValue("@Ente", ente);
                                commandTipoTransazione.Parameters.AddWithValue("@Commissione", commissione);
                                commandTipoTransazione.Parameters.AddWithValue("@TipologiaPagamento", tipologiaPagamento);
                                commandTipoTransazione.Parameters.AddWithValue("@NumeroIdentificativo", numeroIdentificativo);
                                commandTipoTransazione.ExecuteNonQuery();
                            }

                            // Inserimento della notifica
                            string CF = UserSession.Instance.CF;
                            bool Letta = false;
                            string Testo = $"Ricarica di {importo:C} effettuata sulla carta con IBAN: {iban}";
                            string Titolo = "Ricarica Effettuata";
                            string queryNotifica = "INSERT INTO notifica (Titolo, Testo, Letta, CF) VALUES (@Titolo, @Testo, @Letta, @CF)";

                            using (MySqlCommand commandNotifica = new MySqlCommand(queryNotifica, connection, transaction))
                            {
                                commandNotifica.Parameters.AddWithValue("@Titolo", Titolo);
                                commandNotifica.Parameters.AddWithValue("@Testo", Testo);
                                commandNotifica.Parameters.AddWithValue("@Letta", Letta);
                                commandNotifica.Parameters.AddWithValue("@CF", CF);
                                commandNotifica.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
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