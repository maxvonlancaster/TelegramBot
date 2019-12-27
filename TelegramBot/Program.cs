using Autofac;
using System;
using TelegramBot.Services;

namespace TelegramBot
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            ConfigureServices();
        }

        public static void ConfigureServices()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MessagingService>()
                .As<IStartable>()
                .SingleInstance();


            // Configure services
            Container = builder.Build();
            Console.ReadLine();
        }

        // TODO: 1 -> retrieve appsettings.json
        // TODO: 2 -> 
        // TODO: 3 -> 

    }
}
