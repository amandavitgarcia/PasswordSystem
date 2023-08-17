using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAds.Application.DTO.Response.Password
{
    public class PasswordResponse
    {
        // 

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("encrypted-password")]
        public string EncryptedPassword { get; set; }
    }
}
