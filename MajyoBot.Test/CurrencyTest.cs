using System;
using MajyoBot.Feature.Currency;
using Xunit;

namespace MajyoBot.Test
{
    public class CurrencyTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(24.5)]
        public void TestConvertUsdToRmb(decimal amount)
        {
            GoogleFinancialWrapper financial = new GoogleFinancialWrapper();
            string result = financial.Convert("USD", "RMB", amount);
            Console.WriteLine(result);
        }
    }
}
