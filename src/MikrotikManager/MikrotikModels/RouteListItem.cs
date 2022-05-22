using DanilovSoft.MikroApi;

namespace MikrotikManager.MikrotikModels
{
    public class RouteListItem
    {
        [MikroTikProperty(".id")]
        public string Id { get; set; }
        
        [MikroTikProperty("dst-address")]
        public string DstAddress { get; set; }
        
        [MikroTikProperty("comment")]
        public string? Comment { get; set; }
        
        [MikroTikProperty("gateway")]
        public string Gateway { get; set; }
    }
}