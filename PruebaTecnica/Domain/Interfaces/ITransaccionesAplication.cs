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

        Task<string> AddCompras(Transacciones compras);

        Task<string> AddPagos(Transacciones pagos);

        Task<List<Transacciones>> GetTransacciones(int codCliente);

        Task<TitularTargeta> GetClienteCod(int cod);



    }
}
