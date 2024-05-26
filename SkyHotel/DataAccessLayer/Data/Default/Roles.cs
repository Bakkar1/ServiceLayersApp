
//using Microsoft.AspNetCore.Mvc.Rendering;

namespace DataAccessLayer.Data.Default
{
    public class Roles
    {
        public const string Admin = "Admin";
        public const string StoreOwner = "StoreOwner";
        public const string Employee = "Employee";
        public const string Client = "Client";
        public const string NormalUser = "User";
        public const string Supplier = "Supplier";

        public const string AdminOwner = $"{Admin}, {StoreOwner}";
        public const string AdminOwnerEmployee = $"{Admin}, {StoreOwner}, {Employee}";

        //public static SelectList GetRoles()
        //{
        //    List<string> roles = new()
        //    {
        //        NormalUser,
        //        Admin,
        //        StoreOwner,
        //        Employee,
        //        NormalUser,
        //        Supplier
        //    };
        //    return new SelectList(roles);
        //}
    }
}
