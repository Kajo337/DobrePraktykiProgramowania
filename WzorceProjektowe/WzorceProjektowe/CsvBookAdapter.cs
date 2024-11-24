using System.Collections.Generic;
using System.Linq;

// Adapter for importing books from CSV data
public class CsvBookAdapter : IBookDataProvider
{
    private readonly string csvData;

    // Constructor accepts raw CSV data
    public CsvBookAdapter(string csvData)
    {
        this.csvData = csvData;
    }

    // Converts CSV data to standard Book objects
    public IEnumerable<Book> GetBooks()
    {
        var books = new List<Book>();
        var lines = csvData.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line));

        foreach (var line in lines)
        {
            var fields = line.Split(',');
            if (fields.Length >= 2)
            {
                string title = fields[0].Trim();
                string author = fields[1].Trim();
                books.Add(new Book(title, author));
            }
        }

        return books;
    }
}
