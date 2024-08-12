using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
    /// Logica di interazione per ModificaDati.xaml
    /// </summary>
    public partial class ModificaDati : Page
    {
        public ModificaDati()
        {
            InitializeComponent();
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new Home());
        }
        private void btnAggiorna_Click(object sender, RoutedEventArgs e)
        {
            // Connessione al database
            string connectionString = "server=localhost;uid=root;pwd=;database=PosteItalianeDatabase";
            bool emailChanged = false;
            bool passwordChanged = false;

            // Valori dei campi
            string email = txtEmail.Text.Trim(); // Trim rimuove gli spazi bianchi all'inizio e alla fine di una stringa
            string password = txtPassword.Password.Trim();

            // Verifica se entrambi i campi sono vuoti
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Inserisci almeno un dato da aggiornare.", "Nessun dato inserito", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Costruzione della query SQL dinamica
                var updateQuery = new StringBuilder("UPDATE account SET ");
                var parameters = new List<MySqlParameter>();

                if (!string.IsNullOrEmpty(email))
                {
                    updateQuery.Append("Email = @Email, ");
                    parameters.Add(new MySqlParameter("@Email", email));
                    emailChanged = true;
                }
                if (!string.IsNullOrEmpty(password))
                {
                    updateQuery.Append("Password = @Password, ");
                    parameters.Add(new MySqlParameter("@Password", password));
                    passwordChanged = true;
                }

                // Rimuovi l'ultima virgola e aggiungi la clausola WHERE
                if (updateQuery.Length > 0)
                {
                    updateQuery.Length -= 2; // Rimuove l'ultima virgola
                    updateQuery.Append(" WHERE CF = @CF");

                    // Aggiungi il parametro CF
                    parameters.Add(new MySqlParameter("@CF", "ABCDEF12G34H567I")); // Sostituisci con il CF dell'utente attuale

                    // Esegui l'aggiornamento
                    using (var command = new MySqlCommand(updateQuery.ToString(), connection))
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Messaggio in base ai dati aggiornati
                            if (emailChanged && passwordChanged)
                            {
                                MessageBox.Show("L'email e la password sono state aggiornate.");
                            }
                            else if (emailChanged)
                            {
                                MessageBox.Show("L'email è stata aggiornata.");
                            }
                            else
                            {
                                MessageBox.Show("La password è stata aggiornata.");
                            }

                            string Id = Guid.NewGuid().ToString();
                            string CF = UserSession.Instance.CF;
                            bool Letta = false;
                            string Testo = "Hai aggiornato i tuoi dati";
                            string Titolo = "Aggiornamento dati";
                            string queryNotifica = "INSERT INTO notifica (Id, Titolo, Testo, Letta, CF) " +
                                "VALUES (@Id, @Titolo, @Testo, @Letta, @CF)";

                            using (MySqlCommand commandNotifica = new MySqlCommand(queryNotifica, connection))
                            {
                                commandNotifica.Parameters.AddWithValue("@Id", Id);
                                commandNotifica.Parameters.AddWithValue("@Titolo", Titolo);
                                commandNotifica.Parameters.AddWithValue("@Testo", Testo);
                                commandNotifica.Parameters.AddWithValue("@Letta", Letta);
                                commandNotifica.Parameters.AddWithValue("@CF", CF);

                                commandNotifica.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Non è stato possibile aggiornare i dati.", "Errore di aggiornamento", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Nessun campo da aggiornare.", "Nessuna modifica", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }



    }
}
