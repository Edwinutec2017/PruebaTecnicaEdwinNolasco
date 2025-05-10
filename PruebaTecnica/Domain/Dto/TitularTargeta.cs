using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class TitularTargeta
    {
        public int CodCliente { get; set; }
        public string NombreTitular { get;set; }
        public string NumeroTargte { get; set; }
        public decimal LimiteCredito { get; set; }
        public decimal SaldoActual { get; set; }
        public decimal SaldoDisponible { get;set;}
    }
}
