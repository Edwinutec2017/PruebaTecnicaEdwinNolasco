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
        Task<string> AddCompras(Transacciones compras);
        //Registro de Pagos
        Task<string> AddPagos(Transacciones pagos);
        //Transacciones 
        Task<List<Transacciones>> GetTransacciones(int codCliente );
        //Actualizacion de saldo
        Task<bool> ActualizarSaldos(TitularTargeta cliente);
        //Lista de clientes Id 
        Task<TitularTargeta> GetClientesCodCliente(int codCliente);

    }
}
