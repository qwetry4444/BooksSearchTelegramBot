using BooksSearchTelegramBot.Database.Models;
using BooksSearchTelegramBot.Keyboards;
using BooksSearchTelegramBot.res;
using BooksSearchTelegramBot.Services;
using OpenLibraryNET;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace BooksSearchTelegramBot.Handlers
{
    class TextMessageHandler(TelegramBotClient botClient, FSMContext context, OpenLibraryService openLibraryService, DbService dbService)
    {
        public async Task OnMessage(Message msg, UpdateType type)
        {
            switch (context.State)
            {
                case State.Start:
                    HandleOnStartState(msg);
                    break;

                case State.My:
                    HandleOnMyState(msg);
                    break;

                case State.Search: 
                    HandleOnSearchState(msg);
                    break;

                case State.SearchBookByTitle:
                    HandleOnSearchBookByTitleState(msg);
                    break;
            }
        }

        public async void HandleOnStartState(Message msg)
        {
            switch (msg.Text)
            {
                case "/start":
                    var username = msg.Chat.Username;
                    if (username == null)
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.WelcomeMessageWithoutUsername,
                            replyMarkup: Reply.StartMenuReplyMarkup);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: StringsGeneration.GetWelcomeMessageWithUsername(username),
                            replyMarkup: Reply.StartMenuReplyMarkup);
                    }
                    context.State = State.Start;
                    break;

                case "📚 Моё":
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.MyMenuMessage,
                            replyMarkup: Reply.MyMenuReplyMarkup);
                    context.State = State.My;
                    break;

                case "🔎 Поиск":
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.SearchMenuMessage,
                            replyMarkup: Reply.SearchMenuReplyMarkup);
                    context.State = State.Search;
                    break;

                default:
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.PleaseUseKeyboards,
                            replyMarkup: Reply.SearchMenuReplyMarkup);
                    break;
            }
        }

        public async void HandleOnMyState(Message msg)
        {
            switch (msg.Text)
            {
                case "📚 Отложенные":
                    List<UserDeferredBook> userDeferredBooks = await dbService.GetUserDeferredBooks(msg.From.Id);
                    if (userDeferredBooks != null && userDeferredBooks.Count > 0)
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.MyDefferedMessage,
                            replyMarkup: Inline.CreateBookHeadsInlineKeyboard(await openLibraryService.GetListOfWorkByListOfUserDeferredBook(userDeferredBooks)));
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.UserDontHaveDeferredBooks);
                    }
                    break;

                case "✅ Прочитанные":
                    List<UserReadedBook> userReadedBooks = await dbService.GetUserReadedBooks(msg.From.Id);
                    if (userReadedBooks != null && userReadedBooks.Count > 0)
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.MyReadedMessage,
                            replyMarkup: Inline.CreateBookHeadsInlineKeyboard(await openLibraryService.GetListOfWorkByListOfUserReadedBook(userReadedBooks)));
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.UserDontHaveReadedBooks);
                    }
                    break;

                case "⬅ Назад":
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.StartMenuMessage,
                            replyMarkup: Reply.StartMenuReplyMarkup);
                    context.State = State.Start;
                    break;

                default:
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.PleaseUseKeyboards,
                            replyMarkup: Reply.MyMenuReplyMarkup);
                    break;
            }
        }

        public async void HandleOnSearchState(Message msg)
        {
            switch (msg.Text)
            {
                case "📄 Название":
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.SearchByTitleMessage,
                            replyMarkup: Reply.SearchMenuReplyMarkup);
                    context.State = State.SearchBookByTitle;
                    break;

                case "💫 Жанр":
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.SearchByGenreMessage,
                            replyMarkup: Inline.ChoiceGenreInlineMarkup);
                    break;

                case "⭐ Рейтинг":
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.SearchByRatingMessage,
                            replyMarkup: Reply.SearchMenuReplyMarkup);
                    break;

                case "⬅ Назад":
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.StartMenuMessage,
                            replyMarkup: Reply.StartMenuReplyMarkup);
                    context.State = State.Start;
                    break;

                default:
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.PleaseUseKeyboards,
                            replyMarkup: Reply.SearchMenuReplyMarkup);
                    break;
            }
        }

        public async void HandleOnSearchBookByTitleState(Message msg)
        {
            List<OLWork> works = await openLibraryService.SearchBookByTitle(msg.Text);

            await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.SelectMoreApproriate,
                            replyMarkup: Inline.CreateBookHeadsInlineKeyboard(works));
            context.State = State.Search;
        }

        public async void HandleOnSearchBookByGenreState(Message msg)
        {
            await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: "Выберите жанр",
                            replyMarkup: Inline.ChoiceGenreInlineMarkup);
            context.State = State.Search;
        }

        public async void HandleOnSearchBookByRateState(Message msg)
        {
            await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: "Книги отсортированные по рейтингу",
                            replyMarkup: Reply.SearchMenuReplyMarkup);
            context.State = State.Search;
        }
    }
}
