using System.Net;

namespace MikrotikManager.Services.Routes;

public class DomainRoutesSource : IRoutesSource
{
    public bool CanHandle(string src)
    {
        return TryGetUri(src, out _);
    }

    public async ValueTask<IEnumerable<string>> GetRoutesAsync(string src)
    {
        if (!TryGetUri(src, out var uri))
        {
            throw new InvalidOperationException("Не удалось определить URI");
        }
        var addresses = await Dns.GetHostAddressesAsync(uri!.Host);
        return addresses.Select(x => x.ToString()).ToList();
    }

    private static bool TryGetUri(string src, out Uri? uri)
    {
        src = src.StartsWith("http") ? src : $"http://{src}";
        return Uri.TryCreate(src, UriKind.Absolute, out uri);
    }
}