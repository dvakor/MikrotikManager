using ProtoBuf;
using Tenray.ZoneTree.Serializers;

namespace MikrotikManager.Services.Storage;

public class ProtobufSerializer<T> : ISerializer<T>
{
    public T Deserialize(byte[] bytes)
    {
        return Serializer.Deserialize<T>(new ReadOnlyMemory<byte>(bytes));
    }

    public byte[] Serialize(in T entry)
    {
        using var ms = new MemoryStream();
        Serializer.Serialize(ms, entry);
        return ms.ToArray();
    }
}