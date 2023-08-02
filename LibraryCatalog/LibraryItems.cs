namespace librarycatalog;

public class Book
{
    public String Title;
    public String Author;
    public String ISBN;
    public int PublicationYear;

    public Book(String title, String author, String isbn, int publicationYear)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        PublicationYear = publicationYear;
    }
}

public class MediaItem
{
    public String Title;
    public String MediaType;
    public int Duration;

    public MediaItem(String title, String mediaType, int duration)
    {
        Title = title;
        MediaType = mediaType;
        Duration = duration;
    }
}

public class Library
{
    public String Name;
    public String Address;
    private List<Book> Books;
    private List<MediaItem> MediaItems;

    public Library(String name, String address)
    {
        Name = name;
        Address = address;
        Books = new List<Book>();
        MediaItems = new List<MediaItem>();
    }
    public void AddBook(Book book)
    {
        Books.Add(book);
    }

    public void RemoveBook(Book book)
    {
        Books.Remove(book);
    }

    public void AddMediaItem(MediaItem item)
    {
        MediaItems.Add(item);
    }

    public void RemoveMediaItem(MediaItem item)
    {
        MediaItems.Remove(item);
    }

    public void PrintCatalog()
    {
        Console.WriteLine($"Catalog for Library {Name}.\n");
        
        Console.WriteLine($"Books ({Books.Count})");
        Console.WriteLine("---------------------------------------------");
        foreach (Book book in Books)
        {  
            Console.WriteLine($""" 
                Title: {book.Title}
                Author: {book.Author}
                ISBN: {book.ISBN}
                Publication Year: {book.PublicationYear}
            ---------------------------------------------
            """);
        }
        
        Console.WriteLine($"\nMedia Items ({MediaItems.Count})");
        Console.WriteLine("---------------------------------------------");
        foreach (MediaItem item in MediaItems)
        {
            Console.WriteLine($""" 
                Title: {item.Title}
                Media Type: {item.MediaType}
                Duration: {item.Duration} mins
            ---------------------------------------------
            """);
        }
    }
}