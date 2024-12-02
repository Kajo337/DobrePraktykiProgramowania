// Base class for all user types
public abstract class User
{
    public string Name { get; private set; }
    public string Role { get; protected set; }

    protected User(string name)
    {
        Name = name;
    }

    // Method to display user permissions
    public abstract void DisplayPermissions();
}
