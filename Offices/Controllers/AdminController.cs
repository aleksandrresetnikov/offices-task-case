using Microsoft.AspNetCore.Mvc;
using Offices.Services.TerminalImport;

namespace Offices.Controllers;

[ApiController]
[Route("v1/api/admin")]
public class AdminController(ITerminalImportService _terminalImportService) : ControllerBase
{
    [HttpGet("import-terminals")]
    public async Task<ActionResult> HandleImportAsync(CancellationToken ct)
    {
        await _terminalImportService.ImportAsync(ct);
        return NoContent();
    }
}