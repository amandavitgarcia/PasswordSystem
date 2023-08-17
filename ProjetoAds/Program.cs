
using ProjetoAds.CrossCutting.ExtensionMethods;
using ProjetoAds.CrossCutting.Helpers.Auth.Services;
using ProjetoAds.CrossCutting.Settings;
using ProjetoAds.Data.Repository;
using ProjetoAds.Domain.Repository;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger();
builder.Services.AddJwtBearerAuthentication(builder.Configuration);

ConfigureSettings(builder);
ConfigureServices(builder);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
    builder => builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"));
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

void ConfigureSettings(WebApplicationBuilder builder)
{
    var settings = new Settings();
    builder.Configuration.GetSection("ConnectionStrings").Bind(settings);
}

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();


 

    builder.Services.AddTransient<IPasswordRepository, PasswordRepository>();


    builder.Services.AddScoped<ITokenService, TokenService>();
}