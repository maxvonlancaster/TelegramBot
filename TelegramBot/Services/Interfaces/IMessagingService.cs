using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.Services.Interfaces
{
    public interface IMessagingService
    {
        void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e);
    }
}
