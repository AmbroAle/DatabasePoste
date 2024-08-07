using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace PosteItaliane.Pages
{
    /// <summary>
    /// Logica di interazione per VisualizzaUfficiOperazione.xaml
    /// </summary>
    public partial class VisualizzaUfficiOperazione : Page
    {
        public VisualizzaUfficiOperazione()
        {
            InitializeComponent();
        }

        private void btnBollettino_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // La stringa di connessione al database MySQL
                string connStr = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";

                // Query con parametro per selezionare i movimenti
                string query = @"
                            SELECT u.Id, u.Loc_Id, u.Ind_Via, u.Ind_Civico, u.Ind_Comune, u.Ind_Regione, u.Ind_Provincia, u.Apertura, u.Chiusura 
                            FROM UFFICIO_POSTALE u
                            INNER JOIN AUTORIZZAZIONE a ON u.Id = a.Sed_Id
                            WHERE a.Id = @id";

                int operazione = 1;

                // Creazione e apertura della connessione
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // Creazione del comando e aggiunta del parametro
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", operazione);

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
            catch (Exception ex)
            {
                // Simple error notification
                MessageBox.Show("Errore durante il caricamento dei dati: " + ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnPostaPacchi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // La stringa di connessione al database MySQL
                string connStr = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";

                // Query con parametro per selezionare i movimenti
                string query = @"
                            SELECT u.Id, u.Loc_Id, u.Ind_Via, u.Ind_Civico, u.Ind_Comune, u.Ind_Regione, u.Ind_Provincia, u.Apertura, u.Chiusura 
                            FROM UFFICIO_POSTALE u
                            INNER JOIN AUTORIZZAZIONE a ON u.Id = a.Sed_Id
                            WHERE a.Id = @id";

                int operazione = 2;

                // Creazione e apertura della connessione
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // Creazione del comando e aggiunta del parametro
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", operazione);

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
            catch (Exception ex)
            {
                // Simple error notification
                MessageBox.Show("Errore durante il caricamento dei dati: " + ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnVersamentiAltro_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // La stringa di connessione al database MySQL
                string connStr = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";

                // Query con parametro per selezionare i movimenti
                string query = @"
                            SELECT u.Id, u.Loc_Id, u.Ind_Via, u.Ind_Civico, u.Ind_Comune, u.Ind_Regione, u.Ind_Provincia, u.Apertura, u.Chiusura 
                            FROM UFFICIO_POSTALE u
                            INNER JOIN AUTORIZZAZIONE a ON u.Id = a.Sed_Id
                            WHERE a.Id = @id";

                int operazione = 3;

                // Creazione e apertura della connessione
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // Creazione del comando e aggiunta del parametro
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", operazione);

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
            catch (Exception ex)
            {
                // Simple error notification
                MessageBox.Show("Errore durante il caricamento dei dati: " + ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSPID_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // La stringa di connessione al database MySQL
                string connStr = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";

                // Query con parametro per selezionare i movimenti
                string query = @"
                            SELECT u.Id, u.Loc_Id, u.Ind_Via, u.Ind_Civico, u.Ind_Comune, u.Ind_Regione, u.Ind_Provincia, u.Apertura, u.Chiusura 
                            FROM UFFICIO_POSTALE u
                            INNER JOIN AUTORIZZAZIONE a ON u.Id = a.Sed_Id
                            WHERE a.Id = @id";

                int operazione = 4;

                // Creazione e apertura della connessione
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // Creazione del comando e aggiunta del parametro
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", operazione);

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
            catch (Exception ex)
            {
                // Simple error notification
                MessageBox.Show("Errore durante il caricamento dei dati: " + ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAltro_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // La stringa di connessione al database MySQL
                string connStr = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";

                // Query con parametro per selezionare i movimenti
                string query = @"
                            SELECT u.Id, u.Loc_Id, u.Ind_Via, u.Ind_Civico, u.Ind_Comune, u.Ind_Regione, u.Ind_Provincia, u.Apertura, u.Chiusura 
                            FROM UFFICIO_POSTALE u
                            INNER JOIN AUTORIZZAZIONE a ON u.Id = a.Sed_Id
                            WHERE a.Id = @id";

                int operazione = 5;

                // Creazione e apertura della connessione
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // Creazione del comando e aggiunta del parametro
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", operazione);

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
            catch (Exception ex)
            {
                // Simple error notification
                MessageBox.Show("Errore durante il caricamento dei dati: " + ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnIndietro_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new Prenotazione());
        }
    }
}

