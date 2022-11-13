using System.Text.RegularExpressions;

namespace MikrotikManager.Services.Routes;

public class SubnetRouteSource : IRoutesSource
{
    private static readonly Regex Regex = new(
        @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}\/[0-9]{1,3}$",
        RegexOptions.Compiled);
    
    public bool CanHandle(string src)
    {
        return Regex.IsMatch(src);
    }

    public ValueTask<IEnumerable<string>> GetRoutesAsync(string src)
    {
        return ValueTask.FromResult(new List<string>()
        {
            src
        }.AsEnumerable());
    }
}