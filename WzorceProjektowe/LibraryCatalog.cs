using System;
using System.Collections.Generic;

// Singleton: Manages the library's book catalog
public sealed class LibraryCatalog
{
    private static readonly LibraryCatalog instance = new LibraryCatalog();
    private List<Book> books = new List<Book>();

    // List of observers
    private List<INotificationObserver> observers = new List<INotificationObserver>();

    private LibraryCatalog()
    {
        Console.WriteLine("LibraryCatalog instance created!");
    }

    public static LibraryCatalog Instance => instance;

    public void AddBook(Book book)
    {
        books.Add(book);
        Console.WriteLine($"Book added: {book}");
        NotifyObservers($"The book \"{book.Title}\" by {book.Author} is now available.");
    }

    public void RemoveBook(Book book)
    {
        books.Remove(book);
        Console.WriteLine($"Book removed: {book}");
        NotifyObservers($"The book \"{book.Title}\" by {book.Author} has been removed from the catalog.");
    }

    public IEnumerable<Book> GetBooks()
    {
        Console.WriteLine("Retrieving list of books...");
        return books;
    }

    // Subscribe an observer
    public void Subscribe(INotificationObserver observer)
    {
        observers.Add(observer);
        Console.WriteLine("A user has subscribed to notifications.");
    }

    // Unsubscribe an observer
    public void Unsubscribe(INotificationObserver observer)
    {
        observers.Remove(observer);
        Console.WriteLine("A user has unsubscribed from notifications.");
    }

    // Notify all observers
    private void NotifyObservers(string message)
    {
        foreach (var observer in observers)
        {
            observer.Update(message);
        }
    }
}
