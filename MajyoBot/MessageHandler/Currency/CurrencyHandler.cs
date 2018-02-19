using System;
using System.Collections.Generic;
using System.Linq;
using MajyoBot.Utility;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text;
using MajyoBot.Feature.Currency;

namespace MajyoBot.MessageHandler.Currency
{
    public class CurrencyHandler : IMessageHandler
    {
        public string Description => "汇率查询";

        public bool HandleMessage(TelegramBotClient bot, Message message)
        {
            if (!IsValidMessage(message)) { return false; }

            string messageBody = message.Text.TrimFirstWord();
            if (string.IsNullOrWhiteSpace(messageBody)) 
            {
                ShowHelpText(bot, message);
                return true;
            }

            if (!Parse(messageBody, out string from, out string to, out decimal amount))
            {
                ShowHelpText(bot, message);
                return true;
            }

            bot.SendTextMessageAsync(
                message.Chat.Id,
                GoogleFinancialWrapper.Default.Convert(from, to, amount),
                replyToMessageId: message.MessageId
            );

            return true;
        }

        bool Parse(string messageBody, out string from, out string to, out decimal amount)
        {
            Match match = new Regex(@"^(?<amount>\d+(\.\d+)?)?\s*(?<from>\w+)\s*(to)?\s*(?<to>\w+)")
                .Match(messageBody);

            if (!match.Success)
            {
                from = string.Empty;
                to = string.Empty;
                amount = 0m;
                return false;
            }

            from = match.Groups["from"].Value;
            to = match.Groups["to"].Value;
            amount = match.Groups["amount"].Success ? decimal.Parse(match.Groups["amount"].Value) : 1m;
            return true;
        }

        void ShowHelpText(TelegramBotClient bot, Message message)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(@"你可以试试这些方式：");

            builder.AppendLine(@"/currency usd rmb");
            builder.AppendLine(@"/currency usd to rmb");
            builder.AppendLine(@"/currency 100 usd rmb");
            builder.AppendLine(@"/currency 100 usd to rmb");

            string replyMessage = builder.ToString();
            bot.SendTextMessageAsync(
                message.Chat.Id,
                replyMessage,
                Telegram.Bot.Types.Enums.ParseMode.Markdown,
                replyToMessageId: message.MessageId
            );
        }

        bool IsValidMessage(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.TextMessage) { return false; }

            return validCommands.Contains(message.Text.Split(" ").FirstOrDefault());
        }

        static readonly List<string> validCommands = new List<string>()
        {
            "/currency",
            "/cur",
        };
    }
}
