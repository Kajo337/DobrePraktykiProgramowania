using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Singleton Pattern ===\n");

        LibraryCatalog catalog = LibraryCatalog.Instance;

        string jsonData = "[{\"Title\":\"1984\",\"Author\":\"George Orwell\"}]";
        IBookDataProvider jsonAdapter = new JsonBookAdapter(jsonData);

        Console.WriteLine("Importing books from JSON:");
        foreach (var book in jsonAdapter.GetBooks())
        {
            catalog.AddBook(book);
        }

        Console.WriteLine("\n=== Factory Pattern ===\n");

        User student = UserFactory.CreateUser("Student", "Alice");
        student.DisplayPermissions();

        Console.WriteLine("\n=== Observer Pattern ===\n");

        // Creating observers (users)
        UserObserver user1 = new UserObserver("Alice");
        UserObserver user2 = new UserObserver("Bob");

        // Subscribing users to notifications
        catalog.Subscribe(user1);
        catalog.Subscribe(user2);

        // Adding a book (triggers notifications)
        catalog.AddBook(new Book("Brave New World", "Aldous Huxley"));

        // Removing a book (triggers notifications)
        catalog.RemoveBook(new Book("1984", "George Orwell"));

        // Unsubscribing a user
        catalog.Unsubscribe(user1);

        // Adding another book (only remaining subscribers get notified)
        catalog.AddBook(new Book("To Kill a Mockingbird", "Harper Lee"));

        Console.WriteLine("\n=== Program End ===");
    }
}
