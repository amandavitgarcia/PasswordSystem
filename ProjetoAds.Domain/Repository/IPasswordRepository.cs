using ProjetoAds.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAds.Domain.Repository
{
    public interface IPasswordRepository
    {
        // Método que recupera um unico objeto do repositório(banco de dados) com base no ID
        Password GetById(int id);

        // Método que retorna todos os objetos existentes no repositório
        IEnumerable<Password> GetAll();

        // Método responsável por inserir um novo objeto Championship no repositório
        // O objeto championship no parametro contém os dados que serão inseridos 
        void Insert(Password password);

        // Método que atualiza um objeto existente no repositório
        // O objeto championship no parametro contém os dados atualizados
        void Update(Password password);

        // Método responsável por excluir um objeto do repositório. 
        // A exclusão é feita baseando-se no id informado no parametro do método
        void Delete(int id);

        // Método responsável por filtrar nome dos heróis 
        List<Password> SearchBySql(string filter);

        // Método de paginação com SQL
        IEnumerable<Password> PaginationSql(int pageSize, int pageNumber);

        Password GetByDescription(string description);
    }
}
