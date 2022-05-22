using DanilovSoft.MikroApi;

namespace MikrotikManager.MikrotikModels
{
    public class AddressListItem
    {
        [MikroTikProperty(".id")]
        public string Id { get; set; }
        
        [MikroTikProperty("address")]
        public string Address { get; set; }
        
        [MikroTikProperty("comment")]
        public string? Comment { get; set; }
        
        [MikroTikProperty("list")]
        public string Name { get; set; }
    }
}