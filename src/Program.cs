using aspnet_api.Infrastructure;
using aspnet_api.Presentation;
using aspnet_api.Core.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation();

var app = builder.Build();
app.UsePresentation();

app.Run();
