using System;
using System.Numerics;

namespace MajyoBot.Feature.Roll
{
    public class TooManyDicesException : ArgumentException
    {
        public TooManyDicesException(BigInteger number)
        {
            this.number = number;
        }

        private BigInteger number;

        public override string Message
        {
            get
            {
                return $@"唔…… {number}个太多了啦！我最多只有{Roll.MAX_DICE_COUNT}个骰子啦!";
            }
        }
    }
}
