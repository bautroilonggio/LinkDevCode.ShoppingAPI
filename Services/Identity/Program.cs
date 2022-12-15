using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Identity;
using Identity.BusinessLogic.Services;
using Identity.Commons;
using Identity.DataAccess;
using Identity.DataAccess.DbContexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date",
        Example = new OpenApiString("2022-01-01")
    });

    // Lay ten file
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // Lay duong dan file
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
    options.IncludeXmlComments(xmlCommentsFullPath);

    options.AddSecurityDefinition("ShoppingApiBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid token to access this API"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ShoppingApiBearerAuth" }
            }, new List<string>()
        }
    });
});

builder.Services.AddDbContext<IdentityContext>(
    dbConnection => dbConnection.UseSqlServer(
        builder.Configuration["ConnectionStrings:ShoppingDBConnectionString"]));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IAccountService, AccountService>();


builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions
{
    //Credential = GoogleCredential.FromFile(@"D:\WorkSpace\FPT\Traning\Project\Shopping\LinkDevCode.Shopping\Shopping.API\linkdevcodeshoppingapi-firebase-adminsdk-9ho0t-eb4df4e2e7.json")
    Credential = GoogleCredential.FromJson(builder.Configuration["Authentication:Firebase:Config"])
}));

//builder.Services.AddCustomJwtAuthentication();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddJwtBearer("Project", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Project:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Project:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["Authentication:Project:SecretForKey"]))
        };
        options.SaveToken = true;
    })
    .AddJwtBearer("Firebase", options =>
    {
        options.Authority = builder.Configuration["Authentication:Firebase:Issuer"];
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Authentication:Firebase:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Firebase:Audience"],
        };
        options.SaveToken = true;
    })
    .AddGoogle("Google", options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        options.SaveTokens = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                                .AddAuthenticationSchemes("Project", "Firebase", "Google")
                                .RequireAuthenticatedUser()
                                .Build();
});


var app = builder.Build();

app.UseHttpLogging();

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
