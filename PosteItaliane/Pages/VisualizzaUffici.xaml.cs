using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
    /// Logica di interazione per VisualizzaUffici.xaml
    /// </summary>
    public partial class VisualizzaUffici : Page
    {
        public VisualizzaUffici()
        {
            InitializeComponent();
            LoadData();

        }

        private void LoadData()
        {
            try
            {
                string connStr = "server=localhost;uid=root;pwd=;database=PosteItalianeDatabase";
                string query = "SELECT * FROM UFFICIO_POSTALE";

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // Creazione del comando e aggiunta del parametro
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {

                        // Esecuzione della query e recupero del risultato
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            // Assuming you have a DataGrid or similar control to display the results
                            UfficiDataGrid.ItemsSource = dataTable.DefaultView;

                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Gestione degli errori di connessione e SQL
                MessageBox.Show("Errore di connessione: " + ex.Message);
            }
        }

        private void btnIndietro_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new Prenotazione());
        }
    }
}
