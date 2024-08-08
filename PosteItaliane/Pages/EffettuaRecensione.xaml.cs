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
    /// Logica di interazione per EffettuaRecensione.xaml
    /// </summary>
    public partial class EffettuaRecensione : Page
    {
        public EffettuaRecensione()
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
            mainWindow?.NavigateToPage(new Prenotazione());
        }

        private void btnEffettuaRecensione_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string idUfficioText = txtIdUfficio.Text.Trim();
                string descrizione = txtDescrizione.Text.Trim();
                string votoText = txtVoto.Text.Trim();

                if (string.IsNullOrWhiteSpace(idUfficioText) || string.IsNullOrWhiteSpace(descrizione) || string.IsNullOrWhiteSpace(votoText))
                {
                    MessageBox.Show("Tutti i campi sono obbligatori.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(votoText, out int voto) || voto < 1 || voto > 5)
                {
                    MessageBox.Show("Il voto deve essere un numero tra 1 e 5.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(idUfficioText, out int ufficioId))
                {
                    MessageBox.Show("ID Ufficio non valido. Assicurati che sia un numero intero valido.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string connStr = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";

                // Trova il valore massimo attuale di Id
                int nextId;
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string maxIdQuery = "SELECT COALESCE(MAX(Id), 0) + 1 FROM RECENSIONE";
                    using (MySqlCommand cmd = new MySqlCommand(maxIdQuery, conn))
                    {
                        nextId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                // Inserisci la recensione con il nuovo Id
                string query = "INSERT INTO RECENSIONE (Id, Testo, Voto, Data, CF, Ass_Id) VALUES (@id, @testo, @voto, @data, @cf, @ass_id)";

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", nextId);
                        cmd.Parameters.AddWithValue("@testo", descrizione);
                        cmd.Parameters.AddWithValue("@voto", voto);
                        cmd.Parameters.AddWithValue("@data", DateTime.Now);
                        cmd.Parameters.AddWithValue("@cf", UserSession.Instance.CF);
                        cmd.Parameters.AddWithValue("@ass_id", ufficioId);

                        try
                        {
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Recensione registrata con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show("Errore durante l'inserimento della recensione: " + ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Si è verificato un errore: " + ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
