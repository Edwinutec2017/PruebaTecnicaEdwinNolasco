using Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITransaccionesAplication
    {
        Task<List<TitularTargeta>> GetClientes();

        Task<bool> AddCompras(Transacciones compras);

        Task<bool> AddPagos(Transacciones pagos);

        Task<List<Transacciones>> GetTransacciones(int codCliente);



    }
}
