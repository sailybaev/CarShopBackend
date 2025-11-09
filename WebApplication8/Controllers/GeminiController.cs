using Microsoft.AspNetCore.Mvc;
using WebApplication8.Services;

namespace WebApplication8.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GeminiController : ControllerBase
{
    private readonly GeminiService _geminiService;

    public GeminiController(GeminiService geminiService)
    {
        _geminiService = geminiService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] PromptRequest request)
    {
        try
        {
            var response = await _geminiService.GenerateContentAsync(request.Prompt);
            return Ok(new { response });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
}

public class PromptRequest
{
    public string Prompt { get; set; } = string.Empty;
}

