using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAds.Domain.Entities
{
    // Classe que reflete o Banco de Dados
    public class Password
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Encrypted_password { get; set; }
        public bool Active_Password { get; set; }
        public DateTime Data_Creation { get; set; }
        public DateTime Update_Date { get; set; }
    }
}
