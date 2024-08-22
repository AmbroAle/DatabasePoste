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
                string connStr = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";

                // Step 1: Recupera la provincia dell'utente
                string queryProvincia = "SELECT Ind_Provincia FROM UTENTE WHERE CF = @CF";

                string provinciaUtente = "";

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    using (MySqlCommand cmdProvincia = new MySqlCommand(queryProvincia, conn))
                    {
                        // Imposta il CF dell'utente come parametro
                        cmdProvincia.Parameters.AddWithValue("@CF", UserSession.Instance.CF);

                        // Esegui la query per ottenere la provincia
                        provinciaUtente = cmdProvincia.ExecuteScalar()?.ToString() ?? string.Empty;
                    }

                    // Step 2: Se la provincia è stata trovata, carica i dati degli uffici postali
                    if (!string.IsNullOrEmpty(provinciaUtente))
                    {
                        string queryUffici = "SELECT * FROM UFFICIO_POSTALE WHERE Ind_Provincia = @Provincia";

                        using (MySqlCommand cmdUffici = new MySqlCommand(queryUffici, conn))
                        {
                            // Imposta la provincia dell'utente come parametro
                            cmdUffici.Parameters.AddWithValue("@Provincia", provinciaUtente);

                            // Esegui la query e recupera i risultati
                            using (MySqlDataReader reader = cmdUffici.ExecuteReader())
                            {
                                DataTable dataTable = new DataTable();
                                dataTable.Load(reader);

                                // Visualizza i dati in un DataGrid o controllo simile
                                UfficiDataGrid.ItemsSource = dataTable.DefaultView;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Provincia dell'utente non trovata.");
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
