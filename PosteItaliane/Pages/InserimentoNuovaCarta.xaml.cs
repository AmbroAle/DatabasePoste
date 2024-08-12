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
                string numeroIdentificativo = GeneraNumeroIdentificativoUnico(connection);
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
                        string Id = Guid.NewGuid().ToString();
                        string CF = UserSession.Instance.CF;
                        bool Letta = false;
                        string Testo = $"Hai aggiunto una nuova carta";
                        string Titolo = $"Aggiunta una nuova carta";
                        string queryNotifica = "INSERT INTO notifica (Id, Titolo, Testo, Letta, CF) " +
                            "VALUES (@Id, @Titolo, @Testo, @Letta, @CF)";
                        using (MySqlCommand commandTipoTransazione = new MySqlCommand(queryNotifica, connection, transaction))
                        {
                            commandTipoTransazione.Parameters.AddWithValue("@Id", Id);
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
        private string GeneraNumeroIdentificativoUnico(MySqlConnection connection)
        {
            string numeroIdentificativo;
            bool esiste;

            do
            {
                // Genera un numero identificativo casuale (ad esempio un numero di 16 cifre)
                numeroIdentificativo = GeneraNumeroIdentificativoCasuale();

                // Verifica se esiste già nel database
                string queryVerifica = "SELECT COUNT(*) FROM CARTA WHERE NumeroIdentificativo = @NumeroIdentificativo";
                MySqlCommand cmdVerifica = new MySqlCommand(queryVerifica, connection);
                cmdVerifica.Parameters.AddWithValue("@NumeroIdentificativo", numeroIdentificativo);

                esiste = Convert.ToInt32(cmdVerifica.ExecuteScalar()) > 0;
            }
            while (esiste);

            return numeroIdentificativo;
        }

        private string GeneraNumeroIdentificativoCasuale()
        {
            Random rnd = new Random();
            string numeroIdentificativo = rnd.Next(100000000, 999999999).ToString("D9");
            return numeroIdentificativo;
        }
    }
}