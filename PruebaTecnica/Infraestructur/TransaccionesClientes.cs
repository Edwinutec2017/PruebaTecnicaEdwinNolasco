using AutoMapper;
using Domain.Dto;
using Infraestructur.Interface;
using Microsoft.Data.SqlClient;
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


        public TransaccionesClientes(SqlConnection sqlConnection, IMapper mapper ) 
        {
        _connection = sqlConnection;
           _mapper = mapper;
        }

        public async Task<List<TitularTargeta>> GetClientes()
        {
            var clientes = new List<TitularTargeta>();
            try
            {
                  _connection.Open();
                string query = "SELECT * FROM targeta_titular";
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

            }
            finally 
            {
             _connection.Close();
            }
            return await Task.FromResult(clientes);

        }
    }
}
