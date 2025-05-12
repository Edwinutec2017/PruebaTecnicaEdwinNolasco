using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Dtos
{
    public class TitularTargetaDto
    {
        public int CodCliente { get; set; }
        public required string NombreTitular { get; set; }
        public required string NumeroTargeta { get; set; }
        public decimal LimiteCredito { get; set; }
        public decimal SaldoActual { get; set; }
        public decimal SaldoDisponible { get; set; }
    }
}
