using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Dtos
{
    public class TransaccionesDto
    {
        public int CodTransaccion { get; set; }

        public int CodCliente { get; set; }

        public required string Description { get; set; }

        public decimal Monto { get; set; }

        public required string Tipo { get; set; }

        public DateTime FechaTransaccion { get; set; }
    }

}
