namespace BooksSearchTelegramBot.Database.Models
{
    public class UserReadedBook
    {
        public UserReadedBook(long userId, string bookId) { UserId = userId; BookId = bookId; }

        public int Id { get; set; }
        public long UserId { get; set; }
        public string BookId { get; set; } = string.Empty;
    }
}
