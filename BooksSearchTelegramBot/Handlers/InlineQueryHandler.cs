using BooksSearchTelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using OpenLibraryNET;
using BooksSearchTelegramBot.res;
using BooksSearchTelegramBot.Keyboards;
using Telegram.Bot.Types.Enums;
using System.IO;

namespace BooksSearchTelegramBot.Handlers
{
    class InlineQueryHandler(TelegramBotClient botClient, FSMContext context, OpenLibraryService openLibraryService)
    {



        public async Task OnInlineQuery(Update update)
        {
            if (update is { CallbackQuery: { } query })
            {
                if (query.Data != null)
                {
                    string[] dataParts = query.Data.Split('|');

                    if (dataParts.Length > 1)
                    {
                        switch (dataParts[0])
                        {
                            case "work":
                                await onBookSearch(query, dataParts[1]);
                                break;

                            case "💬 Подробнее":
                                await onGetMoreAbourtBook(query, dataParts[1]);
                                break;

                            case "👴 Автор":
                                await onGetBookAuthor(query, dataParts[1]);
                                break;

                            case "✅ Прочитано":
                                break;

                            case "📚 Отложить":
                                break;
                        }
                    }
                    else
                    {
                        switch (dataParts[0])
                        {
                            //case CallbackDataType.GenreJourney:
                            //    await botClient.SendTextMessageAsync(
                            //        chatId: query.From.Id,
                            //        text: $"Книги в жанре {query.InlineMessageId}");
                            //    break;
                            //case CallbackDataType.GenreDetective:
                            //    await botClient.SendTextMessageAsync(
                            //        chatId: query.From.Id,
                            //        text: $"Книги в жанре {query.InlineMessageId}");
                            //    break;
                            //case CallbackDataType.GenreSelfDevelopment:
                            //    await botClient.SendTextMessageAsync(
                            //        chatId: query.From.Id,
                            //        text: $"Книги в жанре {query.InlineMessageId}");
                            //    break;
                            //case CallbackDataType.GenreFantasy:
                            //    await botClient.SendTextMessageAsync(
                            //        chatId: query.From.Id,
                            //        text: $"Книги в жанре {query.InlineMessageId}");
                            //    break;
                            //case CallbackDataType.GenreDrama:
                            //    await botClient.SendTextMessageAsync(
                            //        chatId: query.From.Id,
                            //        text: $"Книги в жанре {query.InlineMessageId}");
                            //    break;
                            //case CallbackDataType.GenreRoman:
                            //    await botClient.SendTextMessageAsync(
                            //        chatId: query.From.Id,
                            //        text: $"Книги в жанре {query.InlineMessageId}");
                            //    break;
                        }
                    }
                }
            }
        }
        
        public async Task onBookSearch(CallbackQuery query, string workId)
        {
            OLWork work = await openLibraryService.SearchBookById(workId);
            byte[]? cover = await openLibraryService.GetBookCover(work);

            if (work != null)
            {
                if (work.Data != null)
                {
                    if (work.Data.AuthorKeys != null && work.Data.AuthorKeys.Count > 0)
                    {
                        OLAuthor author = await openLibraryService.SearchAuthorById(work.Data.AuthorKeys.First());
                        if (author != null)
                        {
                            if (cover != null)
                            {
                                using (var stream = new MemoryStream(cover))
                                    await botClient.SendPhotoAsync(
                                        chatId: query.From.Id,
                                        photo: stream,
                                        caption: StringsGeneration.CreateAboutBookMessage(work, author),
                                        replyMarkup: Inline.CreateBookMenuInlineMarkup(work),
                                        parseMode: ParseMode.Html);
                            }
                            else
                            {
                                await botClient.SendTextMessageAsync(
                                        chatId: query.From.Id,
                                        text: StringsGeneration.CreateAboutBookMessage(work, author),
                                        replyMarkup: Inline.CreateBookMenuInlineMarkup(work),
                                        parseMode: ParseMode.Html);
                            }
                        }
                    }
                }

            }
            context.State = State.Search;
        }

        public async Task onGetMoreAbourtBook(CallbackQuery query, string workId)
        {
            OLWork work = await openLibraryService.SearchBookById(workId);
            String message = StringsGeneration.CreateMoreAboutBookMessage(work);

            await botClient.SendTextMessageAsync(
                    chatId: query.From.Id,
                    text: message,
                    replyMarkup: Reply.SearchMenuReplyMarkup,
                    parseMode: ParseMode.Html);
        }

        public async Task onGetBookAuthor(CallbackQuery query, string athorId)
        {
            OLWork work = await openLibraryService.SearchBookById(workId);
            if (work != null)
            {
                if (work.Data != null)
                {
                    if (work.Data.AuthorKeys != null && work.Data.AuthorKeys.Count > 0)
                    {
                        OLAuthor author = await openLibraryService.SearchAuthorById(work.Data.AuthorKeys.FirstOrDefault());
                        if (author != null)
                        {

                        }
                    }
                }
            }

        }
    }
}
