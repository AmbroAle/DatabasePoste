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
    /// Logica di interazione per AdminHome.xaml
    /// </summary>
    public partial class AdminHome : Page
    {
        public AdminHome()
        {
            InitializeComponent();
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new Login());
        }
        private void btnBloccaAccount_Click(object sender, RoutedEventArgs e)
        {
            string cf = txtCFBloccoAccount.Text.Trim();
            if (string.IsNullOrEmpty(cf))
            {
                MessageBox.Show("Inserisci il CF dell'account.");
                return;
            }

            if (UpdateAccountBlockStatus(cf, true))
            {
                MessageBox.Show("Account bloccato con successo.");
            }
            else
            {
                MessageBox.Show("Errore durante il blocco dell'account.");
            }
        }
        private void btnSbloccaAccount_Click(object sender, RoutedEventArgs e)
        {
            string cf = txtCFSbloccoAccount.Text.Trim();
            if (string.IsNullOrEmpty(cf))
            {
                MessageBox.Show("Inserisci il CF dell'account.");
                return;
            }

            if (UpdateAccountBlockStatus(cf, false))
            {
                MessageBox.Show("Account sbloccato con successo.");
            }
            else
            {
                MessageBox.Show("Errore durante lo sblocco dell'account.");
            }
        }
        private void btnBloccaCarta_Click(object sender, RoutedEventArgs e)
        {
            string numeroIdentificativo = txtNumeroIdentificativoBloccaCarta.Text.Trim();
            if (string.IsNullOrEmpty(numeroIdentificativo))
            {
                MessageBox.Show("Inserisci il numero identificativo della carta.");
                return;
            }

            if (UpdateCardBlockStatus(numeroIdentificativo, true))
            {
                MessageBox.Show("Carta bloccata con successo.");
            }
            else
            {
                MessageBox.Show("Errore durante il blocco della carta.");
            }
        }
        private void btnSbloccaCarta_Click(object sender, RoutedEventArgs e)
        {
            string numeroIdentificativo = txtNumeroIdentificativoSbloccaCarta.Text.Trim();
            if (string.IsNullOrEmpty(numeroIdentificativo))
            {
                MessageBox.Show("Inserisci il numero identificativo della carta.");
                return;
            }

            if (UpdateCardBlockStatus(numeroIdentificativo, false))
            {
                MessageBox.Show("Carta sbloccata con successo.");
            }
            else
            {
                MessageBox.Show("Errore durante lo sblocco della carta.");
            }
        }
        private bool UpdateAccountBlockStatus(string cf, bool block)
        {
            try
            {
                string connStr = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";
                string query = "UPDATE ACCOUNT SET BloccoAccount = @BloccoAccount WHERE CF = @CF";

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@BloccoAccount", block);
                    cmd.Parameters.AddWithValue("@CF", cf);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Errore di connessione: " + ex.Message);
                return false;
            }
        }

        private bool UpdateCardBlockStatus(string numeroIdentificativo, bool block)
        {
            try
            {
                string connStr = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";
                string query = "UPDATE CARTA SET BloccoCarta = @BloccoCarta WHERE NumeroIdentificativo = @NumeroIdentificativo";

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@BloccoCarta", block);
                    cmd.Parameters.AddWithValue("@NumeroIdentificativo", numeroIdentificativo);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Errore di connessione: " + ex.Message);
                return false;
            }
        }

        private void btnVisualizzaUtenti_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new VisualizzaUtenti());
        }
    }
}