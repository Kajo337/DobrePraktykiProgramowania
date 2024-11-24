using System.Collections.Generic;

// Interface for data providers (adapters)
public interface IBookDataProvider
{
    // Fetches books in a standard format
    IEnumerable<Book> GetBooks();
}
