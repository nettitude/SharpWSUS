using System;
using System.Collections.Generic;
using SharpWSUS.Commands;

namespace SharpWSUS.Args
{
    public class CommandCollection
    {
        private readonly Dictionary<string, Func<ICommand>> _availableCommands = new Dictionary<string, Func<ICommand>>();

        // How To Add A New Command:
        //  1. Create your command class in the Commands Folder
        //      a. That class must have a CommandName static property that has the Command's name
        //              and must also Implement the ICommand interface
        //      b. Put the code that does the work into the Execute() method
        //  2. Add an entry to the _availableCommands dictionary in the Constructor below.

        public CommandCollection()
        {
            _availableCommands.Add(Create.CommandName, () => new Create());
            _availableCommands.Add(Approve.CommandName, () => new Approve());
            _availableCommands.Add(Check.CommandName, () => new Check());
            _availableCommands.Add(Delete.CommandName, () => new Delete());
            _availableCommands.Add(Inspect.CommandName, () => new Inspect());
            _availableCommands.Add(Locate.CommandName, () => new Locate());

        }

        public bool ExecuteCommand(string commandName, Dictionary<string, string> arguments)
        {
            bool commandWasFound;

            if (string.IsNullOrEmpty(commandName) || _availableCommands.ContainsKey(commandName) == false)
                commandWasFound= false;
            else
            {
                // Create the command object 
                var command = _availableCommands[commandName].Invoke();
                
                // and execute it with the arguments from the command line
                command.Execute(arguments);

                commandWasFound = true;
            }

            return commandWasFound;
        }
    }
}