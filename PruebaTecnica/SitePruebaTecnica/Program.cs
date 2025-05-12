using BussinesClass;
using BussinesClass.Interfaces;
using Dtos.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

EndpointsDto endpoint = new() { UrlBackenApi = builder.Configuration.GetSection("Back").GetSection("UrlBackenApi").Value };
builder.Services.AddSingleton<EndpointsDto>(endpoint);

//Tasas de interes

ParametrosTasas parametrosTasas = new()
{
    InteresCofigurable= Convert.ToDouble(builder.Configuration.GetSection("Tasas_Interes").GetSection("Interes_configurable").Value),
    PorcentageConfigurable= Convert.ToDouble(builder.Configuration.GetSection("Tasas_Interes").GetSection("Porcentage_configurable").Value),
};

builder.Services.AddSingleton<ParametrosTasas>(parametrosTasas);

builder.Services.AddScoped<ITransaccionesClientes, TransaccionesBol>();
builder.Services.AddScoped<ITransaccionesService, TransaccionesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
