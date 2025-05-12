using AutoMapper;
using Azure;
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
using System.Transactions;

namespace Infraestructur
{
    public class UnitOfWork<T> : IUnitOfWork<T>
    {
        private readonly SqlConnection _connection;
        private readonly IMapper _mapper;
        private readonly ILogger<T> _logger;

        public UnitOfWork(SqlConnection sqlConnection, IMapper mapper, ILogger<T> logger)
        {
            _connection = sqlConnection;
            _mapper = mapper;
            _logger = logger;
           
        }

        public async Task AddItem(string query, List<ParametrosConsultas> param)
        {
            try
            {
                _connection.Open();
                using (var transaccion = _connection.BeginTransaction()) 
                {
                    try
                    {
                        var command = new SqlCommand(query, _connection, transaccion);
                        if (param != null && param.Count > 0)
                        {
                            foreach (var p in param)
                            {
                                command.Parameters.AddWithValue(p.Tipo, p.Valor);
                            }

                        }
                        await command.ExecuteScalarAsync();

                        transaccion.Commit();

                    }
                    catch (Exception ex) 
                    {
                     _logger.LogError($"Error en la transaccion {ex}");
                    transaccion.Rollback();
                    }
                
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error {ex.Message}");
            }
            finally 
            {
            await _connection.CloseAsync();
            }

        }

        public async Task DeleteItem(string query, List<ParametrosConsultas> param)
        {
            try
            {
                _connection.Open();
                using (var transaccion = _connection.BeginTransaction())
                {
                    try
                    {
                        var command = new SqlCommand(query, _connection, transaccion);
                        if (param != null && param.Count > 0)
                        {
                            foreach (var p in param)
                            {
                                command.Parameters.AddWithValue(p.Tipo, p.Valor);
                            }

                        }
                        await command.ExecuteScalarAsync();

                        transaccion.Commit();

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error en la transaccion {ex}");
                        transaccion.Rollback();
                    }

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error {ex.Message}");
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<List<T>> GetAll(string query, List<ParametrosConsultas> param)
        {
            var reponse = new List<T>();
            try
            {
                _connection.Open();

                using (var command= new SqlCommand(query,_connection)) 
                {

                    if (param != null &&  param.Count>0) 
                    {
                        foreach (var p in param) 
                        {
                            command.Parameters.AddWithValue(p.Tipo,p.Valor);
                        }
                    
                    }

                    using (var reader = command.ExecuteReader()) 
                    {
                        while (reader.Read()) 
                        {
                            reponse.Add(_mapper.Map<IDataReader, T>(reader));
                        }
                    
                    }
                
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error {ex}");
            }
            finally 
            {
            await _connection.CloseAsync();
            
            }

            return reponse;
        }

        public async Task<bool> UpdateItem(string query, List<ParametrosConsultas> param)
        {
            bool resp=false;
            try
            {
                _connection.Open();
                using (var transaccion = _connection.BeginTransaction())
                {
                    try
                    {
                        var command = new SqlCommand(query, _connection, transaccion);
                        if (param != null && param.Count > 0)
                        {
                            foreach (var p in param)
                            {
                                command.Parameters.AddWithValue(p.Tipo, p.Valor);
                            }

                        }
                        int  result=  await command.ExecuteNonQueryAsync();
                        resp = result>0;

                       transaccion.Commit();

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error en la transaccion {ex}");
                        transaccion.Rollback();
                    }

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error {ex.Message}");
            }
            finally
            {
                await _connection.CloseAsync();
            }

            return resp;
        }
    }

}
