namespace MikrotikManager.Services.Routes;

public interface IRoutesSource
{
    bool CanHandle(string src);

    ValueTask<IEnumerable<string>> GetRoutesAsync(string src);
}