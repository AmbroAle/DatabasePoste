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
    /// Logica di interazione per TransazioniUfficio.xaml
    /// </summary>
    public partial class TransazioniUfficio : Page
    {
        public TransazioniUfficio()
        {
            InitializeComponent();
        }

        private void btnIndietro_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new Home());
        }

        private void btnVisualizza_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";
            string comune = Comune.Text;  // Ottieni il valore dal TextBox

            string query = @"
                SELECT t.CodTransazione, t.Importo, tt.Tipo, t.Data, u.Ind_Comune, u.Ind_Regione
                FROM TRANSAZIONE t
                INNER JOIN TIPO_TRANSAZIONE tt ON t.CodTransazione = tt.CodTransazione
                INNER JOIN BANCOMAT b ON tt.Id = b.Id
                INNER JOIN UFFICIO_POSTALE u ON b.Id = u.Loc_Id
                WHERE u.Ind_Comune = @comune;";

            DataTable dataTable = new DataTable();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@comune", comune);

                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        adapter.Fill(dataTable);
                    }

                    // Visualizzare i dati nella DataGrid
                    TransazioniUfficioGrid.ItemsSource = dataTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Errore durante il caricamento dei dati: " + ex.Message);
                }
            }
        }
    }
}
