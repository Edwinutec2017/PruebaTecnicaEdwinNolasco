using AutoMapper;
using Domain.Dto;
using Infraestructur.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructur
{
    public class TransaccionesClientes : ITransaccionesClientes
    {
        private readonly SqlConnection _connection;
        private readonly IMapper _mapper;
        private readonly ILogger<TransaccionesClientes> _logger;



        public TransaccionesClientes(SqlConnection sqlConnection, IMapper mapper, ILogger<TransaccionesClientes> logger ) 
        {
        _connection = sqlConnection;
           _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TitularTargeta>> GetClientes()
        {
            var clientes = new List<TitularTargeta>();
            try
            {
                 _connection.Open();
                string query = "EXEC  LISTA_CLIENTES";
                using (var commad= new SqlCommand(query,_connection)) 
                {
                    using (var reader= commad.ExecuteReader()) 
                    {
                        while (reader.Read())
                        {
                            clientes.Add( _mapper.Map<IDataReader, TitularTargeta>(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un erro en la consulta de clientes {ex.Message}");
            }
            finally 
            {
             _connection.Close();
            }
            return await Task.FromResult(clientes);

        }
    }
}
