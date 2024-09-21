using BooksSearchTelegramBot.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace BooksSearchTelegramBot.Services
{
    public class TelegramBotService
    {
        private readonly TelegramBotClient bot;
        private readonly CancellationTokenSource cts;
        private readonly FSMContext fSMContext;
        private readonly OpenLibraryService openLibraryService;
        private readonly TextMessageHandler textMessageHandler;
        private readonly InlineQueryHandler inlineQueryHandler;
        
        public TelegramBotService(string token)
        {
            fSMContext = new FSMContext();
            cts = new CancellationTokenSource();
            bot = new TelegramBotClient(token, cancellationToken: cts.Token);
            openLibraryService = new OpenLibraryService();
            textMessageHandler = new TextMessageHandler(bot, fSMContext, openLibraryService);
            inlineQueryHandler = new InlineQueryHandler(bot, fSMContext, openLibraryService);

        }

        public async Task StartBotAsync()
        {
            var me = await bot.GetMeAsync();

            bot.OnMessage += textMessageHandler.OnMessage;
            bot.OnUpdate += inlineQueryHandler.OnInlineQuery;
            bot.OnError += OnError;

            Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
            Console.ReadLine();

            StopBot();
        }

        public void StopBot()
        {
            cts.Cancel();
            Console.WriteLine("The bot is stopped.");
        }

        async Task OnError(Exception exception, HandleErrorSource source)
        {
            Console.WriteLine(exception); 
        }
    }
}
