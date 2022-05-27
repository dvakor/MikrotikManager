using Microsoft.AspNetCore.Mvc;
using MikrotikManager.Contracts;
using MikrotikManager.Mikrotik;

namespace MikrotikManager
{
    public static class ApiMethods
    {
        public static Task Index(HttpContext context)
        {
            context.Response.Redirect("./index.html");
            return Task.CompletedTask;
        }

        public static async Task<DomainListDto> GetDomainList([FromServices] Mikrotik.MikrotikManager mikrotik)
        {
            return await mikrotik.GetDomainList();
        }

        public static async Task AddDomain(
            HttpContext context,
            [FromServices] Mikrotik.MikrotikManager mikrotik,
            [FromQuery] string domain)
        {
            await mikrotik.AddDomain(domain);
        }

        public static async Task RemoveDomain(
            [FromServices] Mikrotik.MikrotikManager mikrotik,
            [FromQuery] string domain)
        {
            await mikrotik.RemoveDomain(domain);
        }
    }
}