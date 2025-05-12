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

        public TransaccionesBol(ITransaccionesService transaccionesService) 
        {
        _transaccionesService = transaccionesService;
        }   

        public Task<bool> AddCompras(TransaccionesDto transaccionesDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddPagos(TransaccionesDto transaccionesDto)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TitularTargetaDto>> GetClientes()
        {
            return await _transaccionesService.GetClientes();
        }

        public Task<List<TransaccionesDto>> GetTransacciones(ClienteInput cliente)
        {
            throw new NotImplementedException();
        }
    }
}
