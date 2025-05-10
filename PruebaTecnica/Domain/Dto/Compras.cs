using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class Compras
    {
        public int CodCliente { get; set; }
        public string Description { get; set; }
        public decimal Monto { get; set; }
        public string Tipo { get;set; }
        public  DateTime FechaCompra { get; set; }
    }
}
