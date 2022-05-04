using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SharpWSUS.Commands
{
    public class Approve : ICommand
    {

        public static string CommandName => "approve";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine("[*] Action: Approve Update");

            string UpdateID = "";
            string ComputerName = "";
            string GroupName = "InjectGroup";
            string Approver = "WUS Server";
            Group.GroupExists = false;

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

            if (arguments.ContainsKey("/approver"))
            {
                Approver = arguments["/approver"];
            }

            Server.GetServerDetails();
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = Connect.FsqlConnection();

            ClGuid.GenerateTargetGroupGUID();

            if (!Group.FbGetComputerTarget(sqlComm, ComputerName))
            {
                return;
            }

            if (!Group.FbGetGroupID(sqlComm, GroupName))
            {
                return;
            }

            Console.WriteLine("Group Exists = {0}", Group.GroupExists);
            if (Group.GroupExists == false)
            {
                if (!Group.FbCreateGroup(sqlComm, GroupName))
                {
                    return;
                }
            }

            if (!Group.FbAddComputerToGroup(sqlComm, Server.sTargetComputerTargetID))
            {
                return;
            }

            if (!Status.FbApproveUpdate(sqlComm, UpdateID, Approver))
            {
                return;
            }

            Console.WriteLine("\r\n[*] Approve complete\r\n");
            return;
        }
    }
}
