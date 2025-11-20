using Web.Api.Bundles;

var builder = WebApplication.CreateBuilder(args);

AppConfiguration.ConfigureDatabases(builder);
AppConfiguration.ConfigureJwt(builder);
AppConfiguration.ConfigureServices(builder);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

AppConfiguration.EnsureDatabaseMigrated(app);

app.Run();
