using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SharpWSUS.Commands
{
    public class Inspect : ICommand
    {

        public static string CommandName => "inspect";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine("[*] Action: Inspect WSUS Server");

            Server.GetServerDetails();
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = Connect.FsqlConnection();

            Enum.FbGetWSUSConfigSQL(sqlComm);

            Enum.FbEnumAllComputers(sqlComm);

            Enum.FbEnumDownStream(sqlComm);

            Enum.FbEnumGroups(sqlComm);

            Console.WriteLine("\r\n[*] Inspect complete\r\n");
        }
    }
}