using BooksSearchTelegramBot.Database;
using BooksSearchTelegramBot.Database.Repositories;
using BooksSearchTelegramBot.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace BooksSearchTelegramBot.Services
{
    public class TelegramBotService
    {
        private readonly TelegramBotClient bot;
        private readonly CancellationTokenSource cts;

        private readonly OpenLibraryService openLibraryService;

        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserReadedBookRepository userReadedBookRepository;
        private readonly UserDeferredBookRepository userDeferredBookRepository;
        private readonly DbService dbService;

        private readonly Dictionary<long, FSMContext> fSMContexts;
        private readonly TextMessageHandler textMessageHandler;
        private readonly InlineQueryHandler inlineQueryHandler;

        public TelegramBotService(string token)
        {
            cts = new CancellationTokenSource();
            bot = new TelegramBotClient(token, cancellationToken: cts.Token);

            openLibraryService = new OpenLibraryService();

            applicationDbContext = new ApplicationDbContext();
            userReadedBookRepository = new UserReadedBookRepository(applicationDbContext);
            userDeferredBookRepository = new UserDeferredBookRepository(applicationDbContext);
            dbService = new DbService(userReadedBookRepository, userDeferredBookRepository);

            fSMContexts = [];
            textMessageHandler = new TextMessageHandler(bot, fSMContexts, openLibraryService, dbService);
            inlineQueryHandler = new InlineQueryHandler(bot, openLibraryService, dbService);
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
