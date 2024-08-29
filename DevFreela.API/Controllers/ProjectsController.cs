using DevFreela.API.DTOs;
using DevFreela.Application.Services;
using DevFreela.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{    
    private readonly IProjectService _service;
    public ProjectsController(DevFreelaDbContext context, IProjectService service)
    {
        _service = service;
    }
    [HttpGet]
    public IActionResult Get(string search = "", int page = 0, int size = 10)
    {
        var result = _service.GetAll(search, page, size);
        
        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var result = _service.GetById(id);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Post(CreateProjectDTO model)
    {
        var result = _service.Insert(model);
        
        return CreatedAtAction(nameof(GetById), new { id = result.Data }, model);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, UpdateProjectDTO model)
    {
        var result = _service.Update(id, model);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _service.Delete(id);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return NoContent();
    }

    [HttpPut("{id}/start")]
    public IActionResult Start(int id)
    {
        var result = _service.Start(id);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return NoContent();
    }
    
    [HttpPut("{id}/complete")]
    public IActionResult Complete(int id)
    {
        var result = _service.Complete(id);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return NoContent();
    }

    [HttpPost("{id}/comments")]
    public IActionResult Post(int id, CreateProjectCommentDTO model)
    {
        var result = _service.InsertComment(id, model);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        
        return NoContent();
    }
}