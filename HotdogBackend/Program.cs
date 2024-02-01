using Microsoft.IdentityModel.Tokens;
using System.Text;

using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using OtelCommon;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

TraceActivities.Source = new System.Diagnostics.ActivitySource(ServiceNames.HotDogBackend);
builder.Services.AddOpenTelemetry().ConfigureResource(
    rb => rb
    .AddService(ServiceNames.HotDogService) //This is the name of the service that will show up in jaeger
        .AddAttributes(new List<KeyValuePair<string, object>> // These are attributes added to every trace
            {
                new KeyValuePair<string, object>("application", ServiceNames.HotDogBackend),
                new KeyValuePair<string, object>("environment", "dev"),
                new KeyValuePair<string, object>("version", "0.0.5")
            }
        )
    ).WithTracing(builder => builder
    .AddSource(ServiceNames.HotDogBackend)
    .AddAspNetCoreInstrumentation()
    .AddHttpClientInstrumentation()
    .AddSqlClientInstrumentation().AddConsoleExporter()
    .AddOtlpExporter(options =>
    {
        options.Endpoint = new Uri("http://localhost:6831"); //Jaeger endpoint
        options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
    })
    );

    var tokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidAudiences = getValidAudiences(builder.Configuration),
        ValidIssuer = ServiceNames.HotDogBackend,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["ApiKey"]!))
    };

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
    {
        o.SaveToken = true;
        o.TokenValidationParameters = tokenValidationParameters;
    });



IEnumerable<string> getValidAudiences(ConfigurationManager configuration)
{
    //Get all the keys in the AuthorizedClients section of the configuration
    IEnumerable<string> keys = configuration.GetSection("AuthorizedClients").GetChildren().Select(x => x.Key);
    return keys;
}





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
