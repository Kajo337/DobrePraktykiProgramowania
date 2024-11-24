using System;

// Represents a user who receives notifications
public class UserObserver : INotificationObserver
{
    public string Name { get; private set; }

    public UserObserver(string name)
    {
        Name = name;
    }

    // Receives a notification
    public void Update(string message)
    {
        Console.WriteLine($"Notification for {Name}: {message}");
    }
}
