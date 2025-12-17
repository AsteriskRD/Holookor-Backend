namespace HolookorBackend.Core.Domain.Enums
{
    public static class DefaultRoles
    {
        public const string Admin = "Admin";
        public const string Tutor = "Tutor";
        public const string Student = "Student";
        public const string Parent = "Parent";

        public static IEnumerable<string> AllRoles => new[]
        {
            Admin,
            Tutor,
            Student,
            Parent
        };
    }
}
