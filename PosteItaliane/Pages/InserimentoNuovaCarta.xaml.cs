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
    /// Logica di interazione per InserimentoNuovaCarta.xaml
    /// </summary>
    public partial class InserimentoNuovaCarta : Page
    {
        public InserimentoNuovaCarta()
        {
            InitializeComponent();
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new Home());
        }
        private void btnInserisciCarta_Click(object sender, RoutedEventArgs e)
        {
            // Prendi i valori dai campi del form

            string ccv = txtCCV.Text;
            string pin = txtPin.Text;
            string iban = txtIban.Text;
            string tipo = "PostePay";
            decimal saldo = 0;
            string cf = UserSession.Instance.CF;

            DateTime dataCorrente = DateTime.Now;
            // Imposta la scadenza a 4 anni dopo la data corrente
            DateTime scadenza = dataCorrente.AddYears(4);

            // Validazione dei campi
            if (string.IsNullOrWhiteSpace(ccv) ||
                string.IsNullOrWhiteSpace(pin) ||
                string.IsNullOrWhiteSpace(iban))
            {
                MessageBox.Show("Compila tutti i campi correttamente.");
                return;
            }

            // Preparazione della connessione a MySQL
            string connectionString = "server=localhost;uid=root;pwd=;database=PosteItalianeDatabase";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string numeroIdentificativo = GeneraNumeroIdentificativoUnico();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Query per l'inserimento della nuova carta
                        string insertCartaQuery = @"
                            INSERT INTO CARTA 
                            (NumeroIdentificativo, Ccv, Pin, Scadenza, Saldo, Iban, Tipo, CF) 
                            VALUES 
                            (@NumeroIdentificativo, @Ccv, @Pin, @Scadenza, @Saldo, @Iban, @Tipo, @CF)";

                        using (MySqlCommand command = new MySqlCommand(insertCartaQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@NumeroIdentificativo", numeroIdentificativo);
                            command.Parameters.AddWithValue("@Ccv", ccv);
                            command.Parameters.AddWithValue("@Pin", pin);
                            command.Parameters.AddWithValue("@Scadenza", scadenza);
                            command.Parameters.AddWithValue("@Saldo", saldo);
                            command.Parameters.AddWithValue("@Iban", iban);
                            command.Parameters.AddWithValue("@Tipo", tipo);
                            command.Parameters.AddWithValue("@CF", cf);

                            command.ExecuteNonQuery();
                        }
                        string CF = UserSession.Instance.CF;
                        bool Letta = false;
                        string Testo = $"Hai aggiunto una nuova carta";
                        string Titolo = $"Aggiunta una nuova carta";
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
                        transaction.Commit();
                        MessageBox.Show("Carta inserita con successo.");
                        var mainWindow = Application.Current.MainWindow as MainWindow;
                        mainWindow?.NavigateToPage(new Home());
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Errore durante l'inserimento della carta: {ex.Message}");
                    }
                }
            }
        }

        private string GeneraNumeroIdentificativoUnico()
        {
            DateTime now = DateTime.Now;
            string timestamp = now.ToString("yyyyMMddHHmmssfff"); // Genera un timestamp unico
            string randomPart = new Random().Next(1000, 9999).ToString(); // Aggiunge una parte casuale per garantire unicità
            string numeroIdentificativo = timestamp + randomPart;

            return numeroIdentificativo.Substring(0, 16); // Assicura che sia lungo 16 cifre
        }

    }
}