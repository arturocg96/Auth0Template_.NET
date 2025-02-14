namespace AuthTemplate.Constants
{
    public class Permissions
    {
        // Root permissions
        public const string ReadAnyUser = "read:any_user";
        public const string WriteAnyUser = "write:any_user";
        public const string DeleteAnyUser = "delete:any_user";
        public const string ManageRoles = "manage:roles";
        public const string ManageSettings = "manage:settings";
        public const string AccessAdminPanel = "access:admin_panel";
        public const string ManageOrganizations = "manage:organizations";
        public const string ViewMetrics = "view:metrics";
        public const string ManageApiKeys = "manage:api_keys";

        // Admin permissions
        public const string ReadAssignedUsers = "read:assigned_users";
        public const string WriteAssignedUsers = "write:assigned_users";
        public const string ManageContent = "manage:content";

        // User permissions
        public const string ReadOwnProfile = "read:own_profile";
        public const string WriteOwnProfile = "write:own_profile";
        public const string AccessBasicFeatures = "access:basic_features";
    }
}
