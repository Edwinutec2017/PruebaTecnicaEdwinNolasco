using Dtos.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Validador
{
    public class ClienteInputValidador: AbstractValidator<ClienteInput>
    {
        public ClienteInputValidador() 
        {
            RuleFor(p => p.CodCliente)
             .GreaterThan(0).WithMessage("El codigo de clientes no puede ser cero")
             .LessThan(30).WithMessage("Limite de codigo es 30");

        }

    }
}
