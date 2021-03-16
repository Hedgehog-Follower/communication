using System;
using System.IO;
using System.Reflection;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Sending.API.Handlers;
using Sending.API.HealthChecks;
using Sending.API.HttpClients;
using Sending.API.Options.Clients;
using Sending.API.Options.HealthChecks;
using Sending.API.Policies;
using Sending.Domain;

namespace Sending.API
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<ApplicationContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("MsSqlConnectionString"),
                        sqlServerOptions => sqlServerOptions.MigrationsAssembly("Sending.API"));
                });

            services
                .AddProblemDetails(opts =>
                {
                    opts.IncludeExceptionDetails = (ctx, ex) => Environment.IsDevelopment();
                })
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            services
                .AddSwaggerGen(setup =>
                {
                    setup.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Welcome API",
                        Description = "Example of configuration"
                    });

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    setup.IncludeXmlComments(xmlPath);
                });

            services
                .Configure<TestConfiguration>(Configuration.GetSection(TestConfiguration.ConfigurationName));
                //.Configure<BaseHealthCheckConfiguration>(Configuration.GetSection("HealthChecks:Ready"));

            services.TryAddSingleton<ITestConfiguration>(sp =>
                sp.GetRequiredService<IOptions<TestConfiguration>>().Value);

            services
                .AddPollyPolicies()
                .AddHttpClient<ITestClient, TestClient>()
                .AddPolicyHandlerFromRegistry(PolicyNames.Retry);
                //.RegisterAndAddHttpMessageHandler<ValidateHeaderHandler>();

            services
                .AddHttpClient<IWelcomeClient, WelcomeClient>()
                .AddPolicyHandlerFromRegistry(PolicyNames.RetryWithLogging)
                .AddPolicyHandlerFromRegistry(PolicyNames.Timeout)
                .RegisterAndAddHttpMessageHandler<ValidateHeaderHandler>();

            services.AddSingleton<IHttpClientForSingletonConsumers, HttpClientForSingletonConsumers>();


            services
                .AddHealthChecks()
                .AddUriHealthCheck(Configuration.GetSection($"{UriHealthCheckConfiguration.SectionPointer}{UriHealthCheckNames.Google}"))
                .AddDatabaseContextCheck<ApplicationContext>
                    (Configuration.GetSection($"{DbContextHealthCheckConfiguration.SectionPointer}{DbContextCheckNames.ApplicationContext}"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseProblemDetails();


            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            // Endpoint routing separates the process of selecting which "endpoint" will execute from the actual running of that endpoint.
            // An endpoint consists of a path pattern, and something to execute when called.

            // The UseRouting() extension method is what looks at the incoming request and decides which endpoint should execute.
            // Any middleware that appears after the UseRouting() call will know which endpoint will run eventually.
            app.UseRouting();

            // The UseEndpoints() call is responsible for configuring the endpoints, but also for executing them.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Sending API!");
                });
                endpoints.MapDefaultHealthChecks();
            });
        }
    }
}
