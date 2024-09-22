using BooksSearchTelegramBot.Database.Models;
using OpenLibraryNET;
using OpenLibraryNET.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksSearchTelegramBot.Services
{
    public class OpenLibraryService
    {
        OpenLibraryClient client = new OpenLibraryClient();

        async public Task<List<OLWork>> SearchBookByTitle(string title)
        {
            var worksCount = 6;
            var works = new List<OLWork>();
            var parameters = new KeyValuePair<string, string>("limit", worksCount.ToString());

            OLWorkData[]? worksData = await client.Search.GetSearchResultsAsync(title, parameters);

            if (worksData != null && worksData.Length > 0)
            {
                foreach (OLWorkData workData in worksData)
                {
                    works.Add(await SearchBookById(workData.ID));
                }
            }

            return works;
        }

        async public Task<OLWork> SearchBookById(string bookId)
        {
            OLWork work = await client.GetWorkAsync(bookId);
            return work;
        }

        async public Task<OLAuthor> SearchAuthorById(string authorId)
        {
            OLAuthor authorData = await client.GetAuthorAsync(authorId);
            return authorData;
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
                catch (Exception ex)
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
                } catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }

        async public Task<List<OLWork>> GetListOfWorkByListOfUserReadedBook(List<UserReadedBook> userReadedBooks) 
        { 
            //List<OLWork> works = [];
            //foreach (UserReadedBook userReadedBook in userDeferredBooks)
            //{
            //    works.Add(await SearchBookById(userReadedBook.BookId));
            //}
            //return works;

            var tasks = userReadedBooks.Select(userReadedBook => SearchBookById(userReadedBook.BookId));
            var works = await Task.WhenAll(tasks);
            return works.ToList();
        }

        async public Task<List<OLWork>> GetListOfWorkByListOfUserDeferredBook(List<UserDeferredBook> userDeferredBooks)
        {
            var tasks = userDeferredBooks.Select(userDeferredBooks => SearchBookById(userDeferredBooks.BookId));
            var works = await Task.WhenAll(tasks);
            return works.ToList();
        }


    }
}
