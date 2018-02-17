using System;
using System.Collections.Generic;
using System.Numerics;
namespace MajyoBot.Feature.Roll
{
    public class Roll
    {
        public bool IsNumbersTooLarge { get; private set; } = false;

        public static int MAX_DICE_RESULT_COUNT = 20;
        public static int MAX_DICE_COUNT = 10000;

        private List<BigInteger> diceResults;
        public List<BigInteger> DiceResults
        {
            get
            {
                if (IsNumbersTooLarge) { return null; }
                return diceResults;
            }
        }

        public Roll(BigInteger number, BigInteger faces, BigInteger add)
        {
            Number = number;
            Faces = faces;
            Add = add;
            IsNumbersTooLarge = (Number > MAX_DICE_RESULT_COUNT);

            PerformRoll();
        }

        public Roll(BigInteger number, BigInteger faces) : this(number, faces, 0)
        {
        }

        public Roll(BigInteger faces) : this(1, faces, 0)
        {
        }

        public BigInteger Number { get; private set; }
        public BigInteger Faces { get; private set; }
        public BigInteger Add { get; private set; }

        public BigInteger RollResult { get; private set; }

        private void PerformRoll() 
        {
            if (Number > MAX_DICE_COUNT) 
            {
                throw new TooManyDicesException(Number);
            }

            RollResult = Add;

            Random random = new Random();
            diceResults = new List<BigInteger>();

            if (Faces == 1) { RollResult += Number; }
            else 
            {
                byte[] bytes = Faces.ToByteArray();
                for (int i = 0; i < Number; i++)
                {
                    random.NextBytes(bytes);
                    bytes[bytes.Length - 1] &= (byte)0x7F;
                    BigInteger roll = new BigInteger(bytes) % Faces + 1;
                    if (Number <= MAX_DICE_RESULT_COUNT)
                    {
                        DiceResults.Add(roll);
                    }
                    RollResult += roll;
                }
            }
        }
    }
}
