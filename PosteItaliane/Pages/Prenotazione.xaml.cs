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
    /// Logica di interazione per Prenotazione.xaml
    /// </summary>
    public partial class Prenotazione : Page
    {
        public Prenotazione()
        {
            InitializeComponent();
        }

        private void btnIndietro_Click(object sender, RoutedEventArgs e) 
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new Home());
        }

        private void btnVisualizzaUfficiOperazione_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new VisualizzaUfficiOperazione());
        }

        private void btnVisualizzaUffici_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new VisualizzaUffici());
        }

        private void btnVisualizzaUfficiRecensione_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new VisualizzaUfficiRecensione());
        }

        private void btnEffettuaRecensione_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new EffettuaRecensione());
        }
    }
}
