using OpenLibraryNET;

namespace BooksSearchTelegramBot.res
{
    internal static class StringsGeneration
    {
        public static String GetWelcomeMessageWithUsername(String userName)
        {
            return $"Здравствуйте {userName}, это бот для поиска книг";
        }

        public static String CreateAboutBookMessage(OLWork work, OLAuthor author)
        {
            String aboutBookString = String.Empty;
            if (work != null)
            {
                if (work.Data != null)
                {
                    if (work.Data.Title != null)
                    {
                        aboutBookString += $"<b>{work.Data.Title}</b>\n\n";
                    }
                    if (author != null)
                    {
                        if (author.Data != null)
                        {
                            if (author.Data.Name != null)
                            {
                                aboutBookString += $"<b>Автор</b>: {author.Data.Name}\n\n";
                            }
                        }
                    }
                    if (work.Data.ExtensionData != null)
                    {
                        if (work.Data.ExtensionData.ContainsKey("first_publish_year"))
                        {
                            aboutBookString += $"<b>Дата публикации</b>: {work.Data.ExtensionData["first_publish_year"]}\n\n";
                        }
                        else if (work.Data.ExtensionData.ContainsKey("first_publish_date"))
                        {
                            aboutBookString += $"<b>Дата публикации</b>: {work.Data.ExtensionData["first_publish_date"]}\n\n";
                        }
                    }

                    if (work.Data.Subjects != null && work.Data.Subjects.Count > 0)
                    {
                        // Жанры выставленны в алфавитном порядке, поэтому берем случайные жанры из списка
                        Random random = new();
                        int subjectsCount = work.Data.Subjects.Count >= 4 ? 4 : work.Data.Subjects.Count;
                        List<String> subjects = work.Data.Subjects.OrderBy(subject => random.Next()).Take(subjectsCount).ToList();

                        aboutBookString += $"<b>Жанры</b>: ";
                        for (int i = 0; i < subjectsCount; i++) { aboutBookString += $"{subjects[i]}, "; }
                        aboutBookString += "\n\n";

                    }

                    if (work.Ratings != null)
                    {
                        if (work.Ratings.Average != null)
                        {
                            aboutBookString += $"<b>Рейтинг</b>: {work.Ratings.Average}\n\n";
                        }
                    }
                }
            }
            return aboutBookString.Length > 1024 ? aboutBookString[..1023] : aboutBookString;
        }

        public static String CreateMoreAboutBookMessage(OLWork work)
        {
            String message = String.Empty;
            if (work != null)
            {
                if (work.Data != null)
                {
                    if (work.Data.Title != null)
                    {
                        message += $"<b>{work.Data.Title}</b>\n\n";
                    }
                    if (work.Data.Description != null && work.Data.Description != String.Empty)
                    {
                        // Если в описании книги меньше 800 символов, то добавляем его в message иначе, берем первые 800 обрезанные по последней точке.
                        var sentences = work.Data.Description.Split('.');
                        var shortenedDescription = string.Join(".", sentences[..^1]) + "."; // Восстанавливаем предложения до 800 символов

                        // Проверяем, не превышает ли длина итогового текста 800 символов
                        if (shortenedDescription.Length > 800)
                        {
                            shortenedDescription = shortenedDescription[..800];
                        }

                        message += shortenedDescription;
                        message += "\n\n";
                    }
                    if (work.Bookshelves != null)
                    {
                        message += $"<b>Прочитано</b> {work.Bookshelves.AlreadyRead} раз\n";
                        message += $"<b>Сейчас читают</b> {work.Bookshelves.CurrentlyReading} человек\n";
                        message += $"<b>Хотят прочитать</b> {work.Bookshelves.WantToRead} человек\n";
                    }
                    if (work.Editions != null && work.Editions.Count > 0)
                    {
                        message += $"<b>Издание</b>: {work.Editions.FirstOrDefault()}";
                    }
                }
            }

            return message;
        }

        public static String CreateAboutAuthorMessage(OLAuthor author)
        {
            String message = String.Empty;
            if (author != null)
            {
                if (author.Data != null)
                {
                    if (author.Data.Name != null)
                    {
                        message += $"<b>{author.Data.Name}</b>\n\n";
                    }
                    if (author.Data.BirthDate != null && author.Data.DeathDate != null)
                    {
                        message += $"<b>Годы жизни</b>: {author.Data.BirthDate} - {author.Data.DeathDate}\n\n";
                    }
                    if (author.Data.Bio != null)
                    {
                        message += "<b>Биография</b>: ";
                        if (author.Data.Bio.Length < 800)
                        {
                            message += author.Data.Bio;
                        }
                        else
                        {
                            var sentences = author.Data.Bio.Split('.');
                            var shortenedBio = string.Join(".", sentences[..^1]);

                            if (shortenedBio.Length > 800)
                            {
                                shortenedBio = shortenedBio[..800];
                            }

                            message += shortenedBio + ".\n\n";
                        }
                    }
                }
            }

            return message;
        }


        public static String CreateBookHead(OLWork work)
        {
            String bookHead = String.Empty;
            if (work != null)
            {
                if (work.Data != null)
                {
                    if (work.Data.Title != null)
                    {
                        bookHead += $"{work.Data.Title} | ";
                    }
                    if (work.Data.ExtensionData != null)
                    {
                        if (work.Data.ExtensionData.ContainsKey("first_publish_year"))
                        {
                            bookHead += $"{work.Data.ExtensionData["first_publish_year"]} | ";
                        }
                        else if (work.Data.ExtensionData.ContainsKey("first_publish_date"))
                        {
                            bookHead += $"{work.Data.ExtensionData["first_publish_date"]} | ";
                        }
                    }
                    if (work.Data.Subjects != null && work.Data.Subjects.Count > 0)
                    {
                        bookHead += $"{work.Data.Subjects[0]}";
                    }
                }
            }
            return bookHead;
        }
    }
}
