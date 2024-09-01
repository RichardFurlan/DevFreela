using DevFreela.API.DTOs;
using DevFreela.Application.DTOs;
using MediatR;

namespace DevFreela.Application.Queries.User.GetUserById;

public record GetUserByIdQuery(int Id) : IRequest<ResultViewModel<UserViewModel>>;