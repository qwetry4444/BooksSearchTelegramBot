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

        public static InlineKeyboardMarkup AuthorMenuInlineMarkup = new InlineKeyboardMarkup()
            .AddButton("💬 Подробнее", "💬 Подробнее")
            .AddButton("📚 Книги автора", "📚 Книги автора");

        public static InlineKeyboardMarkup ChoiceGenreInlineMarkup = new InlineKeyboardMarkup()
            .AddButton("💕 Роман", "💕 Роман") 
            .AddButton("😭 Драма", "😭 Драма")
            .AddNewRow()
            .AddButton("🌍 Путешествие", "🌍 Путешествие") 
            .AddButton("👽 Фэнтэзи", "👽 Фэнтэзи")
            .AddNewRow()
            .AddButton("🔎 Детектив", "🔎 Детектив")
            .AddButton("🔎 Детективе", "🔎 Детективе");
    }
}
