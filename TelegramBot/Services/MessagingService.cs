using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace TelegramBot.Services
{
    public class MessagingService : IStartable
    {
        private static readonly TelegramBotClient Bot = 
            new TelegramBotClient("955563107:AAFBR8hVPedVsmCAy9JJ4C_DZTlzbDxjJzQ");

        public void Start()
        {
            Bot.OnMessage += Bot_OnMessage;
            Bot.StartReceiving();
        }

        private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                Console.WriteLine(e.Message.Text);
            }
        }
    }
}
