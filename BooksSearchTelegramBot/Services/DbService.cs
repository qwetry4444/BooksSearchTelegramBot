using BooksSearchTelegramBot.Database.Models;
using BooksSearchTelegramBot.Database.Repositories;

namespace BooksSearchTelegramBot.Services
{
    public class DbService
    {
        UserReadedBookRepository userReadedBookRepository;
        UserDeferredBookRepository userDeferredBookRepository;

        public DbService(UserReadedBookRepository _userReadedBookRepository, UserDeferredBookRepository _userDeferredBookRepository) 
        {
            userReadedBookRepository = _userReadedBookRepository;
            userDeferredBookRepository = _userDeferredBookRepository;
        }

        async public Task<List<UserReadedBook>> GetUserReadedBooks(long userId)
        {
            return await userReadedBookRepository.GetAllUserReadedBooksAsync(userId);
        }

        async public Task<List<UserDeferredBook>> GetUserDeferredBooks(long userId)
        {
            return await userDeferredBookRepository.GetAllUserDeferredBooksAsync(userId);
        }

        async public void AddUserReadedBook(long userId, string bookId)
        {
            await userReadedBookRepository.AddAsync(new UserReadedBook(userId, bookId));
        }

        async public void AddUserDeferredBook(long userId, string bookId)
        {
            await userDeferredBookRepository.AddAsync(new UserDeferredBook(userId, bookId));
        }
    }
}
