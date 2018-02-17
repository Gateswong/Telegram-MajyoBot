using System;
using System.Collections.Generic;
using System.Linq;
namespace MajyoBot.Feature.Roll
{
    public class RandomSelect
    {
        public RandomSelect(string title, IReadOnlyList<string> choices)
        {
            Title = title;
            Choices = choices;
        }

        public readonly string Title;
        public readonly IReadOnlyList<string> Choices;

        public string Next() 
        {
            Random random = new Random();
            return Choices[random.Next(Choices.Count)];
        }
    }
}
