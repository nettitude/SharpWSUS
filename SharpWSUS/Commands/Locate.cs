using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SharpWSUS.Commands
{
    public class Locate : ICommand
    {

        public static string CommandName => "locate";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine("[*] Action: Locate WSUS Server");

            Enum.FbGetWSUSServer();

            Console.WriteLine("\r\n[*] Locate complete\r\n");
        }
    }
}