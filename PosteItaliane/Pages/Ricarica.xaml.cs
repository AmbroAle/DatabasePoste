using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
    public partial class Ricarica : Page
    {
        public Ricarica()
        {
            InitializeComponent();
            LoadCards();
        }

        private void LoadCards()
        {
            string connectionString = "server=localhost;uid=root;pwd=;database=PosteItalianeDatabase";
            string query = "SELECT Iban FROM CARTA WHERE NumeroIdentificativo = @NumeroIdentificativo AND Tipo = 'PostePay'";
            string numeroIdentificativo = UserSession.Instance.NumeroIdentificativo;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NumeroIdentificativo", numeroIdentificativo);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            TipoBonificoComboBox.Items.Clear(); // Pulisci gli elementi esistenti
                            while (reader.Read())
                            {
                                // Usare ?? "" per garantire che non ci siano valori nulli
                                string iban = reader["Iban"].ToString() ?? "";
                                ComboBoxItem item = new ComboBoxItem
                                {
                                    Content = iban
                                };
                                TipoBonificoComboBox.Items.Add(item);
                            }

                            if (TipoBonificoComboBox.Items.Count == 0)
                            {
                                MessageBox.Show("Non hai carte PostePay disponibili.", "Informazione", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante il caricamento delle carte: {ex.Message}", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new Home());
        }

        private void btnRicarica_Click(object sender, RoutedEventArgs e)
        {
            if (TipoBonificoComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string iban = selectedItem.Content.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(iban))
                {
                    MessageBox.Show("Seleziona un IBAN valido.", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string importoText = txtImporto.Text;

                if (!decimal.TryParse(importoText, out decimal importo) || importo <= 0)
                {
                    MessageBox.Show("Inserisci un importo valido e maggiore di zero.", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Logica per effettuare la ricarica (da implementare)
                MessageBox.Show($"Ricarica di {importo:C} effettuata sulla carta con IBAN: {iban}", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Seleziona una carta PostePay.", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}