﻿using Dtos.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesClass.Interfaces
{
    public interface ITransaccionesClientes
    {
        Task<string> AddCompras(TransaccionesDto transaccionesDto);
        Task<string> AddPagos(TransaccionesDto transaccionesDto);
        Task<ClienteTransacciones> GetTransacciones(ClienteInput cliente);
        Task<List<TitularTargetaDto>> GetClientes();
        Task<List<TransaccionesDto>> Transacciones(ClienteInput cliente);
        Task<byte[]> GenerarExcelCompras(ClienteInput cliente);
        Task<byte[]> GenerarEstadoDecuentas(ClienteInput cliente);
    }
}
