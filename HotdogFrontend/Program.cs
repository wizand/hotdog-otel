using HotdogFrontend.Components;
using HotdogFrontend.Components.Account;
using HotdogFrontend.Data;
using HotdogFrontend.ManagersAndHandlers;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using OtelCommon;

namespace HotdogFrontend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //This is used to keep track of the Jwt token that enables the frontend to communicate with the backend
            builder.Services.AddSingleton<JwtTokenHandler>();
            //Centralized hhtp client to abstract the management of the http client
            builder.Services.AddHttpClient();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();



            TraceActivities.Source = new System.Diagnostics.ActivitySource(ServiceNames.HotDogFrontend);

            builder.Services.AddOpenTelemetry().ConfigureResource(
                rb => rb
                .AddService(ServiceNames.HotDogService) //This is the name of the service that will show up in jaeger
                    .AddAttributes(new List<KeyValuePair<string, object>> // These are attributes added to every trace
                        {
                        new KeyValuePair<string, object>("application", ServiceNames.HotDogFrontend),
                        new KeyValuePair<string, object>("environment", "dev"),
                        new KeyValuePair<string, object>("version", "0.0.5")
                        }
                    )
                ).WithTracing(builder => builder
                .AddSource(ServiceNames.HotDogFrontend)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddSqlClientInstrumentation()
                .AddConsoleExporter()
                .AddOtlpExporter(options => 
                    {
                        options.Endpoint = new Uri("http://localhost:6831"); //Jaeger endpoint
                        options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    })
                );

                
            builder.Services.AddScoped<OrderService>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            app.Run();
        }
    }
}
