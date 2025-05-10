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
        Task<List<TitularTargeta>> GetClientes();
    }
}
