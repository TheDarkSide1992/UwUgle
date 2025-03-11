using Infrastructure;
using Infrastructure.Implementations;
using Infrastructure.Interface;
using Service.Implementations;
using Service.Interfaces;
using SharedModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddNpgsqlDataSource(Environment.GetEnvironmentVariable("pgconn")!,
    dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());

builder.Services.AddScoped<ISearchRespository<DocumentSimple, Document>, SeartchRepositoryDocument>();
builder.Services.AddScoped<IService<DocumentSimple, Document>, ServiceDocument>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
