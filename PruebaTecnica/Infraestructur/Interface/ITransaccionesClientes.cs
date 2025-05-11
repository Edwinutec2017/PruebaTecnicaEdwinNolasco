using Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructur.Interface
{
    public interface ITransaccionesClientes
    {
        //Lista de clientes 
        Task<List<TitularTargeta>> GetClientes();
        //Registro de compras 
        Task<string> AddCompras(Compras compras);
        //Registro de Pagos
        Task<string> AddPagos(Pagos pagos);
    }
}
