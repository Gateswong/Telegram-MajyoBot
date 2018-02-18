using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using MajyoBot.Feature.Roll;
using Microsoft.Extensions.CommandLineUtils;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MajyoBot.MessageHandler.Roll
{
    public class RollHandler : IMessageHandler
    {
        public string Description => "骰子！";

        public bool HandleMessage(TelegramBotClient bot, Message message)
        {
            if (!IsValidMessage(message)) { return false; }

            string messageBody = message.Text.Substring(
                message.Text.IndexOf(" ", StringComparison.Ordinal)).Trim();

            if (string.IsNullOrEmpty(messageBody)) {
                ShowHelpText(bot, message);
                return true;
            }

            // Roll dices
            if (RollDices(bot, message, messageBody)) 
            {
                return true;
            }

            if (RollSelection(bot, message, messageBody)) 
            {
                return true;
            }

            return true;
        }

        bool RollDices(TelegramBotClient bot, Message message, string messageBody)
        {
            messageBody = messageBody.Replace(" ", string.Empty);
            BigInteger number = 1, add = 0;

            if (!BigInteger.TryParse(messageBody, out BigInteger faces)) 
            {
                Match match = new Regex(@"(?<number>\d+)[dD](?<faces>\d+)(?<add>\+\d+)?").Match(messageBody);
                if (!match.Success) 
                {
                    return false;
                }

                number = BigInteger.Parse(match.Groups["number"].Value);
                faces = BigInteger.Parse(match.Groups["faces"].Value);
                add = match.Groups["add"].Success ?
                           BigInteger.Parse(match.Groups["add"].Value) : 0;
            }

            Feature.Roll.Roll roll = new Feature.Roll.Roll(number, faces, add);
            bot.SendTextMessageAsync(
                message.Chat.Id,
                RollDicesMessage(roll),
                replyToMessageId: message.MessageId
            );

            return true;
        }

        private string RollDicesMessage(Feature.Roll.Roll roll)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("骰子骰子咕噜转…………");

            if (roll.Number > 1 && roll.DiceResults != null) 
            {
                builder.AppendLine("`" + string.Join(" + ", roll.DiceResults) + "`");
            }

            builder.AppendLine($"结果是：{roll.RollResult}");
            return builder.ToString();
        }

        bool RollSelection(TelegramBotClient bot, Message message, string messageBody)
        {
            string[] lines = messageBody.Split('\n');

            if (!lines[0].StartsWith("#", StringComparison.Ordinal)) 
            {
                return false;
            }
            string title = lines[0].Substring(1);
            RandomSelect randomSelect = new RandomSelect(title, lines.Skip(1).ToArray());
            bot.SendTextMessageAsync(
                message.Chat.Id,
                RollSelectionMessage(randomSelect),
                replyToMessageId: message.MessageId
            );

            return true;
        }

        private string RollSelectionMessage(RandomSelect randomSelect)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($@"骰子骰子咕噜转…………");
            if (!string.IsNullOrWhiteSpace(randomSelect.Title))
            {
                builder.AppendLine(randomSelect.Title);
            }
            else 
            {
                builder.Append($@"结果是：""");
            }
            builder.Append($@"{randomSelect.Next()}");
            if (!string.IsNullOrWhiteSpace(randomSelect.Title)) 
            {
                builder.AppendLine();
            }

            return builder.ToString();
        }

        bool IsValidMessage(Message message) 
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.TextMessage) 
            {
                return false;
            }

            string command = message.Text.Split(" ").FirstOrDefault()?.ToLower();
            return validCommands.Contains(command);
        }

        void ShowHelpText(TelegramBotClient bot, Message message, bool showError=false) 
        {
            StringBuilder builder = new StringBuilder();
            if (showError) 
            {
                // builder.AppendLine("！");
            }

            builder.AppendLine(@"例子1：");
            builder.AppendLine(@"`/roll 2d20+4`");
            builder.AppendLine();
            builder.AppendLine(@"掷两个D20骰子，结果再加4.");
            builder.AppendLine();

            builder.AppendLine(@"例子2：");
            builder.AppendLine(@"```");
            builder.AppendLine(@"/roll #吃点什么好呢");
            builder.AppendLine(@"苹果");
            builder.AppendLine(@"面包");
            builder.AppendLine(@"橘子");
            builder.AppendLine(@"馒头");
            builder.AppendLine(@"```");
            builder.AppendLine();
            builder.AppendLine(@"在选项中随机选择一项，每一行是一个选项。");

            string replyMessage = builder.ToString();
            bot.SendTextMessageAsync(
                message.Chat.Id,
                replyMessage,
                Telegram.Bot.Types.Enums.ParseMode.Markdown,
                replyToMessageId: message.MessageId
            );
        }

        static List<string> validCommands = new List<string>() 
        {
            "/r",
            "/roll",
        };
    }
}
