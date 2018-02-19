using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace MajyoBot.Feature.Currency
{
    public class GoogleFinancialWrapper
    {
        readonly static Dictionary<string, string> CurrencyAlias = new Dictionary<string, string>
        {
            {"rmb", "cny" },
        };

        string ConvertUri(string from, string to, decimal amount = 1)
        {
            return $@"https://finance.google.com/finance/converter?from={from.ToLower()}&to={to.ToLower()}&a={amount}";
        }

        string GraphUri(string from, string to)
        {
            return $@"https://www.google.com/finance/chart?q=CURRENCY:{from.ToUpper()}{to.ToUpper()}&tkr=1&p=5Y";
        }

        public string Convert(string from, string to, decimal amount = 1)
        {
            if (CurrencyAlias.ContainsKey(from.ToLower()))
            {
                from = CurrencyAlias[from.ToLower()];
            }

            if (CurrencyAlias.ContainsKey(to.ToLower()))
            {
                to = CurrencyAlias[to.ToLower()];
            }

            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(ConvertUri(from, to, amount));
            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='currency_converter_result']");
            return node.InnerText.Trim();
        }

        public static GoogleFinancialWrapper Default { get; private set; } = new GoogleFinancialWrapper();
    }
}
