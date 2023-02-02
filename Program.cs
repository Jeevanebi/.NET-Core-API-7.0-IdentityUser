using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebService.API.Auth;
using WebService.API.Auth.Controllers;
using WebService.API.Authorization;
using WebService.API.Data;
using WebService.API.Properties;
using WebService.API.Repository;
using WebService.API.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

        // Add services to the container.
        //Resgitering AutoMapper
        builder.Services.AddAutoMapper(typeof(Program));

        ////Normal User DbContext For Testing(SQL SERVER)
        //builder.Services.AddDbContext<ApplicationDbContext>();
        //Identity User DbContext for Production(SQL SERVER)


        builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        //builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        builder.Services.AddDbContext<IdentityUserContext>();

        //Registering Identity 
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequiredLength = 8;

        }).AddEntityFrameworkStores<IdentityUserContext>()
        .AddDefaultTokenProviders();

        //Registering Mail Service
        builder.Services.Configure<MailSettings>(_config.GetSection("MailSettings"));

        //Registering Interface
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddTransient<IMailService, MailService>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        //Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = ".NET-Core-API-7.0-BETA-",
                Version = "v1"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
            });
        });

        //Authentication
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(option =>
        {
            option.SaveToken = true;
            option.TokenValidationParameters = new TokenValidationParameters
            {
                SaveSigninToken = true,
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["Jwt:Issuer"],       // Jwt:Issuer - config value 
                ValidAudience = _config["Jwt:Issuer"],     // Jwt:Issuer - config value 
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"])) // Jwt:Key - config value 
            };
        });

        //Authorization
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(Permissions.Users.View, builder =>
            {
                builder.AddRequirements(new PermissionRequirement(Permissions.Users.View));
            });

            options.AddPolicy(Permissions.Users.Create, builder =>
            {
                builder.AddRequirements(new PermissionRequirement(Permissions.Users.Create));
            });

        });


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}