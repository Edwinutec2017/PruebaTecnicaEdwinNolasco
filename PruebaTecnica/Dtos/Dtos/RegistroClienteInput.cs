using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Dtos
{
    public class RegistroClienteInput
    {

        [MaxLength(100),Required]
      
        public string NombreTitular { get; set; }

        [MaxLength(16),Required]
        public string NumeroTargeta { get; set; }

        [Required]
        public double LimiteCredito { get; set; }


    }
}
