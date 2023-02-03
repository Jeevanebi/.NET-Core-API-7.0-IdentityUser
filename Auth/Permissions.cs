namespace WebService.API.Authorization
{
    public static class Permissions
    {
        public static class Users
        {
            //Only For SuperAdmins
            public const string SuperAdminView = "Permissions.Users.SuperAdminView";
            public const string SuperAdminCreate = "Permissions.Users.SuperAdminCreate";

            //All
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";

            //Guest or Clients
            public const string viewById = "Permissions.Users.viewById";

        }
    }
}
