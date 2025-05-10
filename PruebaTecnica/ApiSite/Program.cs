using Aplication;
using Domain.Interfaces;
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

// Add services .

builder.Services.AddScoped<ITransaccionesClientes, TransaccionesClientes>();
builder.Services.AddScoped<ITransaccionesAplication, TransaccionesAplication>();

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
