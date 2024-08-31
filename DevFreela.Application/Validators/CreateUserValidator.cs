using DevFreela.API.DTOs;
using FluentValidation;

namespace DevFreela.Application.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserDTO>
{
    public CreateUserValidator()
    {
        RuleFor(u => u.Email)
            .EmailAddress()
                .WithMessage("E-mail inválido");

        RuleFor(u => u.BirthDate)
            .Must(d => d < DateTime.Now.AddYears(-18))
            .WithMessage("Deve ser maior de idade");
    }
}