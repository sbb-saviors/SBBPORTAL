
using API.Authorization;
using API.Helpers;
using API.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("portal-api", new OpenApiInfo
    {
        Title = "PORTAL API",
        Version = "v1",
        Description = "Verisyon 1.0"
    });


    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "lütfen token giriniz.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});



IConfiguration configuration = builder.Configuration;
builder.Services.AddDbContext<CORE.Models.AppDbContext>(opt =>
        opt.UseNpgsql(
            configuration.GetConnectionString("WebApiDatabase")));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

services.AddCors();
services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

services.AddScoped<IJwtUtils, JwtUtils>();
services.AddScoped<IUserService, UserService>();

// Register code
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    c.SwaggerEndpoint("/swagger/portal-api/swagger.json", "PORTAL APÝ");
});




var supportedCultures = new[]
{
 new CultureInfo("tr-TR"),
 new CultureInfo("tr"),
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("tr-TR"),
    // Formatting numbers, dates, etc.
    SupportedCultures = supportedCultures,
    // UI strings that we have localized.
    SupportedUICultures = supportedCultures
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAll");


app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseMiddleware<JwtMiddleware>();

app.Run();


app.Run();
