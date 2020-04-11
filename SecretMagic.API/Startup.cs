using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SecretMagic.Model;
using SecretMagic.Repository;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using SecretMagic.API.Services;
using Microsoft.AspNetCore.Authorization;
using SecretMagic.API.Authorization;

namespace SecretMagic.API
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
            services.AddCors(options =>
            {
                options.AddPolicy("UIService",
                builder =>
                {
                    builder.WithOrigins("http://localhost:8080", "https://www.secretmagic.tech", "http://118.178.93.119")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v0.1.0",
                    Title = "Blog.Core API",
                    Description = "API description",
                    TermsOfService = new Uri("https://www.secretmagic.club"),
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "SecretMagic",
                        Email = "ivenlau@qq.com",
                        Url = new Uri("https://www.secretmagic.club")
                    }
                });

                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                c.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Please input Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContextPool<DataContext>(options => options
                // replace with your connection string
                .UseMySql(Configuration.GetConnectionString("MySqlDatabase"),
                mySqlOptions => mySqlOptions
                    // replace with your Server Version and Type
                    .ServerVersion(new Version(8, 0, 16), ServerType.MySql)
                    .MigrationsAssembly(migrationsAssembly)
            ));

            services.AddSingleton<IAuthorizationPolicyProvider, ProtectedPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, ProtectedAuthorizationHandler>();
            services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
            services.AddScoped<Services.IAuthorizationService, AuthorizationService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IUrmRepository, UrmRepository>();
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISettingRepository, SettingRepository>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IOssService, OssService>();
            services.AddScoped<ISiteSettingService, SiteSettingService>();

            services.Configure<TokenConfig>(Configuration.GetSection("TokenConfig"));
            services.Configure<OssConfig>(Configuration.GetSection("OssConfig"));
            var token = Configuration.GetSection("TokenConfig").Get<TokenConfig>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    ValidIssuer = token.Issuer,
                    ValidAudience = token.Audience,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // services.AddAuthentication("Bearer")
            // .AddJwtBearer("Bearer", options =>
            // {
            //     options.Authority = "https://localhost:5554";
            //     options.RequireHttpsMetadata = false;

            //     options.Audience = "api1";
            //     options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
            //     {
            //         OnAuthenticationFailed = context =>
            //         {
            //             if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            //             {
            //                 context.Response.Headers.Add("Token-Expired", "true");
            //             }
            //             return Task.CompletedTask;
            //         }
            //     };
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("UIService");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                c.RoutePrefix = "";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
