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
    /// Logica di interazione per VisualizzaUfficiRecensione.xaml
    /// </summary>
    public partial class VisualizzaUfficiRecensione : Page
    {
        public VisualizzaUfficiRecensione()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string connStr = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";
                string query = @"
            SELECT 
                u.Id,
                u.Ind_Via,
                u.Ind_Civico,
                u.Ind_Comune,
                u.Ind_Regione,
                u.Ind_Provincia,
                u.Apertura,
                u.Chiusura,
                COALESCE(SUM(r.Voto), 0) AS TotaleVoti
            FROM 
                UFFICIO_POSTALE u
            LEFT JOIN 
                RECENSIONE r ON u.Id = r.Ass_Id
            GROUP BY 
                u.Id, u.Ind_Via, u.Ind_Civico, u.Ind_Comune, u.Ind_Regione, u.Ind_Provincia, u.Apertura, u.Chiusura
            ORDER BY 
                TotaleVoti DESC;";

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
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
