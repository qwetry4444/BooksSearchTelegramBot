using BooksSearchTelegramBot.Keyboards;
using BooksSearchTelegramBot.res;
using BooksSearchTelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace BooksSearchTelegramBot.Handlers
{
    internal class TextMessageHandler(TelegramBotClient botClient, FSMContext context, OpenLibraryService openLibraryService)
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
                            text: Strings.MyMenuMessage,
                            replyMarkup: Reply.SearchMenuReplyMarkup);
                    context.State = State.Search;
                    break;
            }
        }

        public async void HandleOnMyState(Message msg)
        {
            switch (msg.Text)
            {
                case "📚 Отложенные":
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.MyDefferedMessage,
                            replyMarkup: Reply.MyMenuReplyMarkup);
                    break;

                case "✅ Прочитанные":
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.MyReadedMessage,
                            replyMarkup: Reply.MyMenuReplyMarkup);
                    break;

                case "⬅ Назад":
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.StartMenuMessage,
                            replyMarkup: Reply.StartMenuReplyMarkup);
                    context.State = State.Start;
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
                    context.State = State.SearchBookByGenre;
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
            }
        }

        public async void HandleOnSearchBookByTitleState(Message msg)
        {
            await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: "Ваша книга:",
                            replyMarkup: Reply.SearchMenuReplyMarkup);
            context.State = State.Search;
            openLibraryService.SearchBookByTitle(msg.Text);
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
