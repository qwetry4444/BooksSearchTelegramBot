using BooksSearchTelegramBot.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksSearchTelegramBot.Database.Repositories
{
    public class UserDeferredBookRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserDeferredBookRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserDeferredBook?> GetByUserAndBookIdAsync(long userId, string bookId)
        {
            return await _dbContext.UserDeferredBook.FindAsync(userId, bookId);
        }

        public async Task<List<UserDeferredBook>> GetAllUserDeferredBooksAsync(long userId)
        {
            return await _dbContext.UserDeferredBook.Where(it => it.UserId == userId).ToListAsync();
        }

        public async Task AddAsync(UserDeferredBook userDeferredBook)
        {
            _dbContext.UserDeferredBook.Add(userDeferredBook);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _dbContext.UserDeferredBook.FindAsync(id);
            if (book != null)
            {
                _dbContext.UserDeferredBook.Remove(book);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
