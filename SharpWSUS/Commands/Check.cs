using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SharpWSUS.Commands
{
    public class Check : ICommand
    {

        public static string CommandName => "check";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine("[*] Action: Check Update");

            string UpdateID = "";
            string ComputerName = "";

            if (arguments.ContainsKey("/updateid"))
            {
                UpdateID = arguments["/updateid"];
            }

            if (arguments.ContainsKey("/computername"))
            {
                ComputerName = arguments["/computername"];
            }

            Server.GetServerDetails();
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = Connect.FsqlConnection();

            if (!Group.FbGetComputerTarget(sqlComm, ComputerName))
            {
                return;
            }
                
            if (!Status.FbGetUpdateStatus(sqlComm, UpdateID))
            {
                return;
            }

            Console.WriteLine("\r\n[*] Check complete\r\n");
        }
    }
}