using Domain.Dto;
using Infraestructur.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructur
{
    public class TransaccionesClientes : ITransaccionesClientes
    {
        private readonly SqlConnection _connection;


        public TransaccionesClientes(SqlConnection sqlConnection) 
        {
        _connection = sqlConnection;
        }

        public async Task<List<TitularTargeta>> GetClientes()
        {
            try
            {
                await _connection.OpenAsync();
                string query = "SELECT * FROM targeta_titular";
                using (var commad= new SqlCommand(query,_connection)) 
                {
                    using (var reader= commad.ExecuteReader()) 
                    {

                        var  clientes = new List<TitularTargeta>();
                        while (reader.Read()) 
                        {
                            var resulta = await commad.ExecuteScalarAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally 
            {
            await _connection.CloseAsync();
            }
            return new List<TitularTargeta>();

        }
    }
}
