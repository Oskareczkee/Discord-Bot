using DB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Web_UI.Models.Discord;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();
//Add session services
builder.Services.AddMemoryCache();
builder.Services.AddSession();

//Add authentication for discord
builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = "Discord";
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
   .AddCookie(options =>
   {
       options.ExpireTimeSpan = TimeSpan.FromDays(1);
       options.LoginPath = new PathString("/Authentication/Login");
       options.LogoutPath = new PathString("/Authentication/Logout");
   })
   .AddJwtBearer(options => {
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = Configuration.GetValue<string>("Jwt:Issuer"),
           ValidAudience = Configuration.GetValue<string>("Jwt:Audience"),
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Jwt:EncryptionKey")))
       };
   }
   ).AddOAuth("Discord",
       options => {
           options.AuthorizationEndpoint = "https://discord.com/oauth2/authorize";
           options.Scope.Add("identify email guilds guilds.members.read");

           options.CallbackPath = new PathString("/Home/Index");
           options.ClientId = Configuration.GetValue<string>("Discord:ClientID");
           options.ClientSecret = Configuration.GetValue<string>("Discord:ClientSecret");
           options.TokenEndpoint = "https://discord.com/api/oauth2/token";
           options.UserInformationEndpoint = "https://discord.com/api/users/@me";

           options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
           options.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
           options.SaveTokens = true;

           options.AccessDeniedPath = "/Authentication/AuthFailed";

           options.Events = new OAuthEvents
           {
               OnCreatingTicket = async context =>
               {
                   var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                   request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                   request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                   var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                   response.EnsureSuccessStatusCode();

                   var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
                   context.RunClaimActions(user);
               }
           };
       });

//Add Database
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseContext")));

builder.Services.AddTransient<IDiscordAPIService, DiscordAPIService>();

//allows for http context injection
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "database",
    pattern: "{controller}/{action}/activeType-{activeType}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/{redirectAction?}/{redirectController?}");

app.Run();
