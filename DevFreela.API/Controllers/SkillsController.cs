using DevFreela.API.DTOs;
using DevFreela.Application.Commands.Skill.InsertSkill;
using DevFreela.Application.Queries.Skill.GetAllSkills;
using DevFreela.Domain.Entities;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SkillsController : ControllerBase
{
    private readonly IMediator _mediator;
    public SkillsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(string search = "", int page = 0, int size = 10)
    {
        var query = new GetAllSkillsQuery(search, page, size);

        var result = await _mediator.Send(query);
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(InsertSkillCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return NoContent();
    }
}