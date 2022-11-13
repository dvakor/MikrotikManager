using MikrotikManager.Services.Routes;
using MikrotikManager.Services.Storage;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddScoped<MikrotikManager.Mikrotik.MikrotikManager>();
//builder.Services.AddScoped<IMikroTikConnection, MikrotikConnectionWrapper>();
//builder.Services.Configure<MikrotikConnectionSettings>(builder.Configuration.GetSection("Mikrotik"));
//builder.Services.AddSingleton(typeof(Scheduler<>));
//builder.Services.AddHostedService<MikrotikUpdaterHostedService>();
builder.Services.AddSingleton<IRoutesSource, DomainRoutesSource>();
builder.Services.AddSingleton<IRoutesSource, IpRoutesSource>();
builder.Services.AddSingleton<IRoutesSource, SubnetRouteSource>();
builder.Services.AddSingleton<RouteSourceFactory>();
builder.Services.AddSingleton<RoutesRepository>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
//app.MapGet("/", ApiMethods.Index);
//app.MapGet("/api/domain-list", ApiMethods.GetDomainList);
//app.MapPost("/api/add-domain", ApiMethods.AddDomain);
//app.MapPost("/api/remove-domain", ApiMethods.RemoveDomain);

app.Run();