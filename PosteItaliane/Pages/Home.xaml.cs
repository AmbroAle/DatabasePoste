using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Logica di interazione per Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            string connStr = "server=localhost;uid=root;pwd=;database=PosteItalianeDatabase";
            string query = "SELECT Saldo FROM CARTA WHERE CF = @CF";

            string cfValue = UserSession.Instance.CF;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CF", cfValue);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            SaldoTextBlock.Text = result.ToString();
                        }
                        else
                        {
                            SaldoTextBlock.Text = "Nessun risultato trovato";
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Errore di connessione: " + ex.Message);
            }
        }

        private void BancoMovimenti_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new MovimentiBancoPosta());
        }

        private void btnBonifico_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new BonificoBancoPosta());
        }

        private void btnPrenotazione_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new Prenotazione());
        }

    }
}
