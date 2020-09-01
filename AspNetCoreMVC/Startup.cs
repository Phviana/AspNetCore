using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCoreMVC.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AspNetCoreMVC
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var connectionString = Configuration.GetConnectionString("Default");

            services.AddDbContext<ApplicationContext>(options => 
                            options.UseSqlServer(connectionString)
                            );

            services.AddTransient<IDataService, DataService>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IRegisterRepository, RegisterRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IHttpHelper, HttpHelper>();
            services.AddTransient<IReportHelper, ReportHelper>();

            //services.AddAuthentication()
            //    .AddMicrosoftAccount(options =>
            //    {
            //        options.ClientId = Configuration["ExternalLogin:Microsoft:ClientId"];
            //        options.ClientSecret = Configuration["ExternalLogin:Microsoft:ClientSecret"];
            //    });


            services.AddAuthentication(options =>
            {
                //Authentication user form
                options.DefaultScheme = "Cookies";
                // Protocol that defines the authentication flow
                options.DefaultChallengeScheme = "OpenIdConnect";

            })
                .AddCookie()
                .AddOpenIdConnect(options =>
                {
                    options.SignInScheme = "Cookies";
                    options.Authority = Configuration["AspNetCoreMVC.IdentityServer_Url"];
                    options.ClientId = "ASPNETCOREMVC";
                    options.ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";
                    options.SaveTokens = true;
                    options.ResponseType = "code id_token";
                    options.RequireHttpsMetadata = false;
                });

            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddHttpClient<IReportHelper,ReportHelper>();
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Order}/{action=Carousel}/{Code?}");
            });

            var dataService = serviceProvider.GetService<IDataService>();
            dataService.DBInicializeAsync(serviceProvider).Wait();
        }
    }
}

