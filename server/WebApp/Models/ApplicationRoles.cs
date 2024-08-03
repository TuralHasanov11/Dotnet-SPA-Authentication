namespace WebApp.Models;

public static class ApplicationRoles
{
    public const string Administrator = "Administrator";
    public const string Visitor = "Visitor";

    public static IEnumerable<string> All()
    {
        yield return Administrator;
        yield return Visitor;
    }
}