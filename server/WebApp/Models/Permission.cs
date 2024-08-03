namespace WebApp.Models;

public sealed class Permission
{
    public int Id { get; }

    public string Name { get; }

    public byte[]? RowVersion { get; }

    private Permission(string name)
    {
        Name = name;
    }

    private Permission()
    { }

    public static Permission Create(string name)
    {
        return new Permission(name);
    }
}