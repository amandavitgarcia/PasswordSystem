using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAds.Application.DTO.Request.Password
{
    // Classe que serve como um modelo de dados para representar uma requisição POST enviada ao servidor, contendo informações sobre uma nova senha a ser criada
    public class PasswordPostRequest
    {
        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("EncryptedPassword")]
        public string EncryptedPassword { get; set; }

        [JsonProperty("ActivePassword")]
        public bool ActivePassword { get; set; }
    }
}
