using DevFreela.Application.DTOs;
using DevFreela.Domain.Respositories;
using DevFreela.Infrastructure.Services.AuthService;
using MediatR;

namespace DevFreela.Application.Commands.User.LoginUser;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, ResultViewModel<LoginUserViewModel>>
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;
    public LoginUserHandler(IAuthService authService, IUserRepository userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }
    public async Task<ResultViewModel<LoginUserViewModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var passwordHash = _authService.ComputeSha256Hash(request.Password);

        var user = await _userRepository.GetUserByEmailAndPasswordAsync(request.Email, passwordHash);
        
        if (user is null)
        {
            return ResultViewModel<LoginUserViewModel>.Error("E-mail ou senha incorretos, tente novamente.");
        }
        
        var token = _authService.GenerateJwtToken(user.Email);

        return ResultViewModel<LoginUserViewModel>.Success(new LoginUserViewModel(user.Email, token));
    }
}