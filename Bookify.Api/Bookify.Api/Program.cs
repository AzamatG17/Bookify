using Bookify.Api.Extensions;
using Bookify.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

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

app.UseErrorHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
