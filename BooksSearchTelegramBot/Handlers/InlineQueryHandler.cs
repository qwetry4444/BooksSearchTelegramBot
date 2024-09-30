using BooksSearchTelegramBot.Keyboards;
using BooksSearchTelegramBot.res;
using BooksSearchTelegramBot.Services;
using OpenLibraryNET;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BooksSearchTelegramBot.Handlers
{
    class InlineQueryHandler(TelegramBotClient botClient, OpenLibraryService openLibraryService, DbService dbService)
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
                                await OnBookSearch(query, dataParts[1]);
                                break;

                            case "💬 Подробнее":
                                await OnGetMoreAbourtBook(query, dataParts[1]);
                                break;

                            case "👴 Автор":
                                await OnGetBookAuthor(query, dataParts[1]);
                                break;

                            case "✅ Прочитано":
                                await OnReadedBook(query, dataParts[1]);
                                break;

                            case "📚 Отложить":
                                await OnDeferredBook(query, dataParts[1]);
                                break;
                        }
                    }
                }
            }
        }

        public async Task OnBookSearch(CallbackQuery query, string workId)
        {
            try
            {
                OLWork? work = await openLibraryService.SearchBookById(workId);

                if (work != null)
                {
                    byte[]? cover = await openLibraryService.GetBookCover(work);
                    if (work.Data != null)
                    {
                        if (work.Data.AuthorKeys != null && work.Data.AuthorKeys.Count > 0)
                        {
                            OLAuthor? author = await openLibraryService.SearchAuthorById(work.Data.AuthorKeys.First());
                            if (author != null)
                            {
                                if (cover != null)
                                {
                                    using var stream = new MemoryStream(cover);
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
                    return;
                }

            }
            catch (Exception) { }
            await botClient.SendTextMessageAsync(
                        chatId: query.From.Id,
                        text: Strings.CouldNotFindBook,
                        parseMode: ParseMode.Html);
        }

        public async Task OnGetMoreAbourtBook(CallbackQuery query, string workId)
        {
            try
            {
                OLWork? work = await openLibraryService.SearchBookById(workId);
                if (work != null)
                {
                    String message = StringsGeneration.CreateMoreAboutBookMessage(work);

                    await botClient.SendTextMessageAsync(
                            chatId: query.From.Id,
                            text: message,
                            replyMarkup: Reply.SearchMenuReplyMarkup,
                            parseMode: ParseMode.Html);
                    return;
                }

            }
            catch (Exception) { }
            await botClient.SendTextMessageAsync(
                        chatId: query.From.Id,
                        text: Strings.CouldNotFindInformation,
                        parseMode: ParseMode.Html);
        }

        public async Task OnGetBookAuthor(CallbackQuery query, string athorId)
        {
            try
            {
                OLAuthor? author = await openLibraryService.SearchAuthorById(athorId);

                if (author != null)
                {
                    byte[]? authorPhoto = await openLibraryService.GetAuthorPhoto(author);
                    String message = StringsGeneration.CreateAboutAuthorMessage(author);
                    if (authorPhoto != null)
                    {
                        using (var stream = new MemoryStream(authorPhoto))
                            await botClient.SendPhotoAsync(
                                            chatId: query.From.Id,
                                            photo: stream,
                                            caption: message,
                                            replyMarkup: Inline.CreateAuthorMenuInlineMarkup(author),
                                            parseMode: ParseMode.Html);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(
                                            chatId: query.From.Id,
                                            text: message,
                                            replyMarkup: Inline.CreateAuthorMenuInlineMarkup(author),
                                            parseMode: ParseMode.Html);
                    }
                    return;
                }
            }
            catch (Exception) { }
            await botClient.SendTextMessageAsync(
                        chatId: query.From.Id,
                        text: Strings.CouldNotFindAuthor,
                        parseMode: ParseMode.Html);
        }

        public async Task OnReadedBook(CallbackQuery query, string bookId)
        {
            dbService.AddUserReadedBook(query.From.Id, bookId);
            await botClient.SendTextMessageAsync(
                    chatId: query.From.Id,
                    text: Strings.AddToReadedMessage,
                    replyMarkup: Reply.SearchMenuReplyMarkup,
                    parseMode: ParseMode.Html);
        }

        public async Task OnDeferredBook(CallbackQuery query, string bookId)
        {
            dbService.AddUserReadedBook(query.From.Id, bookId);
            await botClient.SendTextMessageAsync(
                    chatId: query.From.Id,
                    text: Strings.AddToDefferedMessage,
                    replyMarkup: Reply.SearchMenuReplyMarkup,
                    parseMode: ParseMode.Html);
        }
    }
}
