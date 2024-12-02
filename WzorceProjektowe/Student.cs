// Represents a student user with specific permissions
public class Student : User
{
    public Student(string name) : base(name)
    {
        Role = "Student";
    }

    public override void DisplayPermissions()
    {
        System.Console.WriteLine($"{Name} ({Role}) can borrow up to 3 books.");
    }
}
