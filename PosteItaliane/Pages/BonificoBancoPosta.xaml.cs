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
    /// <summary>
    /// Logica di interazione per BonificoBancoPosta.xaml
    /// </summary>
    public partial class BonificoBancoPosta : Page
    {
        public BonificoBancoPosta()
        {
            InitializeComponent();
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new Home());
        }
        private void btnBonifico_Click(object sender, RoutedEventArgs e)
        {
            string iban = txtIBAN.Text;
            string importo = txtImporto.Text;
            string causale = txtCausale.Text;
            ComboBoxItem selectedItem = (ComboBoxItem)TipoBonificoComboBox.SelectedItem;

            // Verifica se è stato selezionato un valore
            if (selectedItem == null)
            {
                MessageBox.Show("Seleziona un tipo di bonifico.");
                return;
            }
            //Controllo se tutti i campi sono stati compilati
            if (string.IsNullOrWhiteSpace(iban) || string.IsNullOrWhiteSpace(importo) || string.IsNullOrWhiteSpace(causale))
            {
                MessageBox.Show("Compilare tutti i campi.");
                return;
            }

            // Controllo se l'importo è un valore valido
            if (!float.TryParse(importo, out float importoValue) || importoValue <= 0)
            {
                MessageBox.Show("L'importo deve essere un numero positivo.");
                return;
            }

            // Chiamata al metodo MakeBonifico
            if (MakeBonifico(iban, importoValue, causale))
            {
                MessageBox.Show("Bonifico effettuato con successo!");
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.NavigateToPage(new Home());
            }
            else
            {
                MessageBox.Show("Errore durante il bonifico.");
            }
        }
        private bool MakeBonifico(string iban, float importo, string causale)
        {
            return true;
        }
    }
}
