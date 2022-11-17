using Nancy.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace TvShowBot
{
    class Program
    {
        private static IDictionary<string, string> _keys;
        public static TelegramBotClient client;

        public static void Main(string[] args)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = File.ReadAllText("TvShowBot/TvShowBot/config.json");
            _keys = serializer.Deserialize<IDictionary<string,string>>(json);

            Start();
        }

        static void Start()
        {
            string bot_token;
            if(!_keys.TryGetValue("bot_token", out bot_token))
            {
                Console.WriteLine("Can't find bot_token");
                Environment.Exit(0);
            }

            client = new TelegramBotClient(bot_token);
            using var cts = new CancellationTokenSource();
            ReceiverOptions receiverOptions = new ReceiverOptions() { AllowedUpdates = { } };
            client.StartReceiving(Handlers.HandleUpdateAsync, Handlers.HandleErrorAsync, receiverOptions, cts.Token);
            Console.WriteLine("Bot is running successfully");
            Console.ReadLine();
            cts.Cancel();
        }

        public static IDictionary<string, string> GetKeys()
        {
            return _keys;
        }
    }
}


