using BooksSearchTelegramBot;
using BooksSearchTelegramBot.Handlers;
using BooksSearchTelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;


namespace BooksSearchTelegramBot
{
    class Program
    {
        static async Task Main()
        {
            var token = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");

            if (token != null)
            {
                TelegramBotService botService = new TelegramBotService(token);
                await botService.StartBotAsync();
            }
            else
            {
                Console.WriteLine("Token was not found!");
            }
        }
    }
}




