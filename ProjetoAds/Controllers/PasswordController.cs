using Microsoft.AspNetCore.Mvc;
using ProjetoAds.Application.DTO.Request.Password;
using ProjetoAds.Application.DTO.Response.Password;
using ProjetoAds.Domain.Entities;
using ProjetoAds.Domain.Repository;
using System.Text;

namespace ProjetoAds.Controllers
{
    [Produces("application/json")]

    //indica rota da controller
    [Route("password/home")]

    // Herança que permite que PasswordController herde as propriedades e métodos definidos em BaseController.
    public class PasswordController : BaseController
    {
        // Variável usada para injetar dependência do tipo IPasswordRepository no construtor da classe.
        private readonly IPasswordRepository _passwordRepository;

        public PasswordController(IPasswordRepository passwordRepository)
        {
            _passwordRepository = passwordRepository;

        }

        // Endpoint de contingencia 
        [HttpPost("encrypt-maintenance")]
        public void MaintenancePasswordEncrypt()
        {
            // Pega do Banco de Dado 
            var passwordList = _passwordRepository.GetAll();

            // Percorre a lista de senhas
            foreach (var password in passwordList)
            {

                var decryptedPassword = "";
                try
                {
                    // Descriptografa a senha
                    decryptedPassword = DecryptPassword(password.Encrypted_password);
                }
                catch (Exception ex)
                {
                    password.Encrypted_password = EncryptPassword(password.Encrypted_password);
                    _passwordRepository.Update(password);
                }
            }

        }

        // Endpoint que busca por ID
        [HttpGet("get-by-id/{id}/{requestPassword}")]
        public ApiGetPasswordResponse GetById(int id, string requestPassword)
        {
            // Informando a senha de acesso baseando-se na Description que acessada pelo método GetByDescription
            var a = _passwordRepository.GetByDescription("Senha Api");

            // Pegando a senha que vem do banco criptografada e descriptografando ela
            var b = DecryptPassword(a.Encrypted_password);

            // Verificando se a senha informada é a mesma senha do banco descriptografada
            if (requestPassword == b)
            {
                // Pega do Banco de Dados
                var password = _passwordRepository.GetById(id);

                // Recebendo método para descriptografar a senha armazenada na propriedade (q vem do banco de dados) e salvando em uma variavel 
                var decrypt = DecryptPassword(password.Encrypted_password);

                // Utilizando o método Select do LINQ para projetar o objeto password em um novo objeto ApiGetPasswordResponse
                var response = new ApiGetPasswordResponse
                {
                    // Atribui valor para as propriedades da classe de resposta (famoso Mapper)
                    Description = password.Description,
                    DecryptedPassword = decrypt, // Recebe a variavel que descriptografa a senha
                    EncryptedPassword = password.Encrypted_password // Recebe a variavel que criptografa a senha
                };

                // Retorna os valores projetados
                return response;
            }

            // Senha diferente retorna valores nulos
            return new ApiGetPasswordResponse();
        }

        // Endpoint que retorna todos os dados
        [HttpGet("get-all/{requestPassword}")]
        public List<ApiGetPasswordResponse> GetAll(string requestPassword)
        {
            // Pega do Banco de Dado 
            var passwordList = _passwordRepository.GetAll();

            // Lista para armazenar as senhas descriptografadas
            var decryptedPasswords = new List<ApiGetPasswordResponse>();

            var a = _passwordRepository.GetByDescription("Senha Api");

            var b = DecryptPassword(a.Encrypted_password);

            if (requestPassword == b)
            {
                // Percorre a lista de senhas
                foreach (var password in passwordList)
                {
                    // Descriptografa a senha
                    var decryptedPassword = DecryptPassword(password.Encrypted_password);

                    // Cria um novo objeto ApiGetPasswordResponse com a senha descriptografada
                    var response = new ApiGetPasswordResponse
                    {
                        Description = password.Description,
                        DecryptedPassword = decryptedPassword, // Recebe a variavel que descriptografa a senha
                        EncryptedPassword = password.Encrypted_password // Recebe a variavel que criptografa a senha
                    };

                    // Adiciona a senha descriptografada à lista
                    decryptedPasswords.Add(response);
                }

                // Retornando a lista de senhas descriptografadas
                return decryptedPasswords;
            }
                return new List<ApiGetPasswordResponse>();
        }

