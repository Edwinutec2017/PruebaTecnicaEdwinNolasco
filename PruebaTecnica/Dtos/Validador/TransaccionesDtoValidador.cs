using Dtos.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Validador
{
    public class TransaccionesDtoValidadorCompras: AbstractValidator<TransaccionesDto>
    {
        public TransaccionesDtoValidadorCompras() 
        {

            RuleFor(p => p.CodCliente)
          .GreaterThan(0).WithMessage("El codigo de clientes no puede ser cero")
          .LessThan(30).WithMessage("Limite de codigo es 30");

            RuleFor(p => p.Description)
            .NotEmpty().WithMessage("La descipcion es requerdida ")
            .MaximumLength(100).WithMessage("Limite de caracteres es 100")
            .Must(ValidarDescription).WithMessage("Descripcion no valida")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$").WithMessage("Solo texto no caracteres especiales en la descripcion ")
             .When(z => !string.IsNullOrEmpty(z.Description));

            RuleFor(p => p.Monto)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a cero");


            RuleFor(p => p.FechaTransaccion)
                .LessThanOrEqualTo(DateTime.Now)
               .WithMessage($"La fecha no puede ser futura");

        }

        private bool ValidarDescription(string description)
        {
            var tiposValidos = new[] { "string", "String" };
            return !tiposValidos.Contains(description);
        }

    }



}
