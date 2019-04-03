using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using GMSTEK.Data;
using GMSTEK.Models;

namespace GMSTEK
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
            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"))
            );

        
            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDBContext>()
                    .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = "mydomain.com",
                         ValidAudience = "mydomain.com",
                         IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["secretKey"])),
                         ClockSkew = TimeSpan.Zero
                     });

            services.AddScoped<IRepository<Invoice>, Repository<Invoice>>();
            services.AddScoped<IRepository<Item>, Repository<Item>>();
            services.AddScoped<IRepository<InvoiceItem>, Repository<InvoiceItem>>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();

            services.AddMvc().AddJsonOptions(ConfigureJson);
            services.AddCors();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


        }

        private void ConfigureJson(MvcJsonOptions obj)
        {
            obj.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(builder =>
                       builder.AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowAnyOrigin()
                              .AllowCredentials()
                       );

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
           
        }
    }
}
