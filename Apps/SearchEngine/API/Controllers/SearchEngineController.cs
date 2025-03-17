using Logger;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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

    /**
     * Recives Seartch request bassed on Query
     */
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        using var activity = Monitoring.ActivitySource.StartActivity();
        
        if (query.Trim() == "") BadRequest();

        IEnumerable<DocumentSimple> result = await _service.QuerySearch(query);

        return result.Any() ? Ok(result) : NotFound();
    }

    /**
     * Recives request to retrive document
     */
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetDoc([FromRoute] int id)
    {
        using var activity = Monitoring.ActivitySource.StartActivity();

        if (id <= 0) return BadRequest();

        Log.Logger.Here().Debug($@"Attempting to retrieve document {id} ");
        Console.WriteLine($@"test");
        Document result = await _service.GetFile(id);
        
        if (result.DocumentID <= 0) return BadRequest();
        
        Response.Headers.Append("Content-Disposition", "attachment; filename=FileID" + result.DocumentID);
        var file = File(result.File, "application/txt", result.DocumentName+".txt");
        
        return file;; 
        
    }
}