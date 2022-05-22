using Microsoft.AspNetCore.Mvc;
using MikrotikManager.Contracts;

namespace MikrotikManager
{
    public static class ApiMethods
    {
        public static Task Index(HttpContext context)
        {
            context.Response.Redirect("./index.html");
            return Task.CompletedTask;
        }

        public static async Task<DomainListDto> GetDomainList([FromServices] MikrotikService mikrotik)
        {
            return await mikrotik.GetDomainList();
        }

        public static async Task AddDomain(
            HttpContext context,
            [FromServices] MikrotikService mikrotik,
            [FromQuery] string domain)
        {
            await mikrotik.AddDomain(domain);
        }

        public static async Task RemoveDomain(
            [FromServices] MikrotikService mikrotik,
            [FromQuery] string domain)
        {
            await mikrotik.RemoveDomain(domain);
        }
    }
}