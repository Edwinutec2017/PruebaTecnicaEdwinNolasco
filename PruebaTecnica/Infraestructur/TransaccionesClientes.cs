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

namespace Infraestructur
{
    public class TransaccionesClientes : ITransaccionesClientes
    {
        private readonly ILogger<TransaccionesClientes> _logger;
        private readonly IUnitOfWork<TitularTargeta> _unitOfWorkClientes;
        private readonly IUnitOfWork<Transacciones> _unitOfWorkClientesTransacciones;


        public TransaccionesClientes(IMapper mapper, ILogger<TransaccionesClientes> logger, IUnitOfWork<TitularTargeta> unitOfWorkClientes,IUnitOfWork<Transacciones> unitOfWorkTransacciones) 
        {
             _logger = logger;
            _unitOfWorkClientes = unitOfWorkClientes;
            _unitOfWorkClientesTransacciones = unitOfWorkTransacciones;
        }

        public async Task<bool> ActualizarSaldos(TitularTargeta cliente)
        {
            bool resp=false;
            try
            {
                string query = "EXEC actualizar_saldos @saldo_actual, @saldo_disponible,@CodCliente";
                var param = new List<ParametrosConsultas>()
                {
                  new(){Tipo="@CodCliente", Valor=cliente.CodCliente},
                  new(){Tipo="@saldo_actual", Valor=cliente.SaldoActual},
                  new(){Tipo="@saldo_disponible", Valor=cliente.SaldoDisponible},
                };

               resp = await _unitOfWorkClientes.UpdateItem(query, param);
               
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Ocurrio un error en la actualizacion de saldos {ex.Message}");
            }
            return resp;
        }

        public async Task<bool> AddCompras(Transacciones compras)
        {
            bool response =false;

            try
            {
                var query = "INSERT INTO transacciones VALUES(@CodCliente,@Description,@Monto,@Tipo,@FechaCompra)";
                var param = new List<ParametrosConsultas>()
                {
                new(){Tipo="@CodCliente", Valor=compras.CodCliente},
                new(){Tipo="@Description", Valor=compras.Description},
                new(){Tipo="@Monto", Valor=compras.Monto},
                new(){Tipo="@Tipo", Valor=compras.Tipo},
                new(){Tipo="@FechaCompra", Valor=compras.FechaTransaccion},
                };

                await _unitOfWorkClientesTransacciones.AddItem(query, param);

                response = true;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Ocurrio un error -AddCompras {ex.Message}");
            }

           return response;
        }


        public async Task<bool> AddPagos(Transacciones pagos)
        {
            bool response = false;

            try
            {
                var query = "INSERT INTO transacciones VALUES(@CodCliente,@Description,@Monto,@Tipo,@FechaCompra)";
                var param = new List<ParametrosConsultas>()
                {
                new(){Tipo="@CodCliente", Valor=pagos.CodCliente},
                new(){Tipo="@Description", Valor=pagos.Description},
                new(){Tipo="@Monto", Valor=pagos.Monto},
                new(){Tipo="@Tipo", Valor=pagos.Tipo},
                new(){Tipo="@FechaCompra", Valor=pagos.FechaTransaccion},
                };

                await _unitOfWorkClientesTransacciones.AddItem(query, param);

                response = true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error -AddPagos {ex.Message}");
            }


            return response;
        }


        public async Task<List<TitularTargeta>> GetClientes()
        {

            var clientes = new List<TitularTargeta>();
            try
            {
                string query = "EXEC  LISTA_CLIENTES";

                 clientes = await _unitOfWorkClientes.GetAll(query,new List<ParametrosConsultas>());

            }
            catch (Exception ex) 
            {
                _logger.LogError($"Ocurrio un error-GetClientes {ex.Message}");
            }
            return clientes;
        }

        public async Task<TitularTargeta> GetClientesCodCliente(int codCliente)
        {
            var clientes = new List<TitularTargeta>();
            try
            {
                string query = "EXEC CLIENTE @codCliente";
                var param = new List<ParametrosConsultas>()
                {
                new(){Tipo="@codCliente", Valor=codCliente,}
                };

                clientes = await _unitOfWorkClientes.GetAll(query, param);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error-GetClientes {ex.Message}");
            }
            return clientes.First();
        }

        public async Task<List<Transacciones>> GetTransacciones(int codCliente)
        {
            var transacciones = new List<Transacciones>();

            try
            {
                string query = $"EXEC  ESTADO_CUENTAS @CodCliente";
                var param = new List<ParametrosConsultas>() 
                {
                new(){Tipo="@CodCliente", Valor=codCliente,}
                };

                transacciones= await _unitOfWorkClientesTransacciones.GetAll(query,param);

            }
            catch (Exception ex) 
            {
                _logger.LogError($"Ocurrio un error-GetTransacciones {ex}");
            }

            return transacciones;
        }
    }
}
