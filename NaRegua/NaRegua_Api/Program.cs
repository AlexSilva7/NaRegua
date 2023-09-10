using Google.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NaRegua_Api.Common.Dependency;
using NaRegua_Api.Configurations;
using NaRegua_Api.Controllers.V1.Criptograph;
using NaRegua_Api.Providers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddLog4Net("log4net.config");

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
// Add services to the container.
builder.Services.AddRazorPages();

AppSettings.SetConfig(builder.Configuration);
DependencyResolver.SetDependency(builder.Services);

var key = Encoding.ASCII.GetBytes(AppSettings.JwtKey);

builder.Services.AddHostedService<MyBackgroundService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseMiddleware<EncryptionMiddleware>();
app.UseRouting();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=User}");
});

app.MapRazorPages();
app.Run();
