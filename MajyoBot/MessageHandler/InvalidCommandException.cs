using System;
namespace MajyoBot.MessageHandler
{
    public class InvalidCommandException : BotException
    {
        public InvalidCommandException(string command)
        {
            this.Command = command;
        }

        public string Command { get; private set; }

        public override string Message
        {
            get
            {
                return $@"{Command}……唔……我不能理解你的意思呢";
            }
        }
    }
}
