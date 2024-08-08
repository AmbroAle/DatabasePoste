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
            UserSession.Instance.Operazione = 1;
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
            UserSession.Instance.Operazione = 2;
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
            UserSession.Instance.Operazione = 3;
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
            UserSession.Instance.Operazione = 4;
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
            UserSession.Instance.Operazione = 5;
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



        private void btnEffettuaPrenotazione_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Ottieni i valori dai TextBox
                int idUfficio = int.Parse(txtIdUfficio.Text);
                DateTime dataPrenotazione = DateTime.Parse(txtDataTime.Text); // Data e ora inseriti dall'utente

                // Verifica se la data è entro due settimane dalla data attuale
                DateTime dataMassima = DateTime.Now.AddDays(14);
                if (dataPrenotazione > dataMassima)
                {
                    MessageBox.Show("La prenotazione può essere fatta solo entro due settimane dalla data attuale.");
                    return;
                }

                // Connessione al database
                string connectionString = "server=localhost;uid=root;pwd=8323;database=PosteItalianeDatabase";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Controlla se l'utente ha già una prenotazione attiva
                    string cfUtente = UserSession.Instance.CF; // Puoi sostituirlo con il CF dell'utente attualmente loggato
                    string queryVerifica = "SELECT COUNT(*) FROM PRENOTAZIONE WHERE CF = @CF";
                    using (MySqlCommand cmdVerifica = new MySqlCommand(queryVerifica, connection))
                    {
                        cmdVerifica.Parameters.AddWithValue("@CF", cfUtente);
                        int prenotazioniAttive = Convert.ToInt32(cmdVerifica.ExecuteScalar());
                        if (prenotazioniAttive > 0)
                        {
                            MessageBox.Show("Hai già una prenotazione attiva.");
                            return;
                        }
                    }

                    // Recupera gli orari di apertura e chiusura dell'ufficio
                    string queryOrari = "SELECT Apertura, Chiusura FROM UFFICIO_POSTALE WHERE Id = @IdUfficio";
                    TimeSpan orarioApertura, orarioChiusura;

                    using (MySqlCommand cmdOrari = new MySqlCommand(queryOrari, connection))
                    {
                        cmdOrari.Parameters.AddWithValue("@IdUfficio", idUfficio);
                        using (MySqlDataReader reader = cmdOrari.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                orarioApertura = reader.GetTimeSpan(0);
                                orarioChiusura = reader.GetTimeSpan(1);
                            }
                            else
                            {
                                MessageBox.Show("Ufficio non trovato.");
                                return;
                            }
                        }
                    }

                    // Controlla se l'orario della prenotazione è valido
                    TimeSpan orarioPrenotazione = dataPrenotazione.TimeOfDay;
                    if (orarioPrenotazione < orarioApertura || orarioPrenotazione > orarioChiusura || (orarioPrenotazione.TotalMinutes % 30) != 0)
                    {
                        MessageBox.Show("Orario non valido. La prenotazione può essere effettuata solo a intervalli di 30 minuti durante gli orari di apertura dell'ufficio.");
                        return;
                    }

                    

                    // Verifica se l'ID dell'operazione esiste
                    string queryVerificaOperazione = "SELECT COUNT(*) FROM OPERAZIONE WHERE Id = @IdOperazione";
                    using (MySqlCommand cmdVerificaOperazione = new MySqlCommand(queryVerificaOperazione, connection))
                    {
                        cmdVerificaOperazione.Parameters.AddWithValue("@IdOperazione", UserSession.Instance.Operazione);
                        int operazioniEsistenti = Convert.ToInt32(cmdVerificaOperazione.ExecuteScalar());
                        if (operazioniEsistenti == 0)
                        {
                            MessageBox.Show("ID operazione non valido.");
                            return;
                        }
                    }

                    

                    // Inserisci la prenotazione nel database
                    string queryInsert = "INSERT INTO PRENOTAZIONE (Ticket, Data, Sed_Id, Id, CF) VALUES (@Ticket, @Data, @SedId, @Id, @CF)";
                    using (MySqlCommand cmdInsert = new MySqlCommand(queryInsert, connection))
                    {
                        cmdInsert.Parameters.AddWithValue("@Ticket", Guid.NewGuid().ToString());
                        cmdInsert.Parameters.AddWithValue("@Data", dataPrenotazione);
                        cmdInsert.Parameters.AddWithValue("@SedId", idUfficio); // ID della sede
                        cmdInsert.Parameters.AddWithValue("@Id", UserSession.Instance.Operazione); // ID dell'operazione
                        cmdInsert.Parameters.AddWithValue("@CF", cfUtente);

                        cmdInsert.ExecuteNonQuery();
                    }

                    MessageBox.Show("Prenotazione effettuata con successo.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore: " + ex.Message);
            }
        }


    }
}

