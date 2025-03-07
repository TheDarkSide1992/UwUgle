using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchEngineController : ControllerBase
{
    private readonly IService<string, string> _service;

    public SearchEngineController(IService<string, string> service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (query.Trim() == "") BadRequest();

        IEnumerable<string> result = await _service.QuerySearch(query);

        return result.Any() ? Ok(result) : NotFound();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetDoc([FromRoute] int id)
    {
        if (id <= 0) return BadRequest();

        string result = await _service.GetFile(id);

        return result != null ? Ok(result) : NotFound(); 
        
    }
}