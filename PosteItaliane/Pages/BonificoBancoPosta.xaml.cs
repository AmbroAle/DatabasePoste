﻿using MySql.Data.MySqlClient;
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
    /// <summary>
    /// Logica di interazione per BonificoBancoPosta.xaml
    /// </summary>
    public partial class BonificoBancoPosta : Page
    {
        public BonificoBancoPosta()
        {
            InitializeComponent();
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new Home());
        }
        private void btnBonifico_Click(object sender, RoutedEventArgs e)
        {
            string iban = txtIBAN.Text;
            string importo = txtImporto.Text;
            string causale = txtCausale.Text;
            ComboBoxItem selectedItem = (ComboBoxItem)TipoBonificoComboBox.SelectedItem;

            // Verifica se è stato selezionato un valore
            if (selectedItem == null)
            {
                MessageBox.Show("Seleziona un tipo di bonifico.");
                return;
            }
            //Controllo se tutti i campi sono stati compilati
            if (string.IsNullOrWhiteSpace(iban) || string.IsNullOrWhiteSpace(importo) || string.IsNullOrWhiteSpace(causale))
            {
                MessageBox.Show("Compilare tutti i campi.");
                return;
            }

            // Controllo se l'importo è un valore valido
            if (!float.TryParse(importo, out float importoValue) || importoValue <= 0)
            {
                MessageBox.Show("L'importo deve essere un numero positivo.");
                return;
            }

            // Chiamata al metodo MakeBonifico
            if (MakeBonifico(iban, importoValue, causale, selectedItem.Content.ToString() ?? "Default"))
            {
                MessageBox.Show("Bonifico effettuato con successo!");
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.NavigateToPage(new Home());
            }
            else
            {
                MessageBox.Show("Errore durante il bonifico.");
            }
        }
        private bool MakeBonifico(string iban, float importo, string causale, string tipoBonifico)
        {
            float commissione = (tipoBonifico == "Bonifico Istantaneo") ? 1f : 0;
            string ente = "Poste Italiane";
            string tipologiaPagamento = "online";
            string numeroIdentificativo = UserSession.Instance.NumeroIdentificativo;
            string connectionString = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Messaggio di debug: Connessione e transazione iniziate
                    Console.WriteLine("Connessione aperta e transazione iniziata.");

                    // Genera un GUID per CodTransazione, da usare in entrambe le query
                    string codTransazione = Guid.NewGuid().ToString();

                    // Verifica se il conto/carta è bloccata
                    string checkBloccataSaldoQuery = "" +
                        "SELECT BloccoCarta, Saldo " +
                        "FROM CARTA " +
                        "WHERE NumeroIdentificativo = @NumeroIdentificativo";
                    bool bloccata;
                    float saldo;
                    using (MySqlCommand checkCommand = new MySqlCommand(checkBloccataSaldoQuery, connection, transaction))
                    {
                        checkCommand.Parameters.AddWithValue("@NumeroIdentificativo", numeroIdentificativo);

                        using (MySqlDataReader reader = checkCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                saldo = reader.GetFloat("Saldo");
                                bloccata = reader.GetBoolean("BloccoCarta");
                            }
                            else
                            {
                                MessageBox.Show("Errore: Carta non trovata.");
                                return false;
                            }
                        }
                    }

                    if (bloccata)
                    {
                        MessageBox.Show("La carta è bloccata. Impossibile effettuare il bonifico.");
                        return false;
                    }

                    if (saldo < importo + commissione)
                    {
                        MessageBox.Show("Saldo insufficiente per effettuare il bonifico.");
                        return false;
                    }

                    // Prima query: Inserire nella tabella TRANSAZIONE
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

                    // Seconda query: Inserire nella tabella TIPO_TRANSAZIONE
                    string queryTipoTransazione = "INSERT INTO TIPO_TRANSAZIONE (CodTransazione, Tipo, IbanDestinatario, Causale, Ente, Commissione, TipologiaPagamento, NumeroIdentificativo) " +
                        "VALUES (@CodTransazione, @Tipo, @IbanDestinatario, @Causale, @Ente, @Commissione, @TipologiaPagamento, @NumeroIdentificativo)";
                    using (MySqlCommand commandTipoTransazione = new MySqlCommand(queryTipoTransazione, connection, transaction))
                    {
                        commandTipoTransazione.Parameters.AddWithValue("@CodTransazione", codTransazione); // Usa lo stesso CodTransazione
                        commandTipoTransazione.Parameters.AddWithValue("@Tipo", tipoBonifico); // Utilizza il parametro tipoBonifico
                        commandTipoTransazione.Parameters.AddWithValue("@IbanDestinatario", iban);
                        commandTipoTransazione.Parameters.AddWithValue("@Causale", causale);
                        commandTipoTransazione.Parameters.AddWithValue("@Ente", ente);
                        commandTipoTransazione.Parameters.AddWithValue("@Commissione", commissione);
                        commandTipoTransazione.Parameters.AddWithValue("@TipologiaPagamento", tipologiaPagamento);
                        commandTipoTransazione.Parameters.AddWithValue("@NumeroIdentificativo", numeroIdentificativo);

                        commandTipoTransazione.ExecuteNonQuery();
                        Console.WriteLine("Query TIPO_TRANSAZIONE eseguita con successo.");
                    }

                    // Terza query: Aggiornare il saldo della carta/conto
                    string updateSaldoQuery = "UPDATE CARTA SET Saldo = Saldo - @Importo - @Commissione WHERE NumeroIdentificativo = @NumeroIdentificativo";
                    using (MySqlCommand updateSaldoCommand = new MySqlCommand(updateSaldoQuery, connection, transaction))
                    {
                        updateSaldoCommand.Parameters.AddWithValue("@Importo", importo);
                        updateSaldoCommand.Parameters.AddWithValue("@Commissione", commissione);
                        updateSaldoCommand.Parameters.AddWithValue("@NumeroIdentificativo", numeroIdentificativo);

                        updateSaldoCommand.ExecuteNonQuery();
                        Console.WriteLine("Query UPDATE SALDO eseguita con successo.");
                    }

                    string CF = UserSession.Instance.CF;
                    bool Letta = false;
                    string Testo = $"Hai effettuato un {tipoBonifico} di {importo}€ a favore di {iban} con causale {causale}";
                    string Titolo = $"{tipoBonifico}";
                    string queryNotifica = "INSERT INTO notifica (Titolo, Testo, Letta, CF) " +
                        "VALUES (@Titolo, @Testo, @Letta, @CF)";
                    using (MySqlCommand commandNotifica = new MySqlCommand(queryNotifica, connection, transaction))
                    {
                        commandNotifica.Parameters.AddWithValue("@Titolo", Titolo);
                        commandNotifica.Parameters.AddWithValue("@Testo", Testo);
                        commandNotifica.Parameters.AddWithValue("@Letta", Letta);
                        commandNotifica.Parameters.AddWithValue("@CF", CF);

                        commandNotifica.ExecuteNonQuery();
                    }

                    // Commit della transazione
                    transaction.Commit();
                    Console.WriteLine("Transazione completata con successo.");
                    return true;
                }
                catch (Exception ex)
                {
                    // Rollback della transazione in caso di errore
                    transaction.Rollback();
                    Console.WriteLine($"Errore durante l'esecuzione della transazione: {ex.Message}");
                    MessageBox.Show($"Errore durante l'esecuzione della transazione: {ex.Message}", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

    }
}
