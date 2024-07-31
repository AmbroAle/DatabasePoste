using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PosteItaliane
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectDatabase();
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectDatabase();
        }

        private void ConnectDatabase()
        {
            try
            {
                string connStr = "server=localhost;uid=root;pwd=;database=PosteItalianeProgettazioneLogica";
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MessageBox.Show("Connessione riuscita!");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Errore di connessione: " + ex.Message);
            }
        }
        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        private void InsertTestRow(MySqlConnection conn)
        {
            try
            {
                string insertSql = "INSERT INTO utente (CF, Nome, Cognome, Ind_Via,Ind_Civico,Ind_Residenza,Ind_Provincia) " +
                                   "VALUES ('AMGFSDGW34DSRT34', 'Alessandro','Ambrogiani', 'Via dei lelli', '5a','Prova','RN');";
                MySqlCommand insertCmd = new MySqlCommand(insertSql, conn);
                int rowsAffected = insertCmd.ExecuteNonQuery();
                MessageBox.Show(rowsAffected + " riga(e) inserita(e) nella tabella account.");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Errore durante l'inserimento: " + ex.Message);
            }
        }

        private void TextBlock_Registrati_MouseDown(object sender, MouseButtonEventArgs e)
        {

             SignUp registrationWindow = new SignUp();
            registrationWindow.Show();
        }

    }
}