using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksSearchTelegramBot.res
{
    internal static class StringsGeneration
    {
        public static String GetWelcomeMessageWithUsername(String userName)
        {
            return $"Здравствуйте {userName}, это бот для поиска книг";
        }
        //public static readonly String WelcomeMessageWithoutUsername = "Здравствуйте, это бот для поиска книг";

        //public static readonly String BackButton = "⬅ назад";

        //public static readonly String MyButton = "📚 Моё";
        //public static readonly String MyMenuMessage = "Посмотреть прочитанные или отолженные книги?";

        //public static readonly String MyReadedButton = "✅ Прочитанные";
        //public static readonly String MyReadedMessage = "Прочитанные вами книги:";

        //public static readonly String MyDefferedButton = "📚 Отложенные";
        //public static readonly String MyDefferedMessage = "Отложенные книги:";

        //public static readonly String SearchButton = "🔎 Поиск";
        //public static readonly String SearchMessage = "Выберите критерий поиска";

        //public static readonly String BookTitleButton = "📄 Название";
        //public static readonly String BookTitleMessage = "Введите название книги: ";
        //public static readonly String RatingButton = "⭐ Рейтинг";
        //public static readonly String RatingMessage = "Книги отсортированные по рейтингу: ";
        //public static readonly String SelectMoreApproriate = "Выберите наиболее подходящее";

        //public static readonly String AddToReadedMessage = "Книга добавлена в прочитанные";
        //public static readonly String AddToDefferedMessage = "Книга добавлена в отложенные";
    }
}
