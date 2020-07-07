using System;
using System.Text;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.TaxonomyServices;
using Aspian.Domain.UserModel;
using Aspian.Persistence;
using Aspian.Web.Middleware.API;
using AutoMapper;
using FluentValidation.AspNetCore;
using Infrastructure.Upload;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Option;
using Aspian.Domain.UserModel.Policy;
using Infrastructure.Utility;

namespace Aspian.Web
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
            services.AddDbContext<DataContext>(options =>
            {
                // Providing Lazy Loading service for Entity Framework models
                options.UseLazyLoadingProxies();
                // Using MSSQL Server driver and providing access to connection string
                options.UseSqlServer(Configuration.GetConnectionString("AspianConnection"));
            });

            // Providing full mvc ASP.NET Core framework
            services.AddControllersWithViews(opt =>
                   {
                       var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                       opt.Filters.Add(new AuthorizeFilter(policy));
                   })
                    // providing FluentValidation service for Aspian.Application.Core Assembly
                    .AddFluentValidation(cfg =>
                    {
                        cfg.RegisterValidatorsFromAssemblyContaining<Create>();
                    });

            // Identity services
            services
                .AddDefaultIdentity<User>()
                .AddEntityFrameworkStores<DataContext>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"]));
            services.AddAuthentication()
                // .AddCookie(options =>
                // {
                //     options.LoginPath = "/Account/Unauthorized/";
                //     options.AccessDeniedPath = "/Account/Forbidden/";
                // })
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                });

            services.AddAuthorization(options =>
            {
                // AdminOnly Policy
                options.AddPolicy(AspianPolicy.AdminOnly, policy => policy.RequireClaim(AspianClaimType.Role, AspianClaimValue.AdminOnly()));
            });

            // Providing our JWT Generator services 
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddSingleton<ISlugGenerator, SlugGenerator>();

            services.AddScoped<IUploadAccessor, UploadAccessor>();
            services.AddScoped<IOptionAccessor, OptionAccessor>();
            services.Configure<FtpServerSettings>(Configuration.GetSection("FtpServer"));

            // Providing MediatR service for Aspian.Application.Core Assembly
            services.AddMediatR(typeof(List.Handler).Assembly);
            // Providing AutoMapper service for Aspian.Application.Core Assembly
            services.AddAutoMapper(typeof(List.Handler).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseWhen(ctx => ctx.Request.Path.StartsWithSegments("/api", StringComparison.InvariantCultureIgnoreCase),
                    appBuilder =>
                    {
                        // Custom API Error handler
                        appBuilder.UseMiddleware<APIErrorHandlingMiddleware>();
                    }
                );

                app.UseWhen(ctx => !ctx.Request.Path.StartsWithSegments("/api", StringComparison.InvariantCultureIgnoreCase),
                    appBuilder =>
                    {
                        // Default mvc error handler
                        appBuilder.UseDeveloperExceptionPage();
                    }
                );
            }
            else
            {
                app.UseWhen(ctx => ctx.Request.Path.StartsWithSegments("/api", StringComparison.InvariantCultureIgnoreCase),
                    appBuilder =>
                    {
                        // Custom API Error handler
                        appBuilder.UseMiddleware<APIErrorHandlingMiddleware>();
                    }
                );

                app.UseWhen(ctx => !ctx.Request.Path.StartsWithSegments("/api", StringComparison.InvariantCultureIgnoreCase),
                    appBuilder =>
                    {
                        // Default mvc error handler
                        appBuilder.UseExceptionHandler("/Home/Error");
                    }
                );
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
