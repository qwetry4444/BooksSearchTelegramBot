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

        async public Task<OLWorkData[]?>  SearchBookByTitle(string title)
        {
            var parameters = new KeyValuePair<string, string>("limit", "10");

            OLWorkData[]? works = await client.Search.GetSearchResultsAsync(title, parameters);

            if (works != null && works.Length > 0)
            {
                if (works.First() != null)
                {
                    await SearchBookById(works.First().ID);
                }
            }
            
            return works;
            
        }

        async public Task<OLWork> SearchBookById(string id)
        {
            var parameters = new KeyValuePair<string, string>("limit", "10");
            
            OLWork work = await client.GetWorkAsync(id, 10);
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
                cover = await client.Image.GetCoverAsync("olid", work.Data.CoverIDs.First().ToString(), "L");
            }

            return cover;
        }

        async public Task<byte[]?> GetBookCover(OLAuthor author)
        {
            byte[]? authorPhoto = null;
            if (author.Data != null)
            {
                authorPhoto = await client.Image.GetCoverAsync("olid", author.Data.PhotosIDs.First().ToString(), "L");
            }

            return authorPhoto;
        }

    }
}
