using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Logica di interazione per SpesaMensile.xaml
    /// </summary>
    public partial class SpesaMensile : Page
    {
        public SpesaMensile()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            string connectionString = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase"; 
            DateTime now = DateTime.Now;
            DateTime startOfMonth = new DateTime(now.Year, now.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            string query = @"
                SELECT t.CodTransazione, t.Importo, t.Data, tt.Tipo 
                FROM TRANSAZIONE t
                INNER JOIN TIPO_TRANSAZIONE tt ON t.CodTransazione = tt.CodTransazione
                WHERE t.Data >= @startOfMonth AND t.Data <= @endOfMonth AND t.NumeroIdentificativo = @numeroIdentificativo;";

            DataTable dataTable = new DataTable();
            decimal totalImporto = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@startOfMonth", startOfMonth);
                        command.Parameters.AddWithValue("@endOfMonth", endOfMonth);
                        command.Parameters.AddWithValue("@numeroIdentificativo", UserSession.Instance.NumeroIdentificativo);

                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        adapter.Fill(dataTable);
                    }

                    // Calcolo dell'importo totale
                    foreach (DataRow row in dataTable.Rows)
                    {
                        totalImporto += Convert.ToDecimal(row["Importo"]);
                    }

                    // Visualizzare i dati nella MovimentiDataGrid
                    MovimentiDataGrid.ItemsSource = dataTable.DefaultView;

                    // Mostrare l'importo totale in una label o in un altro controllo
                    ImportoTotale.Text = $"Importo Totale: {totalImporto:C}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Errore durante il caricamento dei dati: " + ex.Message);
                }
            }
        }

        private void btnVisualizza_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new Home());
        }
    }
}
