using FluentValidation;
using RapidPayTest.Application.DTOs;
using System;

namespace RapidPayTest.Application.Validations
{
    public class CardManagementDtoValidation : AbstractValidator<CardManagementCreateDto>
    {
        public CardManagementDtoValidation()
        {
            RuleFor(x => x.CardNumber)
                .Must(cardNumber => cardNumber.ToString().Length == 15).WithMessage("The number on the card must be 15")
                .Matches("^[0-9]+$").WithMessage("Card Number must contain only numbers")
                .NotNull().WithMessage("Card Number is Required");
            RuleFor(x => x.IDUser).NotEmpty().WithMessage("IDUser is Required");
            RuleFor(x => x.Amount).NotEmpty().WithMessage("Amount is Required");
        }
    }
}