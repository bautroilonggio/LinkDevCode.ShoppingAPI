using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddOcelot();


builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions
{
    //Credential = GoogleCredential.FromFile(@"D:\WorkSpace\FPT\Traning\Project\Shopping\LinkDevCode.Shopping\Shopping.API\linkdevcodeshoppingapi-firebase-adminsdk-9ho0t-eb4df4e2e7.json")
    Credential = GoogleCredential.FromJson(builder.Configuration["Authentication:Firebase:Config"])
}));

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
        options.RequireHttpsMetadata = false;
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

app.UseOcelot().Wait();

app.UseAuthentication();

app.UseAuthorization();

app.Run();
