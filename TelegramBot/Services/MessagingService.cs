using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBot.Services.Interfaces;
//using GemBox.Presentation;
using TelegramBot.Constants;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Syncfusion.Presentation;
using Syncfusion.OfficeChartToImageConverter;
using Syncfusion.Drawing;
using Telegram.Bot.Types.InputFiles;
using Syncfusion.PresentationRenderer;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Services
{
    public class MessagingService : IStartable, IMessagingService
    {
        private static readonly TelegramBotClient Bot = 
            new TelegramBotClient("955563107:AAFBR8hVPedVsmCAy9JJ4C_DZTlzbDxjJzQ");

        //public static SlideCollection slides;

        public static int presntationId = -1;

        public void Start()
        {
            var cts = new CancellationTokenSource();
            var stoppingToken = cts.Token;
            Bot.OnMessage += Bot_OnMessage;

            Task.Run(() => {
                Bot.StartReceiving(cancellationToken: stoppingToken);
                Console.WriteLine("Recieving started");

                while (!stoppingToken.IsCancellationRequested) 
                {
                    Bot.GetUpdatesAsync();
                }
            });
            //ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                var senderId = e.Message.From.Id;
                string text = e.Message.Text;
                Console.WriteLine(text);
                int i;
                if (int.TryParse(text, out i)) 
                {
                    presntationId = i;
                    IPresentation pptxDoc = Presentation.Open("../../../Files/" + Files.FileNames[i]);
                    pptxDoc.PresentationRenderer = new PresentationRenderer();
                    Stream image = pptxDoc.Slides[0].ConvertToImage(ExportImageFormat.Png);

                    InlineKeyboardButton button = new InlineKeyboardButton();
                    button.CallbackData = $"{senderId},: {i},: {0}";
                    button.Text = "Next page";

                    InlineKeyboardMarkup markup = new InlineKeyboardMarkup(button);

                    InputOnlineFile file = new InputOnlineFile(image);
                    Bot.SendPhotoAsync(senderId, file, replyMarkup: markup);
                }
            }
        }


    }
}
