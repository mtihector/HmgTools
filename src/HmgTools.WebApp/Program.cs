using Autofac;
using Autofac.Extensions.DependencyInjection;
using HmgTools.WebApp.Services;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();



var builder = WebApplication.CreateBuilder(args);




// set DI
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//add serilog
builder.Host.UseSerilog();

// configure container

builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{

});


// Add services to the container.
builder.Services.AddRazorPages();



builder.Services.AddSingleton<IService, Service>();


var app = builder.Build();



IService service = app.Services.GetRequiredService<IService>();
Microsoft.Extensions.Logging.ILogger logger = app.Logger;
IHostApplicationLifetime lifetime = app.Lifetime;
IWebHostEnvironment env = app.Environment;

lifetime.ApplicationStarted.Register(() =>
    logger.LogInformation(
        $"The application {env.ApplicationName} started" +
        $" with injected {service}"));



lifetime.ApplicationStopped.Register(() =>
{
    logger.LogInformation(
       $"The application {env.ApplicationName} stopped");       

});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
