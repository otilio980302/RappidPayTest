using FluentValidation;
using RappidPayTest.Application.DTOs;
using System;

namespace RappidPayTest.Application.Validations
{
    public class CardManagementDtoValidation : AbstractValidator<CardManagementDto>
    {
        public CardManagementDtoValidation()
        {
            RuleFor(x => x.CardNumber).GreaterThan(15).LessThanOrEqualTo(15).NotNull().WithMessage("CardNumber is Required");
            RuleFor(x => x.IDUser).NotEmpty().WithMessage("IDUser is Required");
            RuleFor(x => x.Balance).NotEmpty().WithMessage("Balance is Required");
        }
    }
}