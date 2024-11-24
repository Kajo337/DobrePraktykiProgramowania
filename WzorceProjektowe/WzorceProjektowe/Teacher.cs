// Represents a teacher user with specific permissions
public class Teacher : User
{
    public Teacher(string name) : base(name)
    {
        Role = "Teacher";
    }

    public override void DisplayPermissions()
    {
        System.Console.WriteLine($"{Name} ({Role}) can borrow up to 10 books.");
    }
}
