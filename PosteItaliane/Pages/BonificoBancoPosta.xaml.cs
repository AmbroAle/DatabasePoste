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
            float commissione = (tipoBonifico == "Istantaneo") ? 1.5f : 1f;
            string ente = "Poste Italiane";
            string tipologiaPagamento = "online";
            string numeroIdentificativo = UserSession.Instance.NumeroIdentificativo;
            string connectionString = "server=localhost;uid=root;pwd=;database=PosteItalianeDatabase";
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

                    // Prima query: Inserire nella tabella TRANSAZIONE
                    string queryTransazione = "INSERT INTO TRANSAZIONE (CodTransazione, Importo, Data, NumeroIdentificativo) VALUES (@CodTransazione, @Importo, @Data, @NumeroIdentificativo)";
                    using (MySqlCommand commandTransazione = new MySqlCommand(queryTransazione, connection, transaction))
                    {
                        // Preparazione dei parametri
                        commandTransazione.Parameters.AddWithValue("@CodTransazione", codTransazione);
                        commandTransazione.Parameters.AddWithValue("@Importo", importo);
                        commandTransazione.Parameters.AddWithValue("@Data", DateTime.Now);
                        commandTransazione.Parameters.AddWithValue("@NumeroIdentificativo", "1234567890123456"); // sostituire con un valore valido

                        // Esecuzione della query
                        commandTransazione.ExecuteNonQuery();
                        Console.WriteLine("Query TRANSAZIONE eseguita con successo.");
                    }

                    // Seconda query: Inserire nella tabella TIPO_TRANSAZIONE
                    string queryTipoTransazione = "INSERT INTO TIPO_TRANSAZIONE (CodTransazione, Tipo, IbanDestinatario, Causale,Ente,Commissione,TipologiaPagamento,NumeroIdentificativo) " +
                        "VALUES (@CodTransazione, @Tipo, @IbanDestinatario, @Causale,@Ente,@Commissione,@TipologiaPagamento,@NumeroIdentificativo)";
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
