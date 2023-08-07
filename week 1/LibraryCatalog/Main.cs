using librarycatalog;

Book book = new Book("Ananas", "Someguy", "938utoi2ih", 1999);
Book book2 = new Book("Ananas", "Someguy", "differente", 1999);
Book book3 = new Book("Ananas", "Someguy", "mocha", 1999);

List<Book> books = new() {book, book2, book3};

List<MediaItem> medias = new()
{
    new MediaItem("C# fundamentals", "CD", 20),
    new MediaItem("How to Eat", "DVD", 30)
};

Library library = new Library("Abrehot", "I dont know address man");

foreach (Book buk in books)
{
    library.AddBook(buk);
}

foreach (MediaItem item in medias)
{
    library.AddMediaItem(item);
}

library.PrintCatalog();