using BooksSearchTelegramBot.res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace BooksSearchTelegramBot.Keyboards
{
    internal static class Reply
    {
        public static IReplyMarkup StartMenuReplyMarkup = new ReplyKeyboardMarkup(true)
            .AddButtons(Strings.SearchMenuButton, Strings.MyMenuButton);

        public static IReplyMarkup MyMenuReplyMarkup = new ReplyKeyboardMarkup(true)
            .AddButtons(Strings.MyReadedButton, Strings.MyDefferedButton)
            .AddNewRow()
            .AddButtons(Strings.BackButton);

        public static IReplyMarkup SearchMenuReplyMarkup = new ReplyKeyboardMarkup(true)
            .AddButton(Strings.SearchByTitleButton)
            .AddNewRow(Strings.SearchByRatingButton)
            .AddNewRow(Strings.SearchByGenreButton);
        
            
    }
}
