using StackExchange.Redis;
using System;
using System.Configuration;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["Redis.ConnectionString"]);

            // By default, messages are processed sequentially.
            // See https://github.com/StackExchange/StackExchange.Redis/blob/master/Docs/PubSubOrder.md
            connection.PreserveAsyncOrder = true;

            ISubscriber subscriber = connection.GetSubscriber();

            RedisChannel redisChannel = new RedisChannel("CachedDataRefresh", RedisChannel.PatternMode.Literal);
            subscriber.SubscribeAsync(
                                            redisChannel,
                                            (channel, value) =>
                                            {
                                                Console.WriteLine("Received Notification - Channel: {0}, Value: {1}", channel, value);
                                            });


            Console.WriteLine("Listening for new notifications.  Press any key to terminate.");
            Console.ReadLine();
        }
    }
}
