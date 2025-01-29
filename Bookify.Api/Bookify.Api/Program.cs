using Bookify.Api.Extensions;
using Bookify.Application.Extensions;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .WriteTo.File("logs/logs_.txt", LogEventLevel.Information, rollingInterval: RollingInterval.Day)
        .WriteTo.File("logs/errors_.txt", LogEventLevel.Error, rollingInterval: RollingInterval.Day)
        .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // ��������� ������ � ����� �������
              .AllowAnyHeader() // ��������� ����� ���������
              .AllowAnyMethod(); // ��������� ����� HTTP-������ (GET, POST � �.�.)
    });
});

builder.Services
    .RegisterApplication(builder.Configuration)
    .RegisterApi(builder.Configuration);

builder.Services.AddHttpClient();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDatabaseSeeder();
}

app.UseCors("AllowAll");

app.UseErrorHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
