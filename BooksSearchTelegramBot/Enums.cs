using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksSearchTelegramBot
{
    enum State
    {
        Start,
        My,
        Search,
        SearchBookByTitle,
        SearchBookByGenre,
    }

    enum CallbackDataType
    {
        GenreRoman,
        GenreDrama,
        GenreJourney,
        GenreFantasy,
        GenreDetective,
        GenreSelfDevelopment,

        MoreAboutBook,
        AuthorOfBook,
        AddBookToReaded,
        AddBookToDeffered,

        MoreAboutAuthor,
        BoooksOfAuthor,
    }
    
}
