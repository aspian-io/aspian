using System;
using System.Text;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.TaxonomyServices.AdminServices;
using Aspian.Domain.UserModel;
using Aspian.Persistence;
using Aspian.Web.Middleware.API;
using AutoMapper;
using FluentValidation.AspNetCore;
using Infrastructure.Upload;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Option;
using Aspian.Domain.UserModel.Policy;
using Infrastructure.Utility;
using Infrastructure.Logger;

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
                options.AddPolicy(AspianCorePolicy.AdminOnlyPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin));
                // Admin Activity Policy
                options.AddPolicy(AspianCorePolicy.AdminActivityListPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminActivityListClaim));
                // Admin Attachment Policies
                options.AddPolicy(AspianCorePolicy.AdminAttachmentAddPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminAttachmentAddClaim));
                options.AddPolicy(AspianCorePolicy.AdminAttachmentDeletePolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminAttachmentDeleteClaim));
                options.AddPolicy(AspianCorePolicy.AdminAttachmentDownloadPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminAttachmentDownloadClaim));
                options.AddPolicy(AspianCorePolicy.AdminAttachmentGetImagePolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminAttachmentGetImageClaim));
                options.AddPolicy(AspianCorePolicy.AdminAttachmentListPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminAttachmentListClaim));
                // Admin Comment Policies
                options.AddPolicy(AspianCorePolicy.AdminCommentApprovePolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminCommentApproveClaim));
                options.AddPolicy(AspianCorePolicy.AdminCommentCreatePolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminCommentCreateClaim));
                options.AddPolicy(AspianCorePolicy.AdminCommentDeletePolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminCommentDeleteClaim));
                options.AddPolicy(AspianCorePolicy.AdminCommentDetailsPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminCommentDetailsClaim));
                options.AddPolicy(AspianCorePolicy.AdminCommentEditPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminCommentEditClaim));
                options.AddPolicy(AspianCorePolicy.AdminCommentListPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminCommentListClaim));
                options.AddPolicy(AspianCorePolicy.AdminCommentUnapprovePolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminCommentUnapproveClaim));
                // Admin Option Policies
                options.AddPolicy(AspianCorePolicy.AdminOptionEditPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminOptionEditClaim));
                options.AddPolicy(AspianCorePolicy.AdminOptionListPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminOptionListClaim));
                options.AddPolicy(AspianCorePolicy.AdminOptionRestoreDefaultPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminOptionRestoreDefaultClaim));
                // Admin Post policies
                options.AddPolicy(AspianCorePolicy.AdminPostCreatePolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminPostCreateClaim));
                options.AddPolicy(AspianCorePolicy.AdminPostDeletePolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminPostDeleteClaim));
                options.AddPolicy(AspianCorePolicy.AdminPostDetailsPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminPostDetailsClaim));
                options.AddPolicy(AspianCorePolicy.AdminPostEditPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminPostEditClaim));
                options.AddPolicy(AspianCorePolicy.AdminPostListPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminPostListClaim));
                // Admin Site Policies
                options.AddPolicy(AspianCorePolicy.AdminSiteDetailsPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminSiteDetailsClaim));
                options.AddPolicy(AspianCorePolicy.AdminSiteListPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminSiteListClaim));
                // Admin Taxonomy Policies
                options.AddPolicy(AspianCorePolicy.AdminTaxonomyCreatePolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminTaxonomyCreateClaim));
                options.AddPolicy(AspianCorePolicy.AdminTaxonomyDeletePolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminTaxonomyDeleteClaim));
                options.AddPolicy(AspianCorePolicy.AdminTaxonomyDetailsPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminTaxonomyDetailsClaim));
                options.AddPolicy(AspianCorePolicy.AdminTaxonomyEditPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminTaxonomyEditClaim));
                options.AddPolicy(AspianCorePolicy.AdminTaxonomyListPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminTaxonomyListClaim));
                // Admin User Policies
                options.AddPolicy(AspianCorePolicy.AdminUserCurrentPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminUserCurrentClaim));
                options.AddPolicy(AspianCorePolicy.AdminUserLockoutPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminUserLockoutClaim));
                options.AddPolicy(AspianCorePolicy.AdminUserUnlockPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminUserUnlockClaim));
            });

            // Providing our JWT Generator services 
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IActivityLogger, ActivityLogger>();
            services.AddScoped<ISlugGenerator, SlugGenerator>();

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
