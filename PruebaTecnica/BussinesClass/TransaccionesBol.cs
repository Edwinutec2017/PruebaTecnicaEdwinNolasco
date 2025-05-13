using BussinesClass.Interfaces;
using Dtos.Dtos;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesClass
{
    public class TransaccionesBol : ITransaccionesClientes
    {
        private readonly ITransaccionesService _transaccionesService;
        private readonly ParametrosTasas _parametrosTasas;


        public TransaccionesBol(ITransaccionesService transaccionesService, ParametrosTasas parametrosTasas) 
        {
        _transaccionesService = transaccionesService;
         _parametrosTasas = parametrosTasas;
        }   

        public async Task<string> AddCompras(TransaccionesDto transaccionesDto)
        {
            var resp = "";

            try
            {
                if (transaccionesDto != null) 
                {
                    transaccionesDto.Tipo = "Compra";
                     resp = await _transaccionesService.AddComprasCliente(transaccionesDto);

                }

            }
            catch (Exception ex) 
            {

            }

            return resp;
        }

        public Task<bool> AddPagos(TransaccionesDto transaccionesDto)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TitularTargetaDto>> GetClientes()
        {
            return await _transaccionesService.GetClientes();
        }

        public async Task<ClienteTransacciones> GetTransacciones(ClienteInput cliente)
        {
            ClienteTransacciones clienteTransacciones= new ClienteTransacciones();
            try
            {

                clienteTransacciones.Clientes = await _transaccionesService.GetCliente(cliente);
                clienteTransacciones.Transacciones = await _transaccionesService.GetTransacciones(cliente);

                clienteTransacciones = CalculodeSaldos(clienteTransacciones);

            }
            catch (Exception ex) 
            {
            
            }
            return clienteTransacciones;
        }

        public async Task<List<TransaccionesDto>> Transacciones(ClienteInput cliente)
        {
            return await _transaccionesService.GetTransacciones(cliente);
        }

        private ClienteTransacciones CalculodeSaldos(ClienteTransacciones clienteTransacciones) 
        {
         
            var  mesActuual = DateTime.Now.Month;
            var mesAnterior = DateTime.Now.AddMonths(-1).Month;

            if (clienteTransacciones!=null) 
            {

                clienteTransacciones.Porcentaje = _parametrosTasas.PorcentageConfigurable;
                clienteTransacciones.Interes=_parametrosTasas.InteresCofigurable;

                clienteTransacciones.TotalComprasMesActual = Convert.ToDouble(clienteTransacciones.Transacciones.Where(e => e.Tipo.Equals("Compra") && e.FechaTransaccion.Month.Equals(mesActuual)).Sum(e => e.Monto));
                clienteTransacciones.TotalComprasMesAnterior = Convert.ToDouble(clienteTransacciones.Transacciones.Where(e => e.Tipo.Equals("Compra") && e.FechaTransaccion.Month.Equals(mesAnterior)).Sum(e => e.Monto));
                clienteTransacciones.Transacciones=clienteTransacciones.Transacciones.Where(e=>e.Tipo.Equals("Compra") && e.FechaTransaccion.Month.Equals(mesActuual) ).Select(e=>e).ToList();


                if (clienteTransacciones.Clientes.SaldoActual>0) 
                {
                    clienteTransacciones.InteresBonificable = (double)clienteTransacciones.Clientes.SaldoActual * (clienteTransacciones.Interes/100);
                }
            }
            return clienteTransacciones;
        }



    }
}
