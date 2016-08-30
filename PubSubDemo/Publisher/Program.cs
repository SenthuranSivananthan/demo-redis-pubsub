using StackExchange.Redis;
using System;
using System.Configuration;
using System.Threading;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["Redis.ConnectionString"]);

            RedisChannel redisChannel = new RedisChannel("CachedDataRefresh", RedisChannel.PatternMode.Literal);
            ISubscriber subscriber = connection.GetSubscriber();

            while (true)
            {
                var message = string.Format("Hello-{0}", DateTime.Now.ToString("hh:mm:ss"));
                subscriber.Publish(redisChannel, message);

                Console.WriteLine("Published: {0}", message);

                Thread.Sleep(500);
            }
        }
    }
}
