using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace TvShowBot
{
    class Program
    {
        private static string token { get; set; } = "5752824603:AAGazbrs8bSWeRmwOlQoKM-AY_gsN52R_So";
        public static TelegramBotClient client;

        public static void Main(string[] args)
        {
            Start();
        }

        static void Start()
        {
            client = new TelegramBotClient(token);
            using var cts = new CancellationTokenSource();
            ReceiverOptions receiverOptions = new ReceiverOptions() { AllowedUpdates = { } };
            client.StartReceiving(Handlers.HandleUpdateAsync, Handlers.HandleErrorAsync, receiverOptions, cts.Token);
            Console.WriteLine("Bot is running successfully");
            Console.ReadLine();
            cts.Cancel();
        }
    }
}


