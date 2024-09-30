namespace BooksSearchTelegramBot
{
    enum State
    {
        Start,
        My,
        Search,
        SearchBookByTitle,
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
