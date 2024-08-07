using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosteItaliane
{
    public class UserSession
    {
        private static UserSession? _instance;

        // La proprietà che contiene il valore del CF
        public string CF { get; set; }
        public string NumeroIdentificativo { get; set; }
        // Costruttore privato per impedire la creazione diretta di istanze
        private UserSession()
        {
            this.CF = "prova";
            this.NumeroIdentificativo = "";
        }

        // Metodo per ottenere l'istanza unica del singleton
        public static UserSession Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserSession();
                }
                return _instance;
            }
        }
    }

}
