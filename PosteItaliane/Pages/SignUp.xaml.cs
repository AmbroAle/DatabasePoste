using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
using System.Xml.Linq;


namespace PosteItaliane.Pages
{
    /// <summary>
    /// Logica di interazione per SignUp.xaml
    /// </summary>
    public partial class SignUp : Page
    {
        public SignUp()
        {
            InitializeComponent();
        }
        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(new Login());
        }

        private void txtCF_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string codiceFiscale = txtCF.Text;
            string nome = txtName.Text;
            string cognome = txtSurname.Text;
            string civico = txtCivic.Text;
            string via = txtStreet.Text;
            string provincia = txtProvince.Text;
            string comune = txtCity.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            if (RegisterUser(codiceFiscale, nome, cognome, civico, via, provincia, comune, email, password))
            {
                MessageBox.Show("Registrazione avvenuta con successo!");
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.NavigateToPage(new Login());
            }
            else
            {
                MessageBox.Show("Errore durante la registrazione.");
            }
        }
        private bool RegisterUser(string CF, string nome, string cognome, string civico, string via, string provincia, string comune, string email, string password)
        {
            try
            {
                string connStr = "server=localhost;uid=root;pwd=;database=PosteItalianeDatabase";
                string query = "INSERT INTO UTENTE (CF, Nome, Cognome, Ind_Via, Ind_Civico, Ind_Residenza, Ind_Provincia) " +
                               "VALUES (@CF, @Nome, @Cognome, @Ind_Via, @Ind_Civico, @Ind_Residenza, @Ind_Provincia)";
                string query2 = "INSERT INTO ACCOUNT (CF, BloccoAccount, Email, Password) " +
                                "VALUES (@CF, @BloccoAccount, @Email, @Password)";

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    using (MySqlCommand cmd1 = new MySqlCommand(query, conn))
                    {
                        cmd1.Parameters.AddWithValue("@CF", CF);
                        cmd1.Parameters.AddWithValue("@Nome", nome);
                        cmd1.Parameters.AddWithValue("@Cognome", cognome);
                        cmd1.Parameters.AddWithValue("@Ind_Via", via);
                        cmd1.Parameters.AddWithValue("@Ind_Civico", civico);
                        cmd1.Parameters.AddWithValue("@Ind_Residenza", comune);
                        cmd1.Parameters.AddWithValue("@Ind_Provincia", provincia);

                        cmd1.ExecuteNonQuery();
                    }

                    using (MySqlCommand cmd2 = new MySqlCommand(query2, conn))
                    {
                        cmd2.Parameters.AddWithValue("@CF", CF);
                        cmd2.Parameters.AddWithValue("@BloccoAccount", false);
                        cmd2.Parameters.AddWithValue("@Email", email);
                        cmd2.Parameters.AddWithValue("@Password", password);

                        cmd2.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Errore di connessione: " + ex.Message);
                return false;
            }
        }
    }
}