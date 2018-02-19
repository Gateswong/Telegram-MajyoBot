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
                bool isCommandHandled = true;
                foreach (var handler in messageHandlers) 
                {
                    isCommandHandled = handler.HandleMessage(bot, e.Message);
                    if (isCommandHandled) { break; }
                }
                
                if (!isCommandHandled)
                {
                    throw new InvalidCommandException(
                        e.Message.Text.Split(' ', '\n')[0].TrimStart('/'));
                }
            }
            catch (BotException ex) 
            {
                await bot.SendTextMessageAsync(
                    e.Message.Chat.Id,
                    AppConfiguration.Behavior.ShowFullError ? ex.DiagnoseMessage : ex.Message,
                    Telegram.Bot.Types.Enums.ParseMode.Markdown,
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
                    if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
                    {
                        unexpectedErrorMessageBuilder.AppendLine($@"### Original Text ###");
                        unexpectedErrorMessageBuilder.AppendLine();
                        unexpectedErrorMessageBuilder.AppendLine(e.Message.Text);
                        unexpectedErrorMessageBuilder.AppendLine();
                    }
                    unexpectedErrorMessageBuilder.AppendLine($@"### Stacktrace ###");
                    unexpectedErrorMessageBuilder.AppendLine("```");
                    unexpectedErrorMessageBuilder.AppendLine(ex.ToString());
                    unexpectedErrorMessageBuilder.AppendLine("```");
                }

                await bot.SendTextMessageAsync(
                    e.Message.Chat.Id,
                    unexpectedErrorMessageBuilder.ToString(),
                    Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    replyToMessageId: e.Message.MessageId
                );
            }
        }
    }
}
