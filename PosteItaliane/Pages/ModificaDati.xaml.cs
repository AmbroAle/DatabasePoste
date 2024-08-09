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
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Valori dei campi
                string email = txtEmail.Text.Trim();//Trim rimuove gli spazi bianchi all'inizio e alla fine di una stringa
                string password = txtPassword.Password.Trim();

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
                        if (emailChanged && passwordChanged)
                        {
                            MessageBox.Show("La email e la password sono state aggiornate");
                        }
                        else if (passwordChanged == false && emailChanged == true)
                        {
                            MessageBox.Show("La email è stata aggiornata");
                        }
                        else
                        {
                            MessageBox.Show("La password è stata aggiornata");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Nessun campo da aggiornare.");
                }
            }
        }


    }
}
