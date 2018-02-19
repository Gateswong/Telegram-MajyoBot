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
                return $@"唔……我不能理解""{Command}""的意思呢";
            }
        }
    }
}
