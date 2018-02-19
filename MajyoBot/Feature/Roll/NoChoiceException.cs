using System;
using System.Collections.Generic;
using System.Text;

namespace MajyoBot.Feature.Roll
{
    public class NoChoiceException : BotException
    {
        public override string Message => $@"呜……你要给几个选择啦……";
    }
}
