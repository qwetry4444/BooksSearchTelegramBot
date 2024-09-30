using BooksSearchTelegramBot.Database.Models;
using OpenLibraryNET;
using OpenLibraryNET.Data;

namespace BooksSearchTelegramBot.Services
{
    public class OpenLibraryService
    {
        OpenLibraryClient client = new OpenLibraryClient();

        async public Task<List<OLWork>?> SearchBookByTitle(string title)
        {
            var worksCount = 6;
            var works = new List<OLWork>();
            var parameters = new KeyValuePair<string, string>("limit", worksCount.ToString());

            OLWorkData[]? worksData = await client.Search.GetSearchResultsAsync(title, parameters);

            if (worksData != null && worksData.Length > 0)
            {
                foreach (OLWorkData workData in worksData)
                {
                    try
                    {
                        var work = await SearchBookById(workData.ID);
                        if (work != null)
                        {
                            works.Add(work);
                        }
                    }
                    catch (Exception)
                    {
                        return null;
                    }

                }
            }

            return works;
        }

        async public Task<OLWork?> SearchBookById(string bookId)
        {
            try
            {
                OLWork work = await client.GetWorkAsync(bookId);
                return work;
            }
            catch (Exception)
            {
                return null;
            }

        }

        async public Task<OLAuthor?> SearchAuthorById(string authorId)
        {
            try
            {
                OLAuthor authorData = await client.GetAuthorAsync(authorId);
                return authorData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        async public Task<byte[]?> GetBookCover(OLWork work)
        {
            byte[]? cover = null;
            if (work.Data != null)
            {
                try
                {
                    if (work.Data.CoverIDs != null && work.Data.CoverIDs.Count > 0)
                    {
                        cover = await client.Image.GetCoverAsync("id", work.Data.CoverIDs.First().ToString(), "L");
                    }
                    return cover;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        async public Task<byte[]?> GetAuthorPhoto(OLAuthor author)
        {
            byte[]? authorPhoto = null;
            if (author.Data != null)
            {
                try
                {
                    authorPhoto = await client.Image.GetAuthorPhotoAsync("id", author.Data.PhotosIDs.First().ToString(), "L");
                    return authorPhoto;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        async public Task<List<OLWork>?> GetListOfWorkByListOfUserReadedBook(List<UserReadedBook> userReadedBooks)
        {
            try
            {
                var tasks = userReadedBooks.Select(userReadedBook => SearchBookById(userReadedBook.BookId));
                var works = await Task.WhenAll(tasks);
                return works.ToList();
            }
            catch (Exception)
            {
                return null;
            }

        }

        async public Task<List<OLWork>> GetListOfWorkByListOfUserDeferredBook(List<UserDeferredBook> userDeferredBooks)
        {
            try
            {
                var tasks = userDeferredBooks.Select(userDeferredBooks => SearchBookById(userDeferredBooks.BookId));
                var works = await Task.WhenAll(tasks);
                return works.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
