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

        public async Task<string> AddCompras(Compras compras)
        {
           return await _transaccionesClientes.AddCompras(compras);
        }

        public Task<string> AddPagos(Pagos pagos)
        {
            throw new NotImplementedException();
        }

        public async  Task<List<TitularTargeta>> GetClientes()
        {
         
           return  await _transaccionesClientes.GetClientes();
        }
    }
}
