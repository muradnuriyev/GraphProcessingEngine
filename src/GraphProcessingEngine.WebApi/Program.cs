using GraphProcessingEngine.Core.Repositories;
using GraphProcessingEngine.WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IGraphRepository, FileGraphRepository>();
builder.Services.AddSingleton<GraphStore>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGraphsEndpoints();
app.MapPathFindingEndpoints();

app.Run();
