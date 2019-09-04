using Autofac;
using System;

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

            // Configure services
            Container = builder.Build();
        }
    }
}
