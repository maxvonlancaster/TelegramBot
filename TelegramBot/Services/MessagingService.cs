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
using Telegram.Bot.Args;

namespace TelegramBot.Services
{
    public class MessagingService : IStartable, IMessagingService
    {
        private static readonly TelegramBotClient Bot = 
            new TelegramBotClient("955563107:AAFBR8hVPedVsmCAy9JJ4C_DZTlzbDxjJzQ");

        public static List<IPresentation> presentations = new List<IPresentation>();

        public void Start()
        {
            var cts = new CancellationTokenSource();
            var stoppingToken = cts.Token;
            Bot.OnMessage += Bot_OnMessage;
            Bot.OnCallbackQuery += Bot_OnCallback;

            for (int i = 0; i < Files.FileNames.Length; i++) 
            {
                IPresentation pptxDoc = Presentation.Open("../../../Files/" + Files.FileNames[i]);
                pptxDoc.PresentationRenderer = new PresentationRenderer();
                presentations.Add(pptxDoc);
            }

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
                    Stream image = presentations[i].Slides[0].ConvertToImage(ExportImageFormat.Png);

                    InlineKeyboardButton button = new InlineKeyboardButton();
                    button.CallbackData = $"{senderId}:{i}:{0}";
                    button.Text = "Next page";

                    InlineKeyboardMarkup markup = new InlineKeyboardMarkup(button);

                    InputOnlineFile file = new InputOnlineFile(image);

                    Bot.SendPhotoAsync(senderId, file, replyMarkup: markup);
                }
            }
        }

        private void Bot_OnCallback(object sender, CallbackQueryEventArgs e)
        {
            string[] data = e.CallbackQuery.Data.Split(':');
            int presentationId;
            int slideId;
            if (int.TryParse(data[1], out presentationId) && int.TryParse(data[2], out slideId)) 
            {
                slideId++;
                Stream image = presentations[presentationId].Slides[slideId].ConvertToImage(ExportImageFormat.Png);

                InlineKeyboardButton button = new InlineKeyboardButton();
                button.CallbackData = $"{data[0]}:{presentationId}:{slideId}";
                button.Text = "Next page";

                InlineKeyboardMarkup markup = new InlineKeyboardMarkup(button);

                InputOnlineFile file = new InputOnlineFile(image);

                Bot.SendPhotoAsync(data[0], file, replyMarkup: markup);
            }
        }
    }
}
