using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAds.Application.DTO.Response.Password
{
    // Classe de resposta da API
    public class ApiGetPasswordResponse
    {
        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("DecryptedPassword")]
        public string DecryptedPassword { get; set; }

        [JsonProperty("EncryptedPassword")]
        public string EncryptedPassword { get; set; }

    }
}
