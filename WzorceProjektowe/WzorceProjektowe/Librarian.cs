// Represents a librarian user with specific permissions
public class Librarian : User
{
    public Librarian(string name) : base(name)
    {
        Role = "Librarian";
    }

    public override void DisplayPermissions()
    {
        System.Console.WriteLine($"{Name} ({Role}) can manage all books and user accounts.");
    }
}
