using BooksSearchTelegramBot;
using BooksSearchTelegramBot.Handlers;
using BooksSearchTelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;


using var cts = new CancellationTokenSource();
Console.WriteLine(Environment.CurrentDirectory);
var token = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");

if (token != null)
{
    var bot = new TelegramBotClient(token, cancellationToken: cts.Token);
    var me = await bot.GetMeAsync();

    var FSMContext = new FSMContext();
    var openLibraryService = new OpenLibraryService();

    var messageHandler = new TextMessageHandler(bot, FSMContext, openLibraryService);

    bot.OnError += OnError;
    bot.OnMessage += messageHandler.OnMessage;
    bot.OnUpdate += OnUpdate;
    Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
    Console.ReadLine();
    cts.Cancel(); // stop the bot

    // method to handle errors in polling or in your OnMessage/OnUpdate code
    async Task OnError(Exception exception, HandleErrorSource source)
    {
        Console.WriteLine(exception); // just dump the exception to the console
    }

    // method that handle other types of updates received by the bot:
    async Task OnUpdate(Update update)
    {
        if (update is { CallbackQuery: { } query }) // non-null CallbackQuery
        {
            await bot.AnswerCallbackQueryAsync(query.Id, $"You picked {query.Data}");
            await bot.SendTextMessageAsync(query.Message!.Chat, $"User {query.From} clicked on {query.Data}");
        }
    }
}


