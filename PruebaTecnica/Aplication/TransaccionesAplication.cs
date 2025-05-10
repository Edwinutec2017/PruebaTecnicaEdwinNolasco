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

        public async  Task<List<TitularTargeta>> GetClientes()
        { 
           return  await _transaccionesClientes.GetClientes();
        }
    }
}
