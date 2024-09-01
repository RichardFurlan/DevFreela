using DevFreela.Application.DTOs;
using MediatR;

namespace DevFreela.Application.Commands.User.InsertUser;

public record InsertUserCommand(string FullName, string Email, DateTime BirthDate) : IRequest<ResultViewModel<int>>
{
    public Domain.Entities.User ToEntity()
        => new(FullName, Email, BirthDate);
};