using DevFreela.API.DTOs;
using DevFreela.Application.Commands.User.InsertProfilePicture;
using DevFreela.Application.Commands.User.InsertUser;
using DevFreela.Application.Commands.User.InsertUserSkill;
using DevFreela.Application.Commands.User.LoginUser;
using DevFreela.Application.Queries.User.GetUserById;
using DevFreela.Domain.Entities;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return Ok(result);
    }
    
    
    [HttpPost("register")]
    public async Task<IActionResult> Post(InsertUserCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return CreatedAtAction(nameof(GetById), new { id = result.Data }, command);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Post(LoginUserCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return Ok(result);
    }

    [HttpPost("{id}/skill")]
    public async Task<IActionResult> PostSkill(int id, [FromBody] InsertUserSkillCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return NoContent();
    }

    [HttpPost("{id}/profile-picture")]
    public async Task<IActionResult> PostProfilePicture(int id, IFormFile file, [FromServices] InsertProfilePictureCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return NoContent();
    }
}