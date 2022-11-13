using ProtoBuf;

namespace MikrotikManager.Services.Storage;

[ProtoContract]
public class RouteItem
{
    [ProtoMember(1)]
    public List<string> Routes { get; set; }
}