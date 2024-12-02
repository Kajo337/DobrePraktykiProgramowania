using System.Collections.Generic;
using Newtonsoft.Json;

// Adapter for importing books from JSON data
public class JsonBookAdapter : IBookDataProvider
{
    private readonly string jsonData;

    // Constructor accepts raw JSON data
    public JsonBookAdapter(string jsonData)
    {
        this.jsonData = jsonData;
    }

    // Converts JSON data to standard Book objects
    public IEnumerable<Book> GetBooks()
    {
        var bookData = JsonConvert.DeserializeObject<List<JsonBook>>(jsonData);
        var books = new List<Book>();

        foreach (var jsonBook in bookData)
        {
            books.Add(new Book(jsonBook.Title, jsonBook.Author));
        }

        return books;
    }

    // Internal class for deserialization
    private class JsonBook
    {
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
