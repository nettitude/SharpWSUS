using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SharpWSUS.Commands
{
    public class Delete : ICommand
    {

        public static string CommandName => "delete";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine("[*] Action: Delete Update");

            string UpdateID = "";
            string ComputerName = "";
            string GroupName = "InjectGroup";
            bool KeepGroup = false;

            if (arguments.ContainsKey("/updateid"))
            {
                UpdateID = arguments["/updateid"];
            }

            if (arguments.ContainsKey("/computername"))
            {
                ComputerName = arguments["/computername"];
            }

            if (arguments.ContainsKey("/groupname"))
            {
                GroupName = arguments["/groupname"];
            }

            if (arguments.ContainsKey("/keepgroup"))
            {
                KeepGroup = true;
            }

            Server.GetServerDetails();
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = Connect.FsqlConnection();


            if (!Status.FbDeleteUpdate(sqlComm, UpdateID))
            {
                return;
            }

            if (ComputerName != "")
            {
                if (!Group.FbGetComputerTarget(sqlComm, ComputerName))
                {
                    return;
                }

                if (!Group.FbGetGroupID(sqlComm, GroupName))
                {
                    return;
                }

                if (!Group.FbRemoveComputerFromGroup(sqlComm, Server.sTargetComputerTargetID))
                {
                    return;
                }

                if (KeepGroup == false)
                {
                    if (!Group.FbRemoveGroup(sqlComm))
                    {
                        return;
                    }
                }
            }

            Console.WriteLine("\r\n[*] Delete complete\r\n");

        }
    }
}