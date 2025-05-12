using Dtos.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesClass.Interfaces
{
    public interface ITransaccionesClientes
    {
        Task<bool> AddCompras(TransaccionesDto transaccionesDto);
        Task<bool> AddPagos(TransaccionesDto transaccionesDto);
        Task<ClienteTransacciones> GetTransacciones(ClienteInput cliente);
        Task<List<TitularTargetaDto>> GetClientes();
    }
}
