using Bookify.Api.Extensions;
using Bookify.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // Разрешает доступ с любых доменов
              .AllowAnyHeader() // Разрешает любые заголовки
              .AllowAnyMethod(); // Разрешает любые HTTP-методы (GET, POST и т.д.)
    });
});

builder.Services
    .RegisterApplication(builder.Configuration)
    .RegisterApi(builder.Configuration);

builder.Services.AddHttpClient();

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
