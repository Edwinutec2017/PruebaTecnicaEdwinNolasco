﻿using Dtos.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesClass.Interfaces
{
    public interface IGenrerarPdf
    {
        byte[] GenerarEstadoDeCuenta(ClienteTransacciones clienteTransacciones);

    }
}
