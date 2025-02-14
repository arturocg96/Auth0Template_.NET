namespace AuthTemplate.Configuration
{
    public class PolicySettings
    {
        public static string? RolesClaimType { get; set; }
        public const string PermissionsClaimType = "permissions";
        public static readonly string[] Roles = { "Root", "Administrator", "User" };
        public static readonly string[] Permissions = {
            "read:any_user", "write:any_user", "delete:any_user",
            "manage:roles", "manage:settings", "access:admin_panel",
            "manage:organizations", "view:metrics", "manage:api_keys",
            "read:assigned_users", "write:assigned_users", "manage:content",
            "read:own_profile", "write:own_profile", "access:basic_features",
            "assign:roles", "delete:events", "delete:logs",
            "delete:roles", "delete:users", "manage:all",
            "read:events", "read:logs", "read:roles",
            "read:settings", "read:users", "write:events",
            "write:assigned_user", "write:settings", "write:users",
            "write:roles"
        };
    }
}