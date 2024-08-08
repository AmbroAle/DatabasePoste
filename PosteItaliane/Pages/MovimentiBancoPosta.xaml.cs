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
    /// Logica di interazione per MovimentiBancoPosta.xaml
    /// </summary>
    public partial class MovimentiBancoPosta : Page
    {
        public MovimentiBancoPosta()
        {
            InitializeComponent();
            LoadMovementBancoPosta();
        }

        private void LoadMovementBancoPosta()
        {
            try
            {
                // La stringa di connessione al database MySQL
                string connStr = "server=localhost;uid=root;pwd=;database=PosteItalianeDatabase";

                // Query con parametro per selezionare i movimenti
                string query = @"
            SELECT
                TRANSAZIONE.*,
                TIPO_TRANSAZIONE.Tipo,
                TIPO_TRANSAZIONE.Commissione,
                TIPO_TRANSAZIONE.TipologiaPagamento,
                TIPO_TRANSAZIONE.Ente,
                TIPO_TRANSAZIONE.IbanDestinatario,
                TIPO_TRANSAZIONE.Causale,
                TIPO_TRANSAZIONE.Id
            FROM
                TRANSAZIONE
            INNER JOIN
                TIPO_TRANSAZIONE ON TRANSAZIONE.CodTransazione = TIPO_TRANSAZIONE.CodTransazione
            WHERE
                TRANSAZIONE.NumeroIdentificativo = @NumeroIdentificativo";

                // Numero identificativo della carta, sostituisci con il valore corretto
                string numeroIdentificativoValue = UserSession.Instance.NumeroIdentificativo; // Inserisci qui il numero identificativo corretto

                // Creazione e apertura della connessione
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // Creazione del comando e aggiunta del parametro
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NumeroIdentificativo", numeroIdentificativoValue);

                        // Esecuzione della query e recupero del risultato
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            // Assuming you have a DataGrid or similar control to display the results
                            MovimentiDataGrid.ItemsSource = dataTable.DefaultView;

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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new Home());
        }
    }

}
