namespace MikrotikManager.Services.Routes;

public class RouteSourceFactory
{
    private readonly IEnumerable<IRoutesSource> _routesSources;

    public RouteSourceFactory(IEnumerable<IRoutesSource> routesSources)
    {
        _routesSources = routesSources;
    }

    public IRoutesSource GetRouteSource(string src)
    {
        var source = _routesSources.FirstOrDefault(x => x.CanHandle(src));

        if (source == null)
        {
            throw new InvalidOperationException("Не удалось определить источник маршрутов");
        }

        return source;
    }
}