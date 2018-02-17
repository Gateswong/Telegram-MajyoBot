using System;
using System.Collections.Generic;
using MajyoBot.MessageHandler;
using Microsoft.Extensions.CommandLineUtils;
using System.Linq;

namespace MajyoBot
{
    class Program
    {
        private static CommandLineApplication app;

        static Program() 
        {
            app = new CommandLineApplication();
        }

        static void Main(string[] args)
        {
            RegisterCommands(RollHandler.Commands, RollHandler.Program);
        }

        static void RegisterCommand(string command, Action<CommandLineApplication> program) {
            app.Command(command, program);
        }

        static void RegisterCommands(List<string> commands, Action<CommandLineApplication> program) {
            commands.ForEach(x => app.Command(x, program));
        }
    }
}
