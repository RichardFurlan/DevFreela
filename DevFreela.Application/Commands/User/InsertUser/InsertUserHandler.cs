using DevFreela.Application.DTOs;
using DevFreela.Domain.Respositories;
using DevFreela.Infrastructure.Services.AuthService;
using MediatR;

namespace DevFreela.Application.Commands.User.InsertUser;

public class InsertUserHandler : IRequestHandler<InsertUserCommand, ResultViewModel<int>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    public InsertUserHandler(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<ResultViewModel<int>> Handle(InsertUserCommand request, CancellationToken cancellationToken)
    {
        var validationPassword = _authService.ValidarSenha(request.Password, request.PasswordConfirm);
        if (!string.IsNullOrWhiteSpace(validationPassword))
        {
            return ResultViewModel<int>.Error(validationPassword);
        }
        
        var user = request.ToEntity(_authService);
        
        await _userRepository.AddAsync(user);
        
        return ResultViewModel<int>.Success(user.Id);
    }
}