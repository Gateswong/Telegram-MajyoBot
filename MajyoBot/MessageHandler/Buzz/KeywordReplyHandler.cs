using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MajyoBot.MessageHandler.Buzz
{
    public class KeywordReplyHandler : IMessageHandler
    {
        public string Description => "根据关键词回复特定内容";

        public bool HandleMessage(TelegramBotClient bot, Message message)
        {
            if (message.Text.Contains("傲娇") && message.Text.ToLower().Contains("bot")) 
            {
                bot.SendStickerAsync(
                    message.Chat.Id,
                    new FileToSend("CAADAQAD_gAD1i-bBtxkxTuiGVO3Ag"),
                    replyToMessageId: message.MessageId
                );

                return true;
            }


            if (message.Text.Contains("傲娇") && message.Text.Contains("群主"))
            {
                bot.SendStickerAsync(
                    message.Chat.Id,
                    new FileToSend("CAADAQADCgEAAtYvmwYs_jIMzgABDtUC"),
                    replyToMessageId: message.MessageId
                );

                return true;
            }

            return false;
        }
    }
}
