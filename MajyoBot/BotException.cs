using System;
using System.Text;

namespace MajyoBot
{
    public abstract class BotException : Exception
    {
        public BotException()
        {
            Id = Guid.NewGuid();
        }

        public override string Message { get; }
        public string DiagnoseMessage
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(Message);
                builder.AppendLine();
                builder.AppendLine($@"### Stacktrace ###");
                builder.AppendLine("```");
                builder.AppendLine(ToString());
                builder.AppendLine("```");
                return builder.ToString();
            }
        }
        public Guid Id { get; protected set; }
    }
}
