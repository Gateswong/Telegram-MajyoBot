using System;
using System.Collections.Generic;
using Microsoft.Extensions.CommandLineUtils;

namespace MajyoBot.MessageHandler
{
    public static class RollHandler
    {
        public static List<string> Commands => new List<string>
        { 
            "roll", 
            "r", 
        }; 

        public static void Program(CommandLineApplication app) 
        {
            
        }
    }
}
