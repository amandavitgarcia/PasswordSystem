using Dapper;
using ProjetoAds.Domain.Entities;
using ProjetoAds.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAds.Data.Repository
{
    // Classe onde as consultas SQL são realizadas
    public class PasswordRepository : IPasswordRepository
    {

        // Query que cria e abre uma conexão com o banco de dados e executa uma consulta
        public IEnumerable<Password> GetAll()
        {

            using (var connection = new SqlConnection("Server=localhost\\MSSQLSERVER02;Database=master;Trusted_Connection=True;"))
            {
                connection.Open();

                string query = @"SELECT * FROM [dbo].[PasswordSystem]";

                return connection.Query<Password>(query).ToList();
            }

        }

        // Query que consulta por ID
        public Password GetById(int id)
        {
            using (var connection = new SqlConnection("Server=localhost\\MSSQLSERVER02;Database=master;Trusted_Connection=True;"))
            {
                connection.Open();

                string query = @"SELECT * FROM [dbo].[PasswordSystem] WHERE [id] = @Id";

                return connection.Query<Password>(query, new { Id = id }).FirstOrDefault();
            }
        }

        public Password GetByDescription(string description)
        {
            using (var connection = new SqlConnection("Server=localhost\\MSSQLSERVER02;Database=master;Trusted_Connection=True;"))
            {
                connection.Open();

                string query = @"SELECT * FROM [dbo].[PasswordSystem] WHERE [description] = @Description";

                return connection.Query<Password>(query, new { Description = description }).FirstOrDefault();
            }
        }

        // Query DELETE
        public void Delete(int id)
        {
            using (var connection = new SqlConnection("Server=localhost\\MSSQLSERVER02;Database=master;Trusted_Connection=True;"))
            {
                connection.Open();

                string query = @"DELETE FROM [dbo].[PasswordSystem] WHERE [id] = @Id";

                connection.Execute(query, new
                {
                    Id = id
                });
            }
        }

        // Query de inserção (INSERT)
        public void Insert(Password password)
        {
            using (var connection = new SqlConnection("Server=localhost\\MSSQLSERVER02;Database=master;Trusted_Connection=True;"))
            {
                connection.Open();

                var query = $"INSERT INTO [dbo].[PasswordSystem] (description, encrypted_password, date_creation, update_date, active_password)" +
                            "VALUES" +
                            "(@description, @encrypted_password, GETDATE(), GETDATE(), @active_password)";

                connection.Execute(query, new
                {
                    description = password.Description,
                    encrypted_password = password.Encrypted_password,
                    active_password = password.Active_Password
                });
            }
        }

        // Query de atualização (UPDATE)
        public void Update(Password password)
        {
            using (var connection = new SqlConnection("Server=localhost\\MSSQLSERVER02;Database=master;Trusted_Connection=True;"))
            {
                connection.Open();

                var query = $"UPDATE [dbo].[PasswordSystem] " +
                            "SET description = @Description, encrypted_password = @EncryptedPassword" +
                            " WHERE [id] = @Id";

                connection.Execute(query, new
                {
                    Id = password.Id,
                    Description = password.Description,
                    EncryptedPassword = password.Encrypted_password
                });
            }
        }

        // Query que busca a partir do SQL
        public List<Password> SearchBySql(string filter)
        {
            using (var connection = new SqlConnection("Server=localhost\\MSSQLSERVER02;Database=master;Trusted_Connection=True;"))
            {
                connection.Open();

                string query = @"SELECT * FROM [dbo].[PasswordSystem] WHERE [encrypted_password] LIKE '%' + @EncryptedPassword + '%'";

                return connection.Query<Password>(query, new { HeroName = filter }).ToList();

            }
        }

        // Query que cria uma paginação com SQL 
        public IEnumerable<Password> PaginationSql(int pageSize, int pageNumber)
        {
            using (var connection = new SqlConnection("Server=localhost\\MSSQLSERVER02;Database=master;Trusted_Connection=True;"))
            {
                connection.Open();

                // OFFSET FETCH para paginar 
                string query = @"SELECT * 
                        FROM [dbo].[PasswordSystem] 
                        ORDER BY ID
                        OFFSET @PageSize * (@PageNumber - 1) ROWS
                        FETCH NEXT @PageSize ROWS ONLY;";

                return connection.Query<Password>(query, new { PageSize = pageSize, PageNumber = pageNumber }).ToList();
            }
        }


    }
}
