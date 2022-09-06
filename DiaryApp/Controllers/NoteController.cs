using Microsoft.AspNetCore.Mvc;

namespace DiaryApp.Controllers;

[ApiController]
[Route("Notes")]
public class NoteController : ControllerBase
{
    public NoteController()
    {
        
    }
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Search(string q)
    {
        Console.WriteLine(Request);
        Console.WriteLine(HttpContext);
        return new OkResult();
    }
}