using Aplication;
using Domain.Dto;
using Domain.FlueValidation;
using Domain.Interfaces;
using FluentValidation;
using Infraestructur;
using Infraestructur.Interface;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

//Conection sql
builder.Services.AddScoped<SqlConnection>(p => 
{
    var conection = builder.Configuration.GetConnectionString("DbConnection");
    return  new SqlConnection(conection);
}
);
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

builder.Logging.AddLog4Net(new Log4NetProviderOptions
{
    Log4NetConfigFileName = "log4net.config",
    Watch = true // Para detectar cambios en el archivo de configuración
});

// Add services .

builder.Services.AddScoped<ITransaccionesClientes, TransaccionesClientes>();
builder.Services.AddScoped<ITransaccionesAplication, TransaccionesAplication>();
builder.Services.AddScoped(typeof(IUnitOfWork<>),typeof(UnitOfWork<>));
/*Flue validation*/
builder.Services.AddScoped<IValidator<ClienteInput>, ClienteValidador>();
builder.Services.AddScoped<IValidator<Transacciones>, TransaccionesValidador>();

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
