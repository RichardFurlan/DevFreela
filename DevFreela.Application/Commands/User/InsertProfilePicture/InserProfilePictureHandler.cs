using DevFreela.Application.DTOs;
using DevFreela.Domain.Respositories;
using MediatR;

namespace DevFreela.Application.Commands.User.InsertProfilePicture;

public class InserProfilePictureHandler : IRequestHandler<InsertProfilePictureCommand, ResultViewModel>
{
    private readonly IUserRepository _userRepository;
    public InserProfilePictureHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<ResultViewModel> Handle(InsertProfilePictureCommand request, CancellationToken cancellationToken)
    {
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profile-pictures");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        
        var filePath = Path.Combine("www.fakepathimage.com/", request.File.FileName);
        
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream);
        }
        
        var user = await _userRepository.GetByIdAsync(request.IdUser);
        if (user is null)
        {
            return ResultViewModel.Error("Usuário não encontrado");
        }
        
        user.UpdateProfilePicture(filePath);
        await _userRepository.UpdateAsync(user);
        
        return ResultViewModel.Success();
    }
}