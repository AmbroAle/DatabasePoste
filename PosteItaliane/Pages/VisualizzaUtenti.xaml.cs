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
    /// Logica di interazione per VisualizzaUtenti.xaml
    /// </summary>
    public partial class VisualizzaUtenti : Page
    {
        public VisualizzaUtenti()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                string CF = UserSession.Instance.CF;
                if (string.IsNullOrEmpty(CF))
                {
                    MessageBox.Show("Codice fiscale non disponibile.");
                    return;
                }

                string connStr = "server=localhost;uid=root;pwd=;database=PosteItalianeDatabase";
                string query = "SELECT * FROM ACCOUNT WHERE Amministratore = 0";

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CF", CF);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            // Debug: Mostra i nomi delle colonne
                            foreach (DataColumn column in dataTable.Columns)
                            {
                                Console.WriteLine($"Colonna: {column.ColumnName}");
                            }

                            // Debug: Mostra i dati delle righe
                            foreach (DataRow row in dataTable.Rows)
                            {
                                Console.WriteLine("Riga:");
                                foreach (DataColumn column in dataTable.Columns)
                                {
                                    Console.WriteLine($"{column.ColumnName}: {row[column]}");
                                }
                            }

                            // Assegna i dati al DataGrid
                            UtentiDataGrid.ItemsSource = dataTable.DefaultView;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Errore di connessione: " + ex.Message);
            }
        }
        private void btnIndietro_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new AdminHome());
        }

    }
}
