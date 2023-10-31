using FluentValidation;
using RappidPayTest.Application.DTOs;
using System;

namespace RappidPayTest.Application.Validations
{
    public class PrioridadesDtoValidation : AbstractValidator<PrioridadesDto>
    {
        public PrioridadesDtoValidation()
        {
            RuleFor(x => x.Prioridad).NotNull().WithMessage("Prioridad is Required");
            RuleFor(x => x.Prioridad).NotEmpty().WithMessage("Prioridad is Required");
            RuleFor(x => x.Prioridad).Length(0, 150).WithMessage("Prioridad lenght 0 to 150 caracters");
        }
    }
}