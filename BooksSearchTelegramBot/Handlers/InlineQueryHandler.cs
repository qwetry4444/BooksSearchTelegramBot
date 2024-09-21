using BooksSearchTelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using OpenLibraryNET;
using BooksSearchTelegramBot.res;

namespace BooksSearchTelegramBot.Handlers
{
    class InlineQueryHandler(TelegramBotClient botClient, FSMContext context, OpenLibraryService openLibraryService)
    {
        OLWork work;
        OLAuthor author;


        public async Task OnInlineQuery(Update update)
        {
            if (update is { CallbackQuery: { } query })
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

                        default:
                            work = await openLibraryService.SearchBookById(query.Data.ToString());
                            if (work != null )
                            {
                                if (work.Data != null)
                                {
                                    if (work.Data.AuthorKeys != null && work.Data.AuthorKeys.Count > 0)
                                    {
                                        author = await openLibraryService.SearchAuthorById(work.Data.AuthorKeys.First());
                                    }
                                }
                            }
                            await botClient.SendTextMessageAsync(
                                chatId: query.From.Id,
                                text: StringsGeneration.CreateAboutBookMessage(work, author),
                                );

                            break;
                    }
                }
            }
        }
    }
}
