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
            ConnectDatabase();
        }

        private void ConnectDatabase()
        {
            try
            {
                string connStr = "server=localhost;uid=root;pwd=8323;database=Poste";
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MessageBox.Show("Connessione riuscita,molto bene!");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Errore di connessione: " + ex.Message);
            }
        }
    }
}
