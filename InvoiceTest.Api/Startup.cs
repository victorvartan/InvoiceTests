using InvoiceTest.Api.Security;
using InvoiceTest.DataAccess;
using InvoiceTest.Services;
using InvoiceTest.Services.Interfaces;
using InvoiceTest.Services.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.SqlClient;

namespace InvoiceTest.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            ConfigureDependencyInjection(services);
            ConfigureAuthentication(services);
            ConfigureEntityFramework(services);
        }

        private void ConfigureEntityFramework(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer()
               .AddDbContext<DataContext>(options =>
               {
                   var connectionString = new SqlConnectionStringBuilder(Configuration.GetConnectionString("DefaultConnection")) { Password = Configuration["ConnectionStrings:DefaultConnectionPassword"] }.ConnectionString;
                   options.UseSqlServer(connectionString,
                       sqlServerOptionsAction: sqlOptions =>
                       {
                           sqlOptions.MigrationsAssembly("InvoiceTest.DataAccess");
                           sqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(20), errorNumbersToAdd: null);
                       });
               }, ServiceLifetime.Scoped);
        }

        private void ConfigureDependencyInjection(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IAuthenticatedPrincipal, AuthenticatedPrincipal>();

            services.AddTransient<IInvoiceService, InvoiceService>();
            services.AddTransient<IInvoiceNoteService, InvoiceNoteService>();
            services.AddTransient<IUserService, UserService>();
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CustomAuthenticationExtensions.AUTHENTICATION_SCHEME;
                options.DefaultChallengeScheme = CustomAuthenticationExtensions.AUTHENTICATION_SCHEME;
            })
            .AddCustomAuth(o => { });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
