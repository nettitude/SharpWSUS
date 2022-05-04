using System.Collections.Generic;

namespace SharpWSUS.Commands
{
    public interface ICommand
    {
        void Execute(Dictionary<string, string> arguments);
    }
}