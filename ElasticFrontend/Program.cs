using App.Metrics;
using App.Metrics.Formatters.Json;
using App.Metrics.Formatters.Prometheus;
using Elastic.Apm.NetCoreAll;
using ElasticFrontend;
using ElasticFrontend.Areas.Identity.Data;
using ElasticFrontend.Metric;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DataConnection") ?? throw new InvalidOperationException("Connection string 'DataConnection' not found.");

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddHttpClient();

builder.Services.AddDefaultIdentity<SampleUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IdentityContext>();

builder.Services.AddHealthChecks();

builder.WebHost.ConfigureKestrel(serverOpions =>
{
    serverOpions.ListenAnyIP(5000);
});

builder.WebHost.UseKestrel(options =>
{
    options.AllowSynchronousIO = true;
});

// Default Builder von AppMetrics + Endpoint Format
builder.Services.AddMetrics(AppMetrics.CreateDefaultBuilder().Build());

builder.Services.AddMetricsEndpoints(options =>
{
    options.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter(); // /metrics-text
    options.MetricsEndpointOutputFormatter = new MetricsJsonOutputFormatter(); // /metrics
    options.EnvironmentInfoEndpointEnabled = true; // /env
});

// Beispiel mit File Reporter
/*     
builder.WebHost.ConfigureMetricsWithDefaults(options =>
{
    options.Report.ToTextFile(options =>
    {
        options.MetricsOutputFormatter = new MetricsJsonOutputFormatter();
        options.OutputPathAndFileName = @"C:\temp\metrics\frontend_metrics.json";
        options.FlushInterval = TimeSpan.FromSeconds(5);

    });
});
builder.Services.AddMetricsReportingHostedService();
*/


// Beispiel mit HTTP-Reporter direkt nach Elasticsearch
/*
var filter = new MetricsFilter().WhereType(App.Metrics.MetricType.Timer);
builder.Services.AddMetrics(new MetricsBuilder()
    .Configuration.Configure(options =>
    {
        options.ReportingEnabled = true;
    }).Report.OverHttp(options =>
    {
        options.HttpSettings.AllowInsecureSsl = true;
        options.HttpSettings.RequestUri = new Uri("http://localhost:5000/metrics");
        //options.HttpSettings.UserName = "elastic";
        //options.HttpSettings.Password = "hwgQ3S2_miEdmOpJE6**";
        options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
        options.HttpPolicy.FailuresBeforeBackoff = 5;
        options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
        options.MetricsOutputFormatter = new MetricsPrometheusTextOutputFormatter();
        options.Filter = filter;
        options.FlushInterval = TimeSpan.FromSeconds(30);
    })
.Build());
*/

// Beispiel Reporting to Elasticsearch 
/*
builder.Host
.UseSerilog((context, configuration) =>
{
    var uri = new Uri(context.Configuration["ElasticConfiguration:Uri"]);

    configuration.ReadFrom.Configuration(context.Configuration)
    .Enrich.WithMachineName()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(
        new ElasticsearchSinkOptions(uri)
        {
            ModifyConnectionSettings = x => x.ServerCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true)
            .BasicAuthentication("elastic", "hwgQ3S2_miEdmOpJE6**"),
            IndexFormat = $"{context.Configuration["ApplicationLoggerName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.Now:dd-MM-yyyy}",
            AutoRegisterTemplate = true,
            NumberOfShards = 2,
            NumberOfReplicas = 1,
        })
       .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);
}).UseMetricsWebTracking();
*/
builder.Services.AddSingleton<MetricsIndexer>();

// Add services to the container
builder.Services.AddRazorPages();

var app = builder.Build();

//Metrics-Config
app.UseMiddleware<MetricMiddleware>();
app.UseMetricsAllEndpoints();
//



app.UseAllElasticApm(app.Configuration);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");

app.Services.GetRequiredService<MetricsIndexer>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); ;

app.UseAuthorization();

app.MapRazorPages();

Log.Information($"************************Starting web application************************");

app.Run();