        // INSERT POST com criptografia
        [HttpPost("post/{requestPassword}")]
        public void Post([FromBody] PasswordPostRequest request, [FromRoute] string requestPassword)
        {
            // Criptografando a senha
            string encryptedPassword = EncryptPassword(request.EncryptedPassword);

            // Classe de dominio 
            var password = new Password
            {
                // Populando classe de domínio, atribuindo os valores
                Description = request.Description,
                Encrypted_password = encryptedPassword,
                Active_Password = request.ActivePassword,
            };

            _passwordRepository.Insert(password);


        }

        // UPDATE (passo o ID pelo Json)
        [HttpPut("put-by-id/{requestPassword}")]
        public void Put([FromBody] PasswordPutRequest request, [FromRoute] string requestPassword)
        {
            var a = _passwordRepository.GetByDescription("Senha Api");
            var b = DecryptPassword(a.Encrypted_password);

            if (requestPassword == b)
            {
                var password = _passwordRepository.GetById(request.Id);

                if (password == null)
                {
                    throw new Exception();
                }

                if (request.Description != null)
                {
                    password.Description = request.Description;
                }

                if (request.Password != null)
                {
                    password.Encrypted_password = EncryptPassword(request.Password);

                }

                _passwordRepository.Update(password);
            }
            else
            {
                throw new Exception("Invalid password. Acesso negado!");
            }
        }

        // DELETE 
        [HttpDelete("delete-by-id/{id}/{requestPassword}")]
        public void DeleteById(int id, string requestPassword)
        {
            var a = _passwordRepository.GetByDescription("Senha Api");
            var b = DecryptPassword(a.Encrypted_password);

            if (requestPassword == b)
            {

                // Chamando do repositório o método Delete
                _passwordRepository.Delete(id);

            }
            else
            {
                 throw new Exception("Senha incorreta.\nDelete negado!");
            }
            
        }

        // Endpoint Filtro c/ LINQ
        [HttpGet("search-by-linq/{filter}")]
        public List<ApiGetPasswordResponse> SearchByLinq(string filter)
        {
            // Pega do Banco de Dado 
            var passwordList = _passwordRepository.GetAll();

            var passwordListFiltered = passwordList.Where(descriptionName => descriptionName.Description.ToUpper().Contains(filter.ToUpper())).ToList();
            var response = new List<ApiGetPasswordResponse>();

            // Verifica se a lista tem itens
            if (passwordListFiltered.Any())
            {
                // utilizando o método Select do LINQ para projetar cada objeto password da lista passwordList em um novo objeto ApiGetPasswordResponse
                response = passwordListFiltered.Select(password => new ApiGetPasswordResponse
                {
                    // Mapper 

                    Description = password.Description,
                    DecryptedPassword = password.Encrypted_password
                }).ToList();

            }
            // Retornando a lista de resposta
            return response;
        }

        // Endpoint de paginação com LINQ
        [HttpGet("pagination/{pageSize}/{pageNumber}")]
        public List<ApiGetPasswordResponse> Pagination(int pageSize, int pageNumber)
        {
            // Pega do Banco de Dado 
            var passwordList = _passwordRepository.GetAll().ToList();

            // Pegando os dados da página atual
            var passwordListFiltered = GetPagedData(passwordList, pageSize, pageNumber);

            // Verifica se a lista tem itens
            if (passwordListFiltered.Any())
            {
                // utilizando o método Select do LINQ para projetar cada objeto hero da lista heroList em um novo objeto ApiGetSuperHeroResponse
                var list = passwordListFiltered.Select(password => new ApiGetPasswordResponse
                {
                    // Atribui valor para as propriedades da classe de resposta (famoso Mapper) 
                    Description = password.Description,
                    DecryptedPassword = password.Encrypted_password
                }).ToList();

                // Retornando a lista de resposta
                return list;
            }

            // Caso a lista não tenha itens, retorna uma lista vazia
            return new List<ApiGetPasswordResponse>();

        }
        static List<Password> GetPagedData(List<Password> dataPassword, int pageSize, int pageNumber)
        {
            return dataPassword.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        // Método para criptografar a senha
        private string EncryptPassword(string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            string encryptedPassword = Convert.ToBase64String(bytes);
            return encryptedPassword;
        }

        // Método para descriptografar a senha
        public string DecryptPassword(string encryptedPassword)
        {

            byte[] bytes = Convert.FromBase64String(encryptedPassword);
            string password = Encoding.UTF8.GetString(bytes);
            return password;

        }
        
    }

}


    
    
