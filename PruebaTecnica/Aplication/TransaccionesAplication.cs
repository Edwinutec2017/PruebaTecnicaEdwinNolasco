using Domain.Dto;
using Domain.Interfaces;
using Infraestructur.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication
{
    public class TransaccionesAplication : ITransaccionesAplication
    {
        private readonly ITransaccionesClientes _transaccionesClientes;

        public TransaccionesAplication(ITransaccionesClientes transaccionesClientes) 
        {
        _transaccionesClientes = transaccionesClientes;
        }

        public async Task<string> AddCompras(Transacciones compras)
        {
            var resp = ActualizarSaldosCompras(compras).Result;
            if (resp.Equals("saldos"))
            {
                bool addCompras= await _transaccionesClientes.AddCompras(compras);
                resp = addCompras ? "registrada" : "Ocurrio un error";

            }
            return resp;
           
        }

        public async Task<bool> AddPagos(Transacciones pagos)
        {
          return await _transaccionesClientes.AddPagos(pagos);
        }

        public async Task<TitularTargeta> GetClienteCod(int cod)
        {
           return await _transaccionesClientes.GetClientesCodCliente(cod);
        }

        public async  Task<List<TitularTargeta>> GetClientes()
        {
         
           return  await _transaccionesClientes.GetClientes();
        }

        public async Task<List<Transacciones>> GetTransacciones(int codCliente)
        {
            return await _transaccionesClientes.GetTransacciones(codCliente);
        }


        private async Task<string> ActualizarSaldosCompras(Transacciones transaccion) 
        {
            var resp="";
            try
            {
                if (transaccion.CodCliente>0 && transaccion.Monto>0) 
                {

                    var cliente =await _transaccionesClientes.GetClientesCodCliente(transaccion.CodCliente);

                    var saldoActual = cliente.SaldoActual + transaccion.Monto;
                    cliente.SaldoActual = saldoActual;

                    var SaldoDisponible = cliente.LimiteCredito - transaccion.Monto;

                    cliente.SaldoDisponible = SaldoDisponible;


                    if (cliente.SaldoActual <= cliente.LimiteCredito)
                    {
                        var updateSaldo = await _transaccionesClientes.ActualizarSaldos(cliente);
                        resp = updateSaldo? "saldos":"Error en la actualizacion";
                    }
                    else if(cliente.SaldoActual == cliente.LimiteCredito)
                    {
                        resp = "Ya llego al limite del credito";
                    }

                }

            }
            catch (Exception ex) 
            {
            
            }
            return resp;
        
        }



    }
}
