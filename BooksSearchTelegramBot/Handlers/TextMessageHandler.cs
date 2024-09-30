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
    class TextMessageHandler(TelegramBotClient botClient, Dictionary<long, FSMContext> contexts, OpenLibraryService openLibraryService, DbService dbService)
    {
        public async Task OnMessage(Message msg, UpdateType type)
        {
            if (!contexts.ContainsKey(msg.Chat.Id))
            {
                contexts[msg.Chat.Id] = new FSMContext();
            }

            switch (contexts[msg.Chat.Id].State)
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

                    contexts[msg.Chat.Id].State = State.Start;

                    break;

                case "📚 Моё":
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.MyMenuMessage,
                            replyMarkup: Reply.MyMenuReplyMarkup);
                    contexts[msg.Chat.Id].State = State.My;
                    break;

                case "🔎 Поиск":
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.SearchMenuMessage,
                            replyMarkup: Reply.SearchMenuReplyMarkup);
                    contexts[msg.Chat.Id].State = State.Search;
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
                        try
                        {
                            List<OLWork>? works = await openLibraryService.GetListOfWorkByListOfUserDeferredBook(userDeferredBooks);
                            if (works != null)
                            {
                                await botClient.SendTextMessageAsync(
                                    chatId: msg.Chat,
                                    text: Strings.MyDefferedMessage,
                                    replyMarkup: Inline.CreateBookHeadsInlineKeyboard(works));
                            }
                            else
                            {
                                await botClient.SendTextMessageAsync(
                                        chatId: msg.Chat,
                                        text: Strings.CouldNotFindDeferredBooks,
                                        replyMarkup: Reply.MyMenuReplyMarkup);
                            }
                        }
                        catch (Exception) { }

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
                        try
                        {
                            List<OLWork>? works = await openLibraryService.GetListOfWorkByListOfUserReadedBook(userReadedBooks);
                            if (works != null)
                            {
                                await botClient.SendTextMessageAsync(
                                        chatId: msg.Chat,
                                        text: Strings.MyReadedMessage,
                                        replyMarkup: Inline.CreateBookHeadsInlineKeyboard(works));
                            }
                            else
                            {
                                await botClient.SendTextMessageAsync(
                                        chatId: msg.Chat,
                                        text: Strings.CouldNotFindReadedBooks,
                                        replyMarkup: Reply.MyMenuReplyMarkup);
                            }
                        }
                        catch (Exception) { }

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
                    contexts[msg.Chat.Id].State = State.Start;
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
                    contexts[msg.Chat.Id].State = State.SearchBookByTitle;


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
                    contexts[msg.Chat.Id].State = State.Start;

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
            try
            {
                List<OLWork>? works = await openLibraryService.SearchBookByTitle(msg.Text);
                if (works != null)
                {
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.SelectMoreApproriate,
                            replyMarkup: Inline.CreateBookHeadsInlineKeyboard(works));
                    contexts[msg.Chat.Id].State = State.Search;
                }
            }
            catch (Exception) { }

            if (contexts[msg.Chat.Id].State == State.SearchBookByTitle)
            {
                contexts[msg.Chat.Id].State = State.Search;
            }
        }

        public async void HandleOnSearchBookByGenreState(Message msg)
        {
            await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: "Выберите жанр",
                            replyMarkup: Inline.ChoiceGenreInlineMarkup);
            contexts[msg.Chat.Id].State = State.Search;

        }

        public async void HandleOnSearchBookByRateState(Message msg)
        {
            await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: "Книги отсортированные по рейтингу",
                            replyMarkup: Reply.SearchMenuReplyMarkup);
            contexts[msg.Chat.Id].State = State.Search;
        }
    }
}
