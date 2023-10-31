using FluentValidation;
using RappidPayTest.Application.DTOs;
using System;

namespace RappidPayTest.Application.Validations
{
    public class CardManagementDtoValidation : AbstractValidator<CardManagementCreateDto>
    {
        public CardManagementDtoValidation()
        {
            RuleFor(x => x.CardNumber)
                .Must(cardNumber => cardNumber.ToString().Length == 15).WithMessage("Card Number must be equal to 15 characters")
                .Matches("^[0-9]+$").WithMessage("CardNumber debe contener solo números")
                .NotNull().WithMessage("Card Number is Required");
            RuleFor(x => x.IDUser).NotEmpty().WithMessage("IDUser is Required");
            RuleFor(x => x.Balance).NotEmpty().WithMessage("Balance is Required");
        }
    }
}