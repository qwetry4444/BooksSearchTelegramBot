using BooksSearchTelegramBot.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksSearchTelegramBot.Database.Repositories
{
    public class UserReadedBookRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserReadedBookRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserReadedBook?> GetByUserAndBookIdAsync(long userId, string bookId)
        {
            return await _dbContext.UserReadedBook.FindAsync(userId, bookId);
        }

        public async Task<List<UserReadedBook>> GetAllUserReadedBooksAsync(long userId)
        {
            return await _dbContext.UserReadedBook.Where(it => it.UserId == userId).ToListAsync();
        }

        public async Task AddAsync(UserReadedBook userReadedBook)
        {
            _dbContext.UserReadedBook.Add(userReadedBook);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _dbContext.UserReadedBook.FindAsync(id);
            if (book != null)
            {
                _dbContext.UserReadedBook.Remove(book);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
