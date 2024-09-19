using BooksSearchTelegramBot.Keyboards;
using BooksSearchTelegramBot.res;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace BooksSearchTelegramBot.Handlers
{
    internal class TextMessageHandler(TelegramBotClient botClient, FSMContext context)
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
                            text: Strings.MyDefferedMessage,
                            replyMarkup: Reply.SearchMenuReplyMarkup);
                    context.State = State.SearchBookByTitle;
                    break;

                case "💫 Жанр":
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.MyReadedMessage,
                            replyMarkup: Inline.ChoiceGenreInlineMarkup);
                    context.State = State.SearchBookByGenre;
                    break;

                case "⭐ Рейтинг":
                    await botClient.SendTextMessageAsync(
                            chatId: msg.Chat,
                            text: Strings.StartMenuMessage,
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
    }
}
