using DevFreela.Application.DTOs;
using DevFreela.Infrastructure.Services.AuthService;
using MediatR;

namespace DevFreela.Application.Commands.User.InsertUser;

public record InsertUserCommand(string FullName, string Email, string Password, string PasswordConfirm, DateTime BirthDate) : IRequest<ResultViewModel<int>>
{
    public Domain.Entities.User ToEntity(IAuthService authService)
    {
        var hashedPassword = authService.ComputeSha256Hash(Password);
        return new Domain.Entities.User(FullName, Email, hashedPassword, BirthDate);
    }
};