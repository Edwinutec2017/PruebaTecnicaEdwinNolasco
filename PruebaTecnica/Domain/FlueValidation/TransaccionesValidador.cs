using Domain.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.FlueValidation
{
    public class TransaccionesValidador : AbstractValidator<Transacciones>
    {

        public TransaccionesValidador()
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


            RuleFor(p => p.Tipo)
             .NotEmpty().WithMessage("El tipo es requerido")
            .MaximumLength(6).WithMessage("Limite de caracteres en 6")
            .Must(ContenerSoloLetrasSinEspacio).WithMessage("El tipo  solo es texto")
            .Must(TipoValido).WithMessage("El tipo debe ser Compra o Pagos")
            .When(x => !string.IsNullOrEmpty(x.Tipo));

            RuleFor(p => p.FechaTransaccion)
                .LessThanOrEqualTo(DateTime.Now)
               .WithMessage($"La fecha no puede ser futura");



        }

        private bool ContenerSoloLetras(string nombre)
        {
            return !string.IsNullOrEmpty(nombre) && nombre.All(char.IsLetter);
        }

        private bool ContenerSoloLetrasSinEspacio(string nombre)
        {
            return !string.IsNullOrWhiteSpace(nombre) && nombre.All(char.IsLetter);
        }

        private bool TipoValido(string tipo)
        {
            var tiposValidos = new[] { "Compra", "Pagos" };
            return tiposValidos.Contains(tipo);
        }

        private bool ValidarDescription(string description) 
        {
            var tiposValidos = new[] { "string", "String" };
            return !tiposValidos.Contains(description);
        }
    }
}
