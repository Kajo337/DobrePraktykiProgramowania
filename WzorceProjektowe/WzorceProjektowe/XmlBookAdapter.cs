using System.Collections.Generic;
using System.Xml.Linq;

// Adapter for importing books from XML data
public class XmlBookAdapter : IBookDataProvider
{
    private readonly string xmlData;

    // Constructor accepts raw XML data
    public XmlBookAdapter(string xmlData)
    {
        this.xmlData = xmlData;
    }

    // Converts XML data to standard Book objects
    public IEnumerable<Book> GetBooks()
    {
        var books = new List<Book>();
        var xmlDoc = XDocument.Parse(xmlData);

        foreach (var element in xmlDoc.Descendants("Book"))
        {
            string title = element.Element("Title")?.Value ?? "Unknown";
            string author = element.Element("Author")?.Value ?? "Unknown";
            books.Add(new Book(title, author));
        }

        return books;
    }
}
