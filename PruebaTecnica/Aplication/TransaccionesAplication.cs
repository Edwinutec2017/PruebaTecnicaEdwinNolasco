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

        public async Task<bool> AddCompras(Transacciones compras)
        {
           return await _transaccionesClientes.AddCompras(compras);
        }

        public async Task<bool> AddPagos(Transacciones pagos)
        {
          return await _transaccionesClientes.AddPagos(pagos);
        }

        public async  Task<List<TitularTargeta>> GetClientes()
        {
         
           return  await _transaccionesClientes.GetClientes();
        }

        public async Task<List<Transacciones>> GetTransacciones(int codCliente)
        {
            return await _transaccionesClientes.GetTransacciones(codCliente);
        }
    }
}
