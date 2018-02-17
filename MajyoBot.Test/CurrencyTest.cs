using System;
using System.Diagnostics;
using MajyoBot.Feature.Currency;
using Xunit;

namespace MajyoBot.Test
{
    public class CurrencyTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(24.5)]
        public void TestConvertUsdToCny(decimal amount)
        {
            GoogleFinancialWrapper financial = new GoogleFinancialWrapper();
            string result = financial.Convert("USD", "CNY", amount);
            Debug.WriteLine(result);
            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        [Theory]
        [InlineData(5)]
        public void TestConvertUsdToRmb(decimal amount)
        {
            GoogleFinancialWrapper financial = new GoogleFinancialWrapper();
            string result = financial.Convert("USD", "RMB", amount);
            Debug.WriteLine(result);
            Assert.False(string.IsNullOrWhiteSpace(result));
        }
    }
}
