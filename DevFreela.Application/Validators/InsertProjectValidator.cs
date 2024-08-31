using DevFreela.Application.Commands.Project.InsertProject;
using FluentValidation;

namespace DevFreela.Application.Validators;

public class InsertProjectValidator : AbstractValidator<InsertProjectCommand>
{
    public InsertProjectValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty()
                .WithMessage("Não pode ser vazio")
            .MaximumLength(80)
                .WithMessage("Tamanho máximop é 80 caracteres");

        RuleFor(p => p.TotalCost)
            .GreaterThanOrEqualTo(1000)
                .WithMessage("O projeto deve custar pelo menos R$ 1000");


    }
}