using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ Verifica que los valores de Auth0 estén configurados
var domain = builder.Configuration["Auth0:Domain"];
var audience = builder.Configuration["Auth0:Audience"];

if (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(audience))
{
    throw new Exception("🚨 ERROR: Auth0 Domain o Audience no están configurados en appsettings.json");
}

var issuer = $"https://{domain}/";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });

    // 💡 Habilitar autenticación con Bearer Token en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Introduce el token de Auth0 con el formato 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// ✅ CORS: Permitir peticiones desde Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")  // Cambia esto si tu Angular está en otro puerto
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ✅ Configuración de autenticación con Auth0
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = issuer;
        options.Audience = audience;
        options.RequireHttpsMetadata = false;  // ❗ IMPORTANTE para desarrollo local, en producción cambia a true
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var user = context.Principal;

                // 🛠️ Verifica que el usuario tenga los roles definidos en el token
                var rolesClaim = user?.FindFirst("https://roles0auth.com/roles");
                if (rolesClaim == null)
                {
                    Console.WriteLine("🚨 No se encontraron roles en el token");
                    context.Fail("No roles assigned.");
                }

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"🚨 Error de autenticación: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };
    });

// ✅ Configuración de Autorización basada en **Roles y Permisos**
builder.Services.AddAuthorization(options =>
{
    // 🛡️ Políticas por ROL (basado en la Claim de roles)
    options.AddPolicy("Root", policy => policy.RequireClaim("https://roles0auth.com/roles", "Root"));
    options.AddPolicy("Administrator", policy => policy.RequireClaim("https://roles0auth.com/roles", "Administrator"));
    options.AddPolicy("User", policy => policy.RequireClaim("https://roles0auth.com/roles", "User"));

    // ✅ Políticas por PERMISOS específicos
    options.AddPolicy("read:any_user", policy => policy.RequireClaim("permissions", "read:any_user"));
    options.AddPolicy("write:any_user", policy => policy.RequireClaim("permissions", "write:any_user"));
    options.AddPolicy("delete:any_user", policy => policy.RequireClaim("permissions", "delete:any_user"));
    options.AddPolicy("manage:roles", policy => policy.RequireClaim("permissions", "manage:roles"));
    options.AddPolicy("manage:settings", policy => policy.RequireClaim("permissions", "manage:settings"));
    options.AddPolicy("access:admin_panel", policy => policy.RequireClaim("permissions", "access:admin_panel"));
    options.AddPolicy("manage:organizations", policy => policy.RequireClaim("permissions", "manage:organizations"));
    options.AddPolicy("view:metrics", policy => policy.RequireClaim("permissions", "view:metrics"));
    options.AddPolicy("manage:api_keys", policy => policy.RequireClaim("permissions", "manage:api_keys"));
    options.AddPolicy("read:assigned_users", policy => policy.RequireClaim("permissions", "read:assigned_users"));
    options.AddPolicy("write:assigned_users", policy => policy.RequireClaim("permissions", "write:assigned_users"));
    options.AddPolicy("manage:content", policy => policy.RequireClaim("permissions", "manage:content"));
    options.AddPolicy("read:own_profile", policy => policy.RequireClaim("permissions", "read:own_profile"));
    options.AddPolicy("write:own_profile", policy => policy.RequireClaim("permissions", "write:own_profile"));
    options.AddPolicy("access:basic_features", policy => policy.RequireClaim("permissions", "access:basic_features"));
    options.AddPolicy("assign:roles", policy => policy.RequireClaim("permissions", "assign:roles"));
    options.AddPolicy("delete:events", policy => policy.RequireClaim("permissions", "delete:events"));
    options.AddPolicy("delete:logs", policy => policy.RequireClaim("permissions", "delete:logs"));
    options.AddPolicy("delete:roles", policy => policy.RequireClaim("permissions", "delete:roles"));
    options.AddPolicy("delete:users", policy => policy.RequireClaim("permissions", "delete:users"));
    options.AddPolicy("manage:all", policy => policy.RequireClaim("permissions", "manage:all"));
    options.AddPolicy("read:events", policy => policy.RequireClaim("permissions", "read:events"));
    options.AddPolicy("read:logs", policy => policy.RequireClaim("permissions", "read:logs"));
    options.AddPolicy("read:roles", policy => policy.RequireClaim("permissions", "read:roles"));
    options.AddPolicy("read:settings", policy => policy.RequireClaim("permissions", "read:settings"));
    options.AddPolicy("read:users", policy => policy.RequireClaim("permissions", "read:users"));
    options.AddPolicy("write:events", policy => policy.RequireClaim("permissions", "write:events"));
    options.AddPolicy("write:assigned_user", policy => policy.RequireClaim("permissions", "write:assigned_user"));
    options.AddPolicy("write:settings", policy => policy.RequireClaim("permissions", "write:settings"));
    options.AddPolicy("write:users", policy => policy.RequireClaim("permissions", "write:users"));
    options.AddPolicy("write:roles", policy => policy.RequireClaim("permissions", "write:roles"));

});

var app = builder.Build();

// ✅ Mostrar errores en Desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ✅ Middleware
app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ✅ Manejo de excepciones global
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"💥 ERROR CRÍTICO: {ex.Message}");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("Error interno del servidor.");
    }
});

// 🚀 ¡Ejecuta la aplicación!
app.Run();
