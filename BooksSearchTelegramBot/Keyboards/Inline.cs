using BooksSearchTelegramBot.res;
using OpenLibraryNET;
using OpenLibraryNET.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace BooksSearchTelegramBot.Keyboards
{
    internal static class Inline
    {
        public static InlineKeyboardMarkup BookMenuInlineMarkup = new InlineKeyboardMarkup()
            .AddButton("💬 Подробнее", "💬 Подробнее")
            .AddButton("👴 Автор", "👴 Автор")
            .AddNewRow()
            .AddButton("✅ Прочитано", "✅ Прочитано")
            .AddButton("📚 Отложить", "📚 Отложить");

        public static InlineKeyboardMarkup CreateBookMenuInlineMarkup(OLWork work)
        {
            if (work.Data != null)
            {
                return new InlineKeyboardMarkup()
                    .AddButton("💬 Подробнее", $"💬 Подробнее|{work.ID}")
                    .AddButton("👴 Автор", $"👴 Автор|{work.Data.AuthorKeys.FirstOrDefault()}")
                    .AddNewRow()
                    .AddButton("✅ Прочитано", $"✅ Прочитано|{work.ID}")
                    .AddButton("📚 Отложить", $"📚 Отложить|{work.ID}");
            }
            return BookMenuInlineMarkup;
        }

        public static InlineKeyboardMarkup CreateAuthorMenuInlineMarkup(OLAuthor author)
        {
            return new InlineKeyboardMarkup()
                .AddButton("💬 Подробнее", $"💬 Подробнее")
                .AddButton("📚 Книги автора", $"📚 Книги автора|{author.ID}");
        }

        public static InlineKeyboardMarkup ChoiceGenreInlineMarkup = new InlineKeyboardMarkup()
            .AddButton("💕 Роман", "💕 Роман") 
            .AddButton("😭 Драма", "😭 Драма")
            .AddNewRow()
            .AddButton("🌍 Путешествие", "🌍 Путешествие") 
            .AddButton("👽 Фэнтэзи", "👽 Фэнтэзи")
            .AddNewRow()
            .AddButton("🔎 Детектив", "🔎 Детектив")
            .AddButton("📈 Саморазвитие", "📈 Саморазвитие");


        public static InlineKeyboardMarkup CreateBookHeadsInlineKeyboard(List<OLWork> works)
        {
            InlineKeyboardMarkup BookHeadsInlineKeyboard = new InlineKeyboardMarkup();
            if (works != null)
            {
                foreach (OLWork work in works)
                {
                    BookHeadsInlineKeyboard.AddButton(StringsGeneration.CreateBookHead(work), $"work|{work.ID}");
                    BookHeadsInlineKeyboard.AddNewRow();
                }
            }
            return BookHeadsInlineKeyboard; 
        }
    }


}
