using System.Reflection;
using Tenray.ZoneTree;

namespace MikrotikManager.Services.Storage;

public class RoutesRepository
{
    private readonly IZoneTree<string, RouteItem> _db;

    public RoutesRepository()
    {
        var location = Assembly.GetEntryAssembly()?.Location;
        
        if (string.IsNullOrEmpty(location))
        {
            throw new InvalidOperationException();
        }

        var fi = new FileInfo(location);

        var currentDir = fi.Directory!.FullName;

        _db = new ZoneTreeFactory<string, RouteItem>()
            .SetDataDirectory(currentDir)
            .SetValueSerializer(new ProtobufSerializer<RouteItem>())
            .OpenOrCreate();
    }

    public void AddRoute(string key, IEnumerable<string> routes)
    {
        _db.Upsert(key, new RouteItem
        {
            Routes = routes.ToList()
        });
    }

    public IEnumerable<KeyValuePair<string, RouteItem>> GetRoutes()
    {
        var iterator = _db.CreateIterator();

        while (iterator.Next())
        {
            yield return new KeyValuePair<string, RouteItem>(iterator.CurrentKey, iterator.CurrentValue);
        }
    }
}