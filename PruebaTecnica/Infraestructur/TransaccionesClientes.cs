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

        public async Task<string> AddCompras(Compras compras)
        {
            string response = "";
            try
            {
                _connection.Open();
                using (var transaccion= _connection.BeginTransaction()) 
                {

                    try
                    {

                        var command = new SqlCommand("INSERT INTO compras VALUES(@CodCliente,@Description,@Monto,@Tipo,@FechaCompra)", _connection,transaccion);
                        command.Parameters.AddWithValue("@CodCliente", compras.CodCliente);
                        command.Parameters.AddWithValue("@Description", compras.Description);
                        command.Parameters.AddWithValue("@Monto", compras.Monto);
                        command.Parameters.AddWithValue("@Tipo", compras.Tipo);
                        command.Parameters.AddWithValue("@FechaCompra", compras.FechaCompra);

                        var compraId = Convert.ToInt32(await command.ExecuteScalarAsync());

                      _= transaccion.CommitAsync();
                        response = compraId.Equals(0) ? "Compra Registrada" : "Error en el registro de la comptra ";

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error en la transacion {ex.Message}");
                    transaccion.Rollback();
                    
                    }
                
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error al crear el registro de la compra {ex.Message}");
            }
            finally 
            {
            _connection.Close();
            }

           return response;
        }

        public Task<string> AddPagos(Pagos pagos)
        {
            throw new NotImplementedException();
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
                _logger.LogError($"Ocurrio un error en la consulta de clientes {ex.Message}");
            }
            finally 
            {
             _connection.Close();
            }
            return await Task.FromResult(clientes);

        }
    }
}
