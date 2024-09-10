using DevFreela.Application.Commands.Project.CompleteProject;
using DevFreela.Application.Commands.Project.DeleteProject;
using DevFreela.Application.Commands.Project.InsertComment;
using DevFreela.Application.Commands.Project.InsertProject;
using DevFreela.Application.Commands.Project.StartProject;
using DevFreela.Application.Commands.Project.UpdateProject;
using DevFreela.Application.Queries.Project.GetAllProjects;
using DevFreela.Application.Queries.Project.GetProjectById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers;
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ProjectsController : ControllerBase
{    
    private readonly IMediator _mediator;
    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> Get(string search = "", int page = 0, int size = 10)
    {
        var query = new GetAllProjectsQuery(search, page, size);

        var result = await _mediator.Send(query);
        
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetProjectByIdQuery(id));

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(InsertProjectCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return CreatedAtAction(nameof(GetById), new { id = result.Data }, command);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, UpdateProjectCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteProjectCommand(id));

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return NoContent();
    }

    [HttpPut("{id}/start")]
    public async Task<IActionResult> Start(int id)
    {
        var result = await _mediator.Send(new StartProjectCommand(id));

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return NoContent();
    }
    
    [HttpPut("{id}/complete")]
    public async Task<IActionResult> Complete(int id, CompleteProjectCommand command)
    {
        var commandWithId = command.Id == 0 ? command with { Id = id } : command;

        var result = await _mediator.Send(commandWithId);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return Accepted(result);
    }

    [HttpPost("{id}/comments")]
    public async Task<IActionResult> PostComment(int id, InsertCommentCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return NoContent();
    }
}