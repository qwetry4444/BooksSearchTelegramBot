using BooksSearchTelegramBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using BooksSearchTelegramBot.Keyboards;

namespace BooksSearchTelegramBot.Handlers
{
    class InlineQueryHandler(TelegramBotClient botClient, FSMContext context, OpenLibraryService openLibraryService)
    {
        public async Task OnInlineQuery(CallbackQuery query, UpdateType type)
        {
            if (int.TryParse(query.Data, out int callbackTypeId))
            {
                CallbackDataType callbackType = (CallbackDataType)callbackTypeId;
                switch (callbackType)
                {
                    case CallbackDataType.GenreJourney:
                        await botClient.SendTextMessageAsync(
                            chatId: query.From.Id,
                            text: $"Книги в жанре {query.InlineMessageId}");
                        break;
                    case CallbackDataType.GenreDetective:
                        await botClient.SendTextMessageAsync(
                            chatId: query.From.Id,
                            text: $"Книги в жанре {query.InlineMessageId}");
                        break;
                    case CallbackDataType.GenreSelfDevelopment:
                        await botClient.SendTextMessageAsync(
                            chatId: query.From.Id,
                            text: $"Книги в жанре {query.InlineMessageId}");
                        break;
                    case CallbackDataType.GenreFantasy:
                        await botClient.SendTextMessageAsync(
                            chatId: query.From.Id,
                            text: $"Книги в жанре {query.InlineMessageId}");
                        break;
                    case CallbackDataType.GenreDrama:
                        await botClient.SendTextMessageAsync(
                            chatId: query.From.Id,
                            text: $"Книги в жанре {query.InlineMessageId}");
                        break;
                    case CallbackDataType.GenreRoman:
                        await botClient.SendTextMessageAsync(
                            chatId: query.From.Id,
                            text: $"Книги в жанре {query.InlineMessageId}");
                        break;

                    case CallbackDataType.
                }
            }
        }
    }
}
