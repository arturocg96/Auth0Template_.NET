using AuthTemplate.Configuration;

namespace AuthTemplate.Extensions
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Políticas por ROL
                foreach (var role in PolicySettings.Roles)
                {
                    options.AddPolicy(role, policy =>
                        policy.RequireClaim(PolicySettings.RolesClaimType, role));
                }

                // Políticas por PERMISOS
                foreach (var permission in PolicySettings.Permissions)
                {
                    options.AddPolicy(permission, policy =>
                        policy.RequireClaim(PolicySettings.PermissionsClaimType, permission));
                }
            });

            return services;
        }
    }
}
