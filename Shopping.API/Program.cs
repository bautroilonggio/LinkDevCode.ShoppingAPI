using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Serilog;
using Shopping.API.BusinessLogic.Services;
using Shopping.API.Commons;
using Shopping.API.DataAccess;
using Shopping.API.DataAccess.DbContexts;
using System.Text;

// Cau hinh trinh ghi log su dung Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/shopping.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Kich hoat trinh ghi log su dung Serilog
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddXmlDataContractSerializerFormatters()
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
});


builder.Services.AddDbContext<ShoppingContext>(
    dbConnection => dbConnection.UseSqlServer(
        builder.Configuration["ConnectionStrings:ShoppingDBConnectionString"]));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();



//FirebaseApp.Create(new AppOptions
//{
//    Credential = GoogleCredential.FromFile(@"D:\WorkSpace\FPT\Traning\Project\Shopping\LinkDevCode.Shopping\Shopping.API\linkdevcodeshoppingapi-firebase-adminsdk-9ho0t-eb4df4e2e7.json")
//});


builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer(options =>
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
    });

//builder.Services
//    .AddAuthentication(options =>
//    {
//        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    })
//    .AddJwtBearer("Firebase", options =>
//    {
//        options.Authority = builder.Configuration["Authentication:Firebase:Issuer"];
//        options.TokenValidationParameters = new()
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Authentication:Firebase:Issuer"],
//            ValidAudience = builder.Configuration["Authentication:Firebase:Audience"],
//        };
//    });


//builder.Services
//    .AddAuthentication(options =>
//    {
//        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//    })
//    .AddJwtBearer("GoogleAPI", options =>
//    {
//        options.SaveToken = true;
//        options.TokenValidationParameters = new()
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Authentication:Issuer"],
//            ValidAudience = builder.Configuration["Authentication:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(
//                Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
//        };
//    })
//    .AddCookie()
//    .AddGoogle(options =>
//    {
//        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
//        options.SaveTokens = true;
//    });


//builder.Services.AddAuthorization(opt =>
//{
//    opt.DefaultPolicy = new AuthorizationPolicyBuilder()
//    .AddAuthenticationSchemes("Project", "Firebase")
//    .RequireAuthenticatedUser()
//    .Build();
//});


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
