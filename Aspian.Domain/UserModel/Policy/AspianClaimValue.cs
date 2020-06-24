using System.Collections.Generic;

namespace Aspian.Domain.UserModel.Policy
{
    public class AspianClaimValue
    {
        public const string Admin = "Admin";
        public const string Member = "Member";

        public static string AdminOnly()
        {
            return Admin;
        }

        public static List<string> SpecificValues()
        {
            return new List<string> {
                Admin,
                Member
            };
        }
    }
}