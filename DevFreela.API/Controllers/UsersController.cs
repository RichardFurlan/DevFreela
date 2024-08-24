using DevFreela.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    public IActionResult Post(CreateUserDTO model)
    {
        return NoContent();
    }

    [HttpPost("{id}/skill")]
    public IActionResult PostSkill(UserSkillDTO model)
    {
        return NoContent();
    }
    
    

    [HttpPost("{id}/profile-picture")]
    public IActionResult PostProfilePicture(IFormFile file)
    {
        var description = $"File: {file.FileName}, Size: {file.Length}";
        
        // Processar imagem

        return Ok(description);
    }
}