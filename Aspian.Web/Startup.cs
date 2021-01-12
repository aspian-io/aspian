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
using Infrastructure.Schedule;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using tusdotnet;
using tusdotnet.Models;
using tusdotnet.Models.Configuration;
using tusdotnet.Interfaces;
using Microsoft.Extensions.Options;
using tusdotnet.Stores;
using Aspian.Domain.SiteModel;
using Infrastructure.Upload.Tus.Stores;
using FluentFTP;
using Aspian.Domain.AttachmentModel;
using Microsoft.Data.SqlClient;

namespace Aspian.Web
{
    public class Startup
    {
        private string _connection = null;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Registered services for Scheduling functionalities
            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddSingleton<IScheduler, ScheduleTask>();

            var builder = new SqlConnectionStringBuilder(
            Configuration.GetConnectionString("AspianConnection"));
            builder.Password = Configuration["DbPassword"];
            _connection = builder.ConnectionString;

            // Adding data context
            services.AddDbContext<DataContext>(options =>
            {
                // Providing Lazy Loading service for Entity Framework models
                options.UseLazyLoadingProxies();
                // Using MSSQL Server driver and providing access to connection string
                options.UseSqlServer(_connection);
            });

            // CORS services and policies
            services.AddCors(OptionAccessor =>
            {
                OptionAccessor.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000").WithExposedHeaders(tusdotnet.Helpers.CorsHelper.GetExposedHeaders());
                });
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
                        ValidateIssuer = false,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    };

                });

            services.AddAuthorization(options =>
            {
                // AdminOnly Policy
                options.AddPolicy(AspianCorePolicy.AdminOnlyPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin));
                // Developer Policy
                options.AddPolicy(AspianCorePolicy.DeveloperOnlyPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Developer));
                // Admin Activity Policy
                options.AddPolicy(AspianCorePolicy.AdminActivityListPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminActivityListClaim));
                // Admin Attachment Policies
                options.AddPolicy(AspianCorePolicy.AdminAttachmentAddPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminAttachmentAddClaim));
                options.AddPolicy(AspianCorePolicy.AdminAttachmentDeletePolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminAttachmentDeleteClaim));
                options.AddPolicy(AspianCorePolicy.AdminAttachmentDownloadPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminAttachmentDownloadClaim));
                options.AddPolicy(AspianCorePolicy.AdminAttachmentGetImagePolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminAttachmentGetImageClaim));
                options.AddPolicy(AspianCorePolicy.AdminAttachmentListPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminAttachmentListClaim));
                options.AddPolicy(AspianCorePolicy.AdminAttachmentSettingsPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminAttachmentSettingsClaim));
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
                options.AddPolicy(AspianCorePolicy.AdminSiteEditPolicy, policy => policy.RequireClaim(AspianClaimType.Claim, AspianCoreClaimValue.Admin, AspianCoreClaimValue.AdminSiteListClaim));
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

            services.AddSingleton<ITusUploadUtils, TusUploadUtils>();
            services.AddScoped<IUploadAccessor, UploadAccessor>();
            services.AddScoped<IOptionAccessor, OptionAccessor>();
            services.Configure<FtpServerSettings>(Configuration.GetSection("FtpServer"));

            // Providing MediatR service for Aspian.Application.Core Assembly
            services.AddMediatR(typeof(List.Handler).Assembly);
            // Providing AutoMapper service for Aspian.Application.Core Assembly
            services.AddAutoMapper(typeof(List.Handler).Assembly);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, ILogger<Startup> logger, ITusUploadUtils tusUploadUtils, IOptions<FtpServerSettings> config)
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

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            // To use FTPServer as the storage you must use TusDiskStore instead of CustomTusDiskStore 
            // if you want to use localhost as the storage without config param
            var uploadLocation = UploadLocationEnum.LocalHost;
            app.UseTus(httpContext => new DefaultTusConfiguration
            {
                // Use TusDiskStore instead of CustomTusDiskStore if you want to use localhost as the storage without config param
                Store = new TusDiskStore(tusUploadUtils.GetStorePath("uploads", uploadLocation), true, new TusDiskBufferSize(1024 * 1024 * 5, 1024 * 1024 * 5)),
                // On what url should we listen for uploads?
                UrlPath = "/api/v1/attachments/add-private-media",
                Events = new Events
                {
                    OnAuthorizeAsync = eventContext =>
                    {
                        var refreshToken = httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

                        if (refreshToken == null)
                        {
                            eventContext.FailRequest(HttpStatusCode.Unauthorized);
                            return Task.CompletedTask;
                        }
                        // Do other verification on the user; claims, roles, etc. In this case, check the username.
                        // if (eventContext.HttpContext.User.Identity.Name != "test")
                        // {
                        //     eventContext.FailRequest(HttpStatusCode.Forbidden, "'test' is the only allowed user");
                        //     return Task.CompletedTask;
                        // }

                        // Verify different things depending on the intent of the request.
                        // E.g.:
                        //   Does the file about to be written belong to this user?
                        //   Is the current user allowed to create new files or have they reached their quota?
                        //   etc etc
                        switch (eventContext.Intent)
                        {
                            case IntentType.CreateFile:
                                break;
                            case IntentType.ConcatenateFiles:
                                break;
                            case IntentType.WriteFile:
                                break;
                            case IntentType.DeleteFile:
                                break;
                            case IntentType.GetFileInfo:
                                break;
                            case IntentType.GetOptions:
                                break;
                            default:
                                break;
                        }

                        return Task.CompletedTask;
                    },

                    OnBeforeCreateAsync = async ctx =>
                    {
                        var storePath = tusUploadUtils.GetStorePath("uploads", uploadLocation);
                        // If localhost is the storage
                        if (uploadLocation == UploadLocationEnum.LocalHost)
                        {
                            // Create temp directory if it does not exist...
                            Directory.CreateDirectory(storePath);
                        }

                        // If FTP server is the storage
                        if (uploadLocation == UploadLocationEnum.FtpServer)
                        {
                            // create an FTP client
                            FtpClient client = new FtpClient(config.Value.ServerUri, config.Value.ServerPort, config.Value.ServerUsername, config.Value.ServerPassword);
                            // Connecting to the server
                            await client.ConnectAsync();

                            // check if a folder doesn't exist
                            if (!await client.DirectoryExistsAsync(storePath))
                                await client.CreateDirectoryAsync(storePath);
                        }

                        if (!ctx.Metadata.ContainsKey("name"))
                        {
                            ctx.FailRequest("name metadata must be specified. ");
                        }

                        if (!ctx.Metadata.ContainsKey("type"))
                        {
                            ctx.FailRequest("filetype metadata must be specified. ");
                        }

                        if (!await tusUploadUtils.IsFileTypeAllowed(ctx.Metadata["type"].GetString(Encoding.UTF8), SiteTypeEnum.Blog))
                        {
                            ctx.FailRequest("Filetype is not allowed!");
                        }
                    },

                    OnFileCompleteAsync = async eventContext =>
                    {
                        ITusFile file = await eventContext.GetFileAsync();

                        var refreshToken = httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

                        // Saving file information into database
                        await tusUploadUtils.SaveTusFileInfoAsync(file, SiteTypeEnum.Blog, refreshToken, uploadLocation, UploadLinkAccessibilityEnum.Private, eventContext.CancellationToken);

                        // If FTP server is the storage
                        if (uploadLocation == UploadLocationEnum.FtpServer)
                        {
                            // create an FTP client
                            FtpClient client = new FtpClient(config.Value.ServerUri, config.Value.ServerPort, config.Value.ServerUsername, config.Value.ServerPassword);
                            // Connecting to the server
                            await client.DisconnectAsync();
                        }

                    },
                }
            });

            app.UseTus(httpContext => new DefaultTusConfiguration
            {
                // Use TusDiskStore instead of CustomTusDiskStore if you want to use localhost as the storage without config param
                Store = new TusDiskStore(
                    tusUploadUtils.GetStorePath("uploads",
                    uploadLocation, UploadLinkAccessibilityEnum.Private),
                    true,
                    new TusDiskBufferSize(1024 * 1024 * 5, 1024 * 1024 * 5)),

                // On what url should we listen for uploads?
                UrlPath = "/api/v1/attachments/add-public-media",
                Events = new Events
                {
                    OnAuthorizeAsync = eventContext =>
                    {
                        var refreshToken = httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

                        if (refreshToken == null)
                        {
                            eventContext.FailRequest(HttpStatusCode.Unauthorized);
                            return Task.CompletedTask;
                        }
                        // Do other verification on the user; claims, roles, etc. In this case, check the username.
                        // if (eventContext.HttpContext.User.Identity.Name != "test")
                        // {
                        //     eventContext.FailRequest(HttpStatusCode.Forbidden, "'test' is the only allowed user");
                        //     return Task.CompletedTask;
                        // }

                        // Verify different things depending on the intent of the request.
                        // E.g.:
                        //   Does the file about to be written belong to this user?
                        //   Is the current user allowed to create new files or have they reached their quota?
                        //   etc etc
                        switch (eventContext.Intent)
                        {
                            case IntentType.CreateFile:
                                break;
                            case IntentType.ConcatenateFiles:
                                break;
                            case IntentType.WriteFile:
                                break;
                            case IntentType.DeleteFile:
                                break;
                            case IntentType.GetFileInfo:
                                break;
                            case IntentType.GetOptions:
                                break;
                            default:
                                break;
                        }

                        return Task.CompletedTask;
                    },

                    OnBeforeCreateAsync = async ctx =>
                    {
                        var storePath = tusUploadUtils.GetStorePath(
                            "uploads",
                            uploadLocation,
                            UploadLinkAccessibilityEnum.Private);
                        // If localhost is the storage
                        if (uploadLocation == UploadLocationEnum.LocalHost)
                        {
                            // Create temp directory if it does not exist...
                            Directory.CreateDirectory(storePath);
                        }

                        // If FTP server is the storage
                        if (uploadLocation == UploadLocationEnum.FtpServer)
                        {
                            // create an FTP client
                            FtpClient client = new FtpClient(config.Value.ServerUri, config.Value.ServerPort, config.Value.ServerUsername, config.Value.ServerPassword);
                            // Connecting to the server
                            await client.ConnectAsync();

                            // check if a folder doesn't exist
                            if (!await client.DirectoryExistsAsync(storePath))
                                await client.CreateDirectoryAsync(storePath);
                        }

                        if (!ctx.Metadata.ContainsKey("name"))
                        {
                            ctx.FailRequest("name metadata must be specified. ");
                        }

                        if (!ctx.Metadata.ContainsKey("type"))
                        {
                            ctx.FailRequest("filetype metadata must be specified. ");
                        }

                        if (!await tusUploadUtils.IsFileTypeAllowed(ctx.Metadata["type"].GetString(Encoding.UTF8), SiteTypeEnum.Blog))
                        {
                            ctx.FailRequest("Filetype is not allowed!");
                        }
                    },

                    OnFileCompleteAsync = async eventContext =>
                    {
                        ITusFile file = await eventContext.GetFileAsync();

                        var refreshToken = httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

                        // Saving file information into database
                        await tusUploadUtils.SaveTusFileInfoAsync(file, SiteTypeEnum.Blog, refreshToken, uploadLocation, UploadLinkAccessibilityEnum.Public, eventContext.CancellationToken);

                        // If FTP server is the storage
                        if (uploadLocation == UploadLocationEnum.FtpServer)
                        {
                            // create an FTP client
                            FtpClient client = new FtpClient(config.Value.ServerUri, config.Value.ServerPort, config.Value.ServerUsername, config.Value.ServerPassword);
                            // Connecting to the server
                            await client.DisconnectAsync();
                        }

                    },
                }
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
