namespace Offices.Services.TerminalImport;

public interface ITerminalImportService
{
    Task ImportAsync(CancellationToken ct = default);
}