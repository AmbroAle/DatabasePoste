using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
    /// Logica di interazione per Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void TextBlock_Registrati_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new SignUp());
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtUser.Text;
            string password = txtPass.Password;

            var loginDetails = ConnectDatabase(email, password);

            if (loginDetails.HasValue)
            {
                var (isAdmin, cf, isBlocked) = loginDetails.Value;

                if (isBlocked)
                {
                    MessageBox.Show("Il tuo account è stato bloccato.");
                }
                else
                {
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    if (isAdmin)
                    {
                        mainWindow?.NavigateToPage(new AdminHome());
                    }
                    else
                    {
                        mainWindow?.NavigateToPage(new Home());
                    }
                }
            }
            else
            {
                MessageBox.Show("Email o password errati");
            }
        }



        private (bool IsAdmin, string CF, bool IsBlocked)? ConnectDatabase(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            try
            {
                string connStr = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";
                string query = "" +
                    "SELECT Password, CF, Amministratore, BloccoAccount " +
                    "FROM ACCOUNT " +
                    "WHERE Email = @Email";
                string queryCarta = "" +
                    "SELECT NumeroIdentificativo " +
                    "FROM CARTA " +
                    "WHERE CF = @CF " +
                    "AND Tipo = 'BancoPosta'";

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPassword = reader.GetString("Password");
                            string cf = reader.GetString("CF");
                            bool isAdmin = reader.GetBoolean("Amministratore");
                            bool isBlocked = reader.GetBoolean("BloccoAccount");

                            if (storedPassword == password)
                            {
                                // Imposta il CF nella UserSession
                                UserSession.Instance.CF = cf;
                                reader.Close();

                                MySqlCommand cmdCarta = new MySqlCommand(queryCarta, conn);
                                cmdCarta.Parameters.AddWithValue("@CF", cf);
                                using (MySqlDataReader readerCarta = cmdCarta.ExecuteReader())
                                {
                                    if (readerCarta.Read())
                                    {
                                        string NumeroIdentificativo = readerCarta.GetString("NumeroIdentificativo");
                                        UserSession.Instance.NumeroIdentificativo = NumeroIdentificativo;
                                    }
                                }

                                return (isAdmin, cf, isBlocked);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Errore di connessione: " + ex.Message);
            }

            return null;
        }


    }
}