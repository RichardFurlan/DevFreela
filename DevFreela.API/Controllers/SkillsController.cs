using DevFreela.API.DTOs;
using DevFreela.Domain.Entities;
using DevFreela.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SkillsController : ControllerBase
{
    private readonly DevFreelaDbContext _context;
    public SkillsController(DevFreelaDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var skills = _context.Skills.ToList();
        
        //Criar Model para retorno
        return Ok(skills);
    }

    [HttpPost]
    public IActionResult Post(CreateSkillDTO model)
    {
        var skill = new Skill(model.Description);

        _context.Skills.Add(skill);
        _context.SaveChanges();
        
        return NoContent();
    }
}