using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using mosaCupBackend.Data;
using mosaCupBackend.Endpoints;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<mosaCupBackendContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("mosaCupBackendContext") ?? throw new InvalidOperationException("Connection string 'mosaCupBackendContext' not found.")));

// Add services to the container.
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

app.UseHttpsRedirection();

app.MapUserDataEndpoints();

app.MapFollowEndpoints();

app.MapPostEndpoints();

app.MapLikeEndpoints();

app.MapNotificationEndpoints();

app.Run();

