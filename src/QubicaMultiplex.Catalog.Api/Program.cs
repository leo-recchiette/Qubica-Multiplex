using QubicaMultiplex.Catalog.Api.Repositories;
using QubicaMultiplex.Catalog.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<CatalogService>();
builder.Services.AddScoped<CatalogRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
