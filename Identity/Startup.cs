using Identity.DbContext;
using Identity.Filter;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity
{

    public class Startup
    {
        // Startup
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }






        // Configure Services ================================================================================= 
        public void ConfigureServices(IServiceCollection services)// This method gets called by the runtime. Use this method to add services to the container.
        {



            // Log in - DbContext
            services.AddDbContextPool<IdentityContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("IdentityContextConnection")));


            // UnitOfWork - Filter
            services.AddScoped<UnitOfWorkFilter>();
            services.AddControllers(config => { config.Filters.AddService<UnitOfWorkFilter>(); });  // UnitOfWork for all Controllers

            // CORS
            services.AddCors();




            // Log In
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

            }).AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders(); // AddDefaultTokenProviders is used for the Update Log In Password etc.







            // Log In
            // Make all Controllers protected by default so only Authorized Users can accsess them, for Anonymouse Users use [AlloAnonymouse] over the controllers.
            services.AddMvc(options => {
                var policy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();
                options.Filters.Add(new AuthorizeFilter(policy));

            }).AddXmlSerializerFormatters();

            //services.AddControllers();






            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity", Version = "v1" });
            });
        }









        // Configure ===========================================================================================
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        {
            // Default Code------------------------------------------------------------------------------------>
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Swagger
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity v1"));
            }




            app.UseHsts(); // Allow HTTPS
            app.UseHttpsRedirection();
            app.UseRouting();

            // CORS - Allow calling the API from WebBrowsers
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins seperated with comma
                .SetIsOriginAllowed(origin => true));// Allow any origin  




            // Log In
            app.UseAuthentication(); // UseAuthentication SHOULD ALWAYS BE BEFORE Authorization
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
