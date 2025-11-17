using Data.ContextMysql;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Web.Api.Bundles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MySqlDbContext>(options =>
{
    var connection = builder.Configuration.GetConnectionString("SchoolDb");

    if (string.IsNullOrEmpty(connection))
    {
        throw new Exception("Could not find connection string for mysql db.");
    }

    options.UseMySQL(connection);
});

builder.Services.AddScoped<IApplicationUnitOfWorkMySql, ApplicationUnitOfWorkMySql>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<MySqlDbContext>();
var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

if (db.Database.GetPendingMigrations().Any())
{
    db.Database.Migrate();
}

await DefaultUserSeed.SeedAdminAsync(db, config);

app.Run();
