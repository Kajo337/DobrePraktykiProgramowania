// Represents a standard book object
public class Book
{
    public string Title { get; private set; }
    public string Author { get; private set; }

    // Constructor for initializing a book
    public Book(string title, string author)
    {
        Title = title;
        Author = author;
    }

    // Returns a formatted string representation of the book
    public override string ToString() => $"\"{Title}\" by {Author}";
}
