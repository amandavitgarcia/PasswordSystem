using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAds.Application.DTO.Request.Password
{
    // Classe usada para representar os dados de uma requisição PUT relacionada a aplicação

    // Essa classe serve como um modelo de dados para representar uma requisição PUT enviada ao servidor, contendo informações sobre a senha a ser atualizada
    public class PasswordPutRequest
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

    }
}
