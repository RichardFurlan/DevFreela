using DevFreela.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Options;

namespace DevFreela.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly FreelanceTotalCostConfig _config;
    public ProjectsController(IOptions<FreelanceTotalCostConfig> options)
    {
        _config = options.Value;
    }
    
    [HttpGet]
    public IActionResult Get(string search)
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        return Ok();
    }

    [HttpPost]
    public IActionResult Post(CreateProjectDTO model)
    {
        if (model.TotalCost < _config.Minimum || model.TotalCost > _config.Maximum)
        {
            return BadRequest("Numero fora dos limites.");
        }
        
        return CreatedAtAction(nameof(GetById), new { id = 1 }, model);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, UpdateProjectDTO model)
    {
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return NoContent();
    }

    [HttpPut("{id}/start")]
    public IActionResult Start(int id)
    {
        return NoContent();
    }
    
    [HttpPut("{id}/complete")]
    public IActionResult Complete(int id)
    {
        return NoContent();
    }

    [HttpPost("{id}/comments")]
    public IActionResult Post(int id, CreateProjectCommentDTO model)
    {
        return Ok();
    }
}