using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MajyoBot.MessageHandler
{
    public interface IMessageHandler
    {
        string Description { get; }
        bool HandleMessage(TelegramBotClient bot, Message message);
    }
}
