using System;

// Factory class for creating different types of users
public static class UserFactory
{
    public static User CreateUser(string userType, string name)
    {
        return userType.ToLower() switch
        {
            "student" => new Student(name),
            "teacher" => new Teacher(name),
            "librarian" => new Librarian(name),
            _ => throw new ArgumentException("Invalid user type")
        };
    }
}