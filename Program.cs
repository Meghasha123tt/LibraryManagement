using System.Collections;

namespace LibraryMngSystem
{
    // Book class
    public class Book
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public bool IsAvailable { get; set; }
    }

    class Library
    {
        public delegate void BookOperation(Book book);

        public event Action<string> BookNotification;

        private List<Book> books = new List<Book>();

        private SortedList<string, int> categoryCount = new SortedList<string, int>();

        private ArrayList removedBooks = new ArrayList();

        public void AddBook(Book book)
        {
            foreach (var b in books)
            {
                if (b.ID == book.ID)
                {
                    Console.WriteLine("Error: A book with this ID already exists.");
                    return;
                }
            }

            books.Add(book);
            if (categoryCount.ContainsKey(book.Category))
            {
                categoryCount[book.Category]++;
            }
            else
            {
                categoryCount.Add(book.Category, 1);
            }
        }

        public void IssueBook(int bookId)
        {
            List<Book> book = books.Where(delegate (Book b) { return b.ID == bookId; }).ToList();
            if (book.Count > 0)
            {
                if (book[0].IsAvailable)
                {
                    book[0].IsAvailable = false;
                    if (BookNotification != null)
                    {
                        BookNotification("Book issued: " + book[0].Title);
                    }
                }
                else
                {
                    Console.WriteLine("Book is not available.");
                }
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }

        public void ReturnBook(int bookId)
        {
            List<Book> book = books.Where(delegate (Book b) { return b.ID == bookId; }).ToList();
            if (book.Count > 0)
            {
                book[0].IsAvailable = true;
                if (BookNotification != null)
                {
                    BookNotification("Book returned: " + book[0].Title);
                }
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }

        public void RemoveBook(int bookId)
        {
            List<Book> book = books.Where(delegate (Book b) { return b.ID == bookId; }).ToList();
            if (book.Count > 0)
            {
                books.Remove(book[0]);
                removedBooks.Add(book[0]);
                categoryCount[book[0].Category]--;
                if (categoryCount[book[0].Category] == 0)
                {
                    categoryCount.Remove(book[0].Category);
                }
                Console.WriteLine("Book removed successfully.");
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }

        public List<Book> QueryBooks(string category)
        {
            List<Book> result = new List<Book>();
            foreach (var book in books)
            {
                if (book.Category.Equals(category))
                {
                    result.Add(book);
                }
            }
            return result;
        }

        public void DisplayBooks()
        {
            foreach (var book in books)
            {
                Console.WriteLine($"ID: {book.ID}, Title: {book.Title}, Author: {book.Author}, Category: {book.Category}, Available: {book.IsAvailable}");
            }
        }


        public void DisplayCategoryCounts()
        {
            foreach (var category in categoryCount)
            {
                Console.WriteLine($"Category: {category.Key}, Count: {category.Value}");
            }
        }

        public void DisplayRemovedBooks()
        {
            foreach (Book book in removedBooks)
            {
                Console.WriteLine($"ID: {book.ID}, Title: {book.Title}, Author: {book.Author}, Category: {book.Category}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Library library = new Library();

            library.BookNotification += delegate (string message) { Console.WriteLine("Notification: " + message); };

            while (true)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Add a Book");
                Console.WriteLine("2. Issue a Book");
                Console.WriteLine("3. Return a Book");
                Console.WriteLine("4. Remove a Book");
                Console.WriteLine("5. Query Books");
                Console.WriteLine("6. Display All Books");
                Console.WriteLine("7. Display Category Counts");
                Console.WriteLine("8. Display Removed Books");
                Console.WriteLine("9. Exit");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter Book ID:");
                        int id = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Book Title:");
                        string title = Console.ReadLine();
                        Console.WriteLine("Enter Book Author:");
                        string author = Console.ReadLine();
                        Console.WriteLine("Enter Book Category:");
                        string category = Console.ReadLine();
                        Console.WriteLine("Is the book available? (true/false):");
                        bool isAvailable = bool.Parse(Console.ReadLine());

                        Book newBook = new Book { ID = id, Title = title, Author = author, Category = category, IsAvailable = isAvailable };
                        library.AddBook(newBook);
                        break;

                    case 2:
                        Console.WriteLine("Enter Book ID to issue:");
                        int issueId = int.Parse(Console.ReadLine());
                        library.IssueBook(issueId);
                        break;

                    case 3:
                        Console.WriteLine("Enter Book ID to return:");
                        int returnId = int.Parse(Console.ReadLine());
                        library.ReturnBook(returnId);
                        break;

                    case 4:
                        Console.WriteLine("Enter Book ID to remove:");
                        int removeId = int.Parse(Console.ReadLine());
                        library.RemoveBook(removeId);
                        break;

                    case 5:
                        Console.WriteLine("Enter category to filter books by (leave blank for all):");
                        string queryCategory = Console.ReadLine();
                        var queriedBooks = library.QueryBooks(queryCategory);

                        Console.WriteLine("Queried Books:");
                        foreach (var book in queriedBooks)
                        {
                            Console.WriteLine($"ID: {book.ID}, Title: {book.Title}");
                        }
                        break;

                    case 6:
                        Console.WriteLine("All Books:");
                        library.DisplayBooks();
                        break;

                    case 7:
                        Console.WriteLine("Category Counts:");
                        library.DisplayCategoryCounts();
                        break;

                    case 8:
                        Console.WriteLine("Removed Books:");
                        library.DisplayRemovedBooks();
                        break;

                    case 9:
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }
    }
}
