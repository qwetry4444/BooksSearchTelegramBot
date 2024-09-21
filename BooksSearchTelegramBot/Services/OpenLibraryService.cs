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

        async public Task<List<OLWork>>  SearchBookByTitle(string title)
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
                if (work.Data.CoverIDs != null && work.Data.CoverIDs.Count > 0)
                {
                    cover = await client.Image.GetCoverAsync("id", work.Data.CoverIDs.First().ToString(), "L");
                }
            }
            return cover;
        }

        async public Task<byte[]?> GetAuthorPhoto(OLAuthor author)
        {
            byte[]? authorPhoto = null;
            if (author.Data != null)
            {
                authorPhoto = await client.Image.GetCoverAsync("id", author.Data.PhotosIDs.First().ToString(), "L");
            }

            return authorPhoto;
        }

    }
}
