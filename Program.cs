using System.Text;
using CoroDr.IdentityAPI.LoginProvider;
using CoroDr.IdentityAPI.Models;
using CoroDr.IdentityAPI.Repository;
using CoroDr.IdentityAPI.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContexts(builder.Configuration);

//builder.Services.Configure<JWTModel>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<IJWTManageRespositoryInterface, JWTManageRespository>();
builder.Services.AddScoped<IPasswordHasherRepositoryInterface, PasswordHashing>();
builder.Services.AddScoped<IUserRepositoryInterface, UserRepository>();
builder.Services.AddScoped<IGoogleInterface, GoogleLogin>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "",
        ValidAudience = "",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("myKeyToHash")),

    };
}).AddGoogle(options =>
{
    options.ClientId = configuration["GoogleAuthSettings:clientId"];
    options.ClientSecret = configuration["GoogleAuthSettings:clientSecret"];
    options.Scope.Add("Profile");
    options.SignInScheme = Microsoft.AspNetCore.Identity.IdentityConstants.ExternalScheme;

});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();

