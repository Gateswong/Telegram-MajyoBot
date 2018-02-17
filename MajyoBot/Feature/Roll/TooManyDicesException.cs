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
                return $@"{number} exceeded maximum dice limit {Roll.MAX_DICE_COUNT}!";
            }
        }
    }
}
