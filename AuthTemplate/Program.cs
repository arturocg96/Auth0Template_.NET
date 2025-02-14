using Auth.API.Middleware;
using AuthTemplate.Configuration;
using AuthTemplate.Extensions;


var builder = WebApplication.CreateBuilder(args);

var domain = builder.Configuration["Auth0:Domain"];
var audience = builder.Configuration["Auth0:Audience"];
PolicySettings.RolesClaimType = builder.Configuration["Auth0:RolesClaimType"];

if (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(audience))
{
    throw new Exception("ERROR: Auth0 Domain o Audience no están configurados en appsettings.json");
}

var issuer = $"https://{domain}/";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger();
builder.Services.AddCustomCors();
builder.Services.AddCustomAuthentication(issuer, audience);
builder.Services.AddCustomAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();