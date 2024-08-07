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
            /*string iban = txtIBAN.Text;
            string importo = txtImporto.Text;
            string causale = txtCausale.Text;

            if (string.IsNullOrWhiteSpace(iban) || string.IsNullOrWhiteSpace(importo) || string.IsNullOrWhiteSpace(causale))
            {
                MessageBox.Show("Compilare tutti i campi");
                return;
            }

            if (MakeBonifico(iban, importo, causale))
            {
                MessageBox.Show("Bonifico effettuato con successo!");
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.NavigateToPage(new Home());
            }
            else
            {
                MessageBox.Show("Errore durante il bonifico");
            }*/
        }/*
        private bool MakeBonifico(string iban,float importo, string causale)
        {
            return true;
        }*/
    }
}
