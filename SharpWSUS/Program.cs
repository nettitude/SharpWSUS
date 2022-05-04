using System;
using System.Collections.Generic;
using SharpWSUS.Args;

namespace SharpWSUS
{
    class Program
    {
        private static void MainExecute(string commandName, Dictionary<string, string> parsedArgs)
        {
            // main execution logic

            Info.ShowLogo();

            try
            {
                var commandFound = new CommandCollection().ExecuteCommand(commandName, parsedArgs);

                // show the usage if no commands were found for the command name
                if (commandFound == false)
                    Info.ShowUsage();
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\n[!] Unhandled SharpWSUS exception:\r\n");
                Console.WriteLine(e);
            }
        }

        public static void Main(string[] args)
        {
            // try to parse the command line arguments, show usage on failure and then bail
            var parsed = ArgumentParser.Parse(args);
            if (parsed.ParsedOk == false)
            {
                Info.ShowLogo();
                Info.ShowUsage();
                return;
            }

            var commandName = args.Length != 0 ? args[0] : "";

            MainExecute(commandName, parsed.Arguments);
            
        }
    }
}
