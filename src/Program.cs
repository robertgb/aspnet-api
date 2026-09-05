using aspnet_api.Infrastructure;
using aspnet_api.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddPresentation();

var app = builder.Build();
app.UsePresentation();

app.Run();
