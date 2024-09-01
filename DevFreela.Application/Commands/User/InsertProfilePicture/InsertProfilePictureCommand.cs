using DevFreela.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DevFreela.Application.Commands.User.InsertProfilePicture;

public record InsertProfilePictureCommand(int IdUser, IFormFile File) : IRequest<ResultViewModel>;