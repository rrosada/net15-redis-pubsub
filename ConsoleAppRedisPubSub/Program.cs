using StackExchange.Redis;
using System;


namespace ConsoleAppRedisPubSub
{
    class Program
    {
        static string _configuration = "40.77.24.62";
        static string _channel = "Perguntas";
        static ConnectionMultiplexer redis = null;

        static void Main(string[] args)
        {
            OpenConnection();
            Listener();

            Console.ReadLine();
        }

        static void OpenConnection()
        {            
            redis = ConnectionMultiplexer.Connect(_configuration);
        }

        static void Listener()
        {
            if (!redis.IsConnected)
                OpenConnection();

            var sub = redis.GetSubscriber();
            sub.Subscribe(_channel, (ch, msg) =>
            {               
                Response(msg.ToString().Split(':')[0]);
            });
        }

        static void Response(string value)
        {            
            IDatabase db = redis.GetDatabase();

            HashEntry[] hashEntries =
            {
                new HashEntry ("RodrigoMarcos", "Response OK")
            };

            db.HashSet(value, hashEntries);
            var sub = redis.GetSubscriber();
        }
    }
}
