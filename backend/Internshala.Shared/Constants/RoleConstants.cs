namespace Internshala.Shared.Constants;

public static class RoleConstants
{
    public const string Student = "Student";
    public const string Employer = "Employer";
    public const string Admin = "Admin";
    public const string StudentOrEmployer = $"{Student},{Employer}";
    public const string AllAuthenticated = $"{Student},{Employer},{Admin}";
}

public static class PolicyConstants
{
    public const string CanPostInternship = "CanPostInternship";
    public const string CanApply = "CanApply";
    public const string AdminOnly = "AdminOnly";
}

public static class CacheKeys
{
    public const string Categories = "categories:all";
    public const string Locations = "locations:all";
    public const string Skills = "skills:all";
    public static string InternshipById(int id) => $"internship:{id}";
}
