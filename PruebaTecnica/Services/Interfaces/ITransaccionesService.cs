using Dtos.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITransaccionesService
    {
        Task<List<TitularTargetaDto>> GetClientes();
        Task<List<TransaccionesDto>> GetTransacciones(ClienteInput clienteInput);
        Task<TitularTargetaDto> GetCliente(ClienteInput clienteInput);

        Task<string> AddComprasCliente(TransaccionesDto compras);
        Task<string> AddPagosCliente(TransaccionesDto compras);
    }
}
