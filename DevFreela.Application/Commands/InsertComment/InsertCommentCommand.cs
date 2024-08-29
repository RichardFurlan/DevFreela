using DevFreela.Application.DTOs;
using MediatR;

namespace DevFreela.Application.Commands.InsertComment;

public record InsertCommentCommand(string Content, int IdProject, int IdUser) : IRequest<ResultViewModel>;