// USING
using API.Middlewares;
using Infrastructure;
using Serilog;
// builder config
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.EnableAnnotations();
});
builder.Services.AddInfrastructure(configuration);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
// app config
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(option => option.DisplayRequestDuration());
}
app.MapMinimalAPI();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.Run();
