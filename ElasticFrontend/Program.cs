using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.EntityFrameworkCore;
using Elastic.Apm.NetCoreAll;
using ElasticFrontend;
using ElasticFrontend.Areas.Identity.Data;
using ElasticFrontend.Metric;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DataConnection") ?? throw new InvalidOperationException("Connection string 'DataConnection' not found.");

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<SampleUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IdentityContext>();

builder.Services.AddHealthChecks();

builder.WebHost.ConfigureKestrel(serverOpions =>
{
    serverOpions.ListenAnyIP(5000);
});


X509Certificate2 caCertificate = new X509Certificate2("C:\\Projects\\DiplomArbeit\\Playground_AspNetElastic\\Elasticsearch_Kibana-Local\\elasticsearch-8.10.2_nodel1\\config\\certs\\http_ca.crt");
X509Chain chain = new X509Chain();
chain.ChainPolicy.ExtraStore.Add(caCertificate);

builder.Host
    .UseSerilog((context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration)
        .Enrich.WithMachineName()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(
            new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfiguration:Uri"]))
            {
                ModifyConnectionSettings = x => x.ServerCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true)
                .BasicAuthentication("elastic", "hwgQ3S2_miEdmOpJE6**"),
                IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.Now:dd-MM-yyyy}",
                AutoRegisterTemplate = true,
                NumberOfShards = 2,
                NumberOfReplicas = 1,
            })
           .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);
    })
    .UseMetricsWebTracking()
    .UseMetrics(options =>
    {
        options.EndpointOptions = endpointOptions =>
        {
            endpointOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
            endpointOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
            endpointOptions.EnvironmentInfoEndpointEnabled = true;
        };
    });
builder.Services.AddSingleton<Indexer>();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseMiddleware<MetricMiddleware>();
app.UseMetricsAllMiddleware();
app.UseMetricsAllEndpoints();

app.UseAllElasticApm(app.Configuration);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMetricServer();

app.UseHealthChecks("/health");

app.UseAllElasticApm(app.Configuration, new HttpDiagnosticsSubscriber(),
    new EfCoreDiagnosticsSubscriber());

app.Services.GetRequiredService<Indexer>();


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); ;

app.UseAuthorization();

app.MapRazorPages();

Log.Information($"************************Starting web application************************");

app.Run();
