using System.Net;
using DanilovSoft.MikroApi;
using Microsoft.Extensions.Options;
using MikrotikManager.Contracts;
using MikrotikManager.Mikrotik.Models;

namespace MikrotikManager.Mikrotik
{
    public sealed class MikrotikManager
    {
        private readonly IOptions<MikrotikConnectionSettings> _settings;
        private readonly Scheduler<MikrotikManager> _scheduler;
        private readonly IMikroTikConnection _connection;

        public MikrotikManager(
            IOptions<MikrotikConnectionSettings> settings,
            Scheduler<MikrotikManager> scheduler,
            IMikroTikConnection connection)
        {
            _settings = settings;
            _scheduler = scheduler;
            _connection = connection;
        }
        
        public async Task<DomainListDto> GetDomainList()
        {
            var list = await GetDomainListInnerAsync();

            return new DomainListDto
            {
                Domains = list
                    .Where(x => !string.IsNullOrEmpty(x.Comment))
                    .Where(x => x.Comment?.Equals("domain", StringComparison.OrdinalIgnoreCase) ?? false)
                    .Select(x => x.Address)
                    .OrderBy(x => x)
            };
        }

        public async Task AddDomain(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain))
            {
                return;
            }
            
            if (!domain.StartsWith("http"))
            {
                domain = $"http://{domain}";
            }
            
            var uri = new Uri(domain, UriKind.Absolute);
            var settings = _settings.Value;
            var api = await GetConnectionAsync();

            var domains = await GetDomainListInnerAsync();

            if (domains.Any(x => x.Comment?.Equals(uri.Host) ?? false))
            {
                return;
            }
            
            await api
                .Command("/ip firewall address-list add")
                .Attribute("list", settings.VpnName)
                .Attribute("comment", "domain")
                .Attribute("address", uri.Host)
                .SendAsync();
            
            _scheduler.Reset();
        }

        public async Task RemoveDomain(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain))
            {
                return;
            }
            
            if (!domain.StartsWith("http"))
            {
                domain = $"http://{domain}";
            }
            
            var uri = new Uri(domain, UriKind.Absolute);
            
            var api = await GetConnectionAsync();

            var domains = await GetDomainListInnerAsync();

            var domainToRemove = domains.FirstOrDefault(x => x.Address.Equals(uri.Host));
            
            if (domainToRemove == null)
            {
                return;
            }
            
            await api
                .Command("/ip firewall address-list remove")
                .Attribute("numbers", domainToRemove.Id)
                .SendAsync();
            
            _scheduler.Reset();
        }
        
        public async Task UpdateRoutes()
        {
            var settings = _settings.Value;
            var api = await GetConnectionAsync();
            var domains = await GetDomainListInnerAsync();
            var ips = domains
                .Where(x => IPAddress.TryParse(x.Address, out _))
                .Where(x => x.Name.Equals(settings.VpnName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var routes = await api
                .Command("/ip route print")
                .ToListAsync<RouteListItem>();

            var routesToDelete = routes
                .Where(x => x.Comment?.Equals(settings.VpnName, StringComparison.OrdinalIgnoreCase) ?? false)
                .Select(x => x.Id)
                .ToList();

            if (routesToDelete.Any())
            {
                await api
                    .Command("/ip route remove")
                    .Attribute("numbers", string.Join(',', routesToDelete))
                    .SendAsync();
            }

            var subnets = ips.Select(ToSubnet).Distinct().ToList();

            foreach (var ip in subnets)
            {
                await api
                    .Command("/ip route add")
                    .Attribute("dst-address", ip)
                    .Attribute("gateway", settings.VpnName)
                    .Attribute("comment", settings.VpnName)
                    .SendAsync();
            }
        }

        private static string ToSubnet(AddressListItem addressListItem)
        {
            var ip = addressListItem.Address
                .Split('.')
                .Take(3)
                .ToList();
            
            ip.Add("0/24");

            return string.Join('.', ip);
        }

        private async ValueTask<IMikroTikConnection> GetConnectionAsync()
        {
            if (_connection.Connected)
            {
                return _connection;
            }
            
            var settings = _settings.Value;
            
            await _connection.ConnectAsync(
                settings.Login, settings.Password, 
                settings.Address, false, settings.Port, CancellationToken.None);
            
            return _connection;
        }
        
        private async Task<List<AddressListItem>> GetDomainListInnerAsync()
        {
            var api = await GetConnectionAsync();
            var list = await api
                .Command("/ip firewall address-list print")
                .ToListAsync<AddressListItem>();
            return list;
        }
    }
}