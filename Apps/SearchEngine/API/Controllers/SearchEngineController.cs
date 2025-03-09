using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using SharedModels;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchEngineController : ControllerBase
{
    private readonly IService<DocumentSimple, Document> _service;

    public SearchEngineController(IService<DocumentSimple, Document> service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (query.Trim() == "") BadRequest();

        IEnumerable<DocumentSimple> result = await _service.QuerySearch(query);

        return result.Any() ? Ok(result) : NotFound();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetDoc([FromRoute] int id)
    {
        if (id <= 0) return BadRequest();

        Document result = await _service.GetFile(id);

        return result.DocumentID <= 0 ? Ok(result) : NotFound(); 
        
    }
}