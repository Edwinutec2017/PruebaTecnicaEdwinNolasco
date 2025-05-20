using Domain.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.FlueValidation
{
    public class ClienteValidador: AbstractValidator<ClienteInput>
    {

        public ClienteValidador() 
        {
            RuleFor(p => p.CodCliente)
                .GreaterThan(0).WithMessage("El codigo de clientes no puede ser cero")
                .LessThan(30).WithMessage("Limite de codigo es 30");
        }
    }
}
