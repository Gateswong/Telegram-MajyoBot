using System;
using System.Collections.Generic;
using MajyoBot.MessageHandler.Roll;
using Microsoft.Extensions.CommandLineUtils;
using System.Linq;
using Telegram.Bot;
using MajyoBot.Utility;
using MajyoBot.MessageHandler;
using System.Threading;
using System.Text;

namespace MajyoBot
{
    class Program
    {
        static TelegramBotClient bot;
        static List<IMessageHandler> messageHandlers;

        static void Main(string[] args)
        {
            InitMessageHandlers();
            bot = new TelegramBotClient(AppConfiguration.TelegramBot.ApiToken);
            bot.OnMessage += Bot_OnMessage;

            try
            {
                bot.StartReceiving();
                while (true) { Thread.Sleep(1000); }
            }
            catch (ApplicationException)
            {
                bot.StopReceiving();
            }
        }

        static void InitMessageHandlers()
        {
            messageHandlers = new List<IMessageHandler>();        
            messageHandlers.Add(new RollHandler());
        }

        static async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            try 
            {
                foreach (var handler in messageHandlers) 
                {
                    if (handler.HandleMessage(bot, e.Message)) { break; }
                }
            }
            catch (BotException ex) 
            {
                await bot.SendTextMessageAsync(
                    e.Message.Chat.Id,
                    ex.Message,
                    replyToMessageId: e.Message.MessageId
                );
            }
            catch (Exception ex) 
            {
                StringBuilder unexpectedErrorMessageBuilder = new StringBuilder();
                unexpectedErrorMessageBuilder.AppendLine($@"呜……发生了我没有预料到的错误！");
                if (AppConfiguration.Behavior.ShowFullError) 
                {
                    unexpectedErrorMessageBuilder.AppendLine();
                    unexpectedErrorMessageBuilder.AppendLine($@"------");
                    unexpectedErrorMessageBuilder.AppendLine();
                    unexpectedErrorMessageBuilder.AppendLine(ex.ToString());
                }

                await bot.SendTextMessageAsync(
                    e.Message.Chat.Id,
                    unexpectedErrorMessageBuilder.ToString(),
                    replyToMessageId: e.Message.MessageId
                );
            }
        }
    }
}
