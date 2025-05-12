using Domain.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructur.Interface
{
    public interface IUnitOfWork<T>
    {

        Task<List<T>> GetAll(string query,List<ParametrosConsultas> param);
        Task DeleteItem(string query, List<ParametrosConsultas> param);
        Task AddItem(string query, List<ParametrosConsultas> param);
        Task<bool> UpdateItem(string query, List<ParametrosConsultas> param);


    }
}
