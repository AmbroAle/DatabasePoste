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
                string connStr = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";

                // Query originale per selezionare i movimenti
                string queryMovimenti = @"
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

                // Query aggiuntiva per calcolare il totale degli importi per tipo
                string queryTotalePerTipo = @"
            SELECT
                tt.Tipo,
                SUM(t.Importo) AS TotaleImporto
            FROM
                TRANSAZIONE t
            INNER JOIN
                TIPO_TRANSAZIONE tt ON t.CodTransazione = tt.CodTransazione
            WHERE
                t.NumeroIdentificativo = @NumeroIdentificativo
            GROUP BY
                tt.Tipo
            ORDER BY
                TotaleImporto DESC";

                // Numero identificativo della carta
                string numeroIdentificativoValue = UserSession.Instance.NumeroIdentificativo; // Assicurati che questa proprietà esista e sia correttamente valorizzata

                // Creazione e apertura della connessione
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // Esecuzione della prima query per selezionare i movimenti
                    using (MySqlCommand cmdMovimenti = new MySqlCommand(queryMovimenti, conn))
                    {
                        cmdMovimenti.Parameters.AddWithValue("@NumeroIdentificativo", numeroIdentificativoValue);

                        using (MySqlDataReader reader = cmdMovimenti.ExecuteReader())
                        {
                            DataTable dataTableMovimenti = new DataTable();
                            dataTableMovimenti.Load(reader);

                            // Visualizzazione dei movimenti nella grid MovimentiDataGrid
                            MovimentiDataGrid.ItemsSource = dataTableMovimenti.DefaultView;
                        }
                    }

                    // Esecuzione della seconda query per calcolare il totale per tipo
                    using (MySqlCommand cmdTotale = new MySqlCommand(queryTotalePerTipo, conn))
                    {
                        cmdTotale.Parameters.AddWithValue("@NumeroIdentificativo", numeroIdentificativoValue);

                        using (MySqlDataReader readerTotale = cmdTotale.ExecuteReader())
                        {
                            DataTable dataTableTotale = new DataTable();
                            dataTableTotale.Load(readerTotale);

                            // Visualizzazione dei totali nella grid TotaleTipoDataGrid
                            TotaleTipoDataGrid.ItemsSource = dataTableTotale.DefaultView;
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
