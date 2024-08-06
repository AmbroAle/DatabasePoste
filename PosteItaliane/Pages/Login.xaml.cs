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
            if (ConnectDatabase(email, password))
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.NavigateToPage(new Home());
            }
            else
            {
                MessageBox.Show("Email o password errati");
            }

        }
        private bool ConnectDatabase(string email, string password)
        {
            try
            {
                string connStr = "server=localhost;uid=root;pwd=;database=PosteItalianeDatabase";
                string query = "SELECT Password FROM ACCOUNT WHERE Email = @Email";

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", email);

                    object result = cmd.ExecuteScalar();
                    string storedPassword = result as string ?? string.Empty;

                    if (storedPassword != null && storedPassword == password)
                    {
                        return true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Errore di connessione: " + ex.Message);
            }
            return false;

        }



    }
}
