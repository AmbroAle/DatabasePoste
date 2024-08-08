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
    /// Logica di interazione per VisualizzaRecensioni.xaml
    /// </summary>
    public partial class VisualizzaRecensioni : Page
    {
        public VisualizzaRecensioni()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData() 
        {
            try
            {
                string connStr = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";
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
            mainWindow?.NavigateToPage(new Home());
        }

        private void btnCerca_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Ottieni l'ID dell'ufficio dal TextBox
                string ufficioIdText = txtIdUfficio.Text;

                // Verifica che l'ID sia valido
                if (int.TryParse(ufficioIdText, out int ufficioId))
                {
                    // 2. Connessione al database
                    string connStr = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";
                    string query = "SELECT Voto, Testo, Data, CF FROM RECENSIONE WHERE Ass_Id = @UfficioId";

                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            // 3. Parametro della query
                            cmd.Parameters.AddWithValue("@UfficioId", ufficioId);

                            // 4. Esegui la query
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                DataTable dataTable = new DataTable();
                                dataTable.Load(reader);

                                // 5. Carica i risultati nella DataGrid
                                RecensioniDataGrid.ItemsSource = dataTable.DefaultView;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Inserisci un ID valido.");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Errore di connessione: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore: " + ex.Message);
            }
        }

    }
}
