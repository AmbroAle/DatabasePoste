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

        public string CF { get; set; }
        public string NumeroIdentificativo { get; set; }
        public int UserId { get; set; }
        public int Operazione { get; set; }

        private UserSession()
        {
            this.CF = "prova";
            this.NumeroIdentificativo = "";
            this.UserId = -1;
            this.Operazione = 0;
        }

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
