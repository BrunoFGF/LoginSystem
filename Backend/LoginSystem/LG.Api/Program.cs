using LG.Application.Extensions;
using LG.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

var Cors = "Cors";

builder.Services.AddInjectionInfrastructure(Configuration);
builder.Services.AddInjectionApplication(Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: Cors,
       builder =>
       {
           builder.WithOrigins("*");
           builder.AllowAnyMethod();
           builder.AllowAnyHeader();
       });
});

var app = builder.Build();

app.UseCors(Cors);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
