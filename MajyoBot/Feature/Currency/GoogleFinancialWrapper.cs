using System;
using HtmlAgilityPack;

namespace MajyoBot.Feature.Currency
{
    public class GoogleFinancialWrapper
    {
        public GoogleFinancialWrapper()
        {
        }

        public string ConvertUri(string from, string to, decimal amount=1) 
        {
            return $@"https://finance.google.com/finance/converter?from={from.ToLower()}&to={to.ToLower()}&a={amount}";
        }

        public string GraphUri(string from, string to) 
        {
            return $@"https://www.google.com/finance/chart?q=CURRENCY:{from.ToUpper()}{to.ToUpper()}&tkr=1&p=5Y";
        }

        public string Convert(string from, string to, decimal amount=1) 
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(ConvertUri(from, to, amount));
            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='currency_converter_result']");
            return node.InnerText;
        }
    }
}
