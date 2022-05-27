using DanilovSoft.MikroApi;
using MikrotikManager;
using MikrotikManager.Mikrotik;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<MikrotikManager.Mikrotik.MikrotikManager>();
builder.Services.AddScoped<IMikroTikConnection, MikrotikConnectionWrapper>();
builder.Services.Configure<MikrotikConnectionSettings>(builder.Configuration.GetSection("Mikrotik"));
builder.Services.AddSingleton(typeof(Scheduler<>));
builder.Services.AddHostedService<MikrotikUpdaterHostedService>();

var app = builder.Build();

app.UseStaticFiles();
app.MapGet("/", ApiMethods.Index);
app.MapGet("/api/domain-list", ApiMethods.GetDomainList);
app.MapPost("/api/add-domain", ApiMethods.AddDomain);
app.MapPost("/api/remove-domain", ApiMethods.RemoveDomain);

app.Run();