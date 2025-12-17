var builder = WebApplication.CreateBuilder(args);

// DI - Add services to the container

var app = builder.Build();

// Configure the HTTP request pipeline.

app.Run();
