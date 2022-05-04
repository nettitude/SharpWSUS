using System;
using System.Text;
using System.Data.SqlClient;
using SharpWSUS.Commands;

public class Build
{
    
    public static bool FbImportUpdate(SqlCommand sqlCommFun)
    {
        System.Data.DataTable dtDataTbl = new System.Data.DataTable();
        SqlDataReader sqldrReader;
        StringBuilder sbUpdate = new StringBuilder();

        sbUpdate.AppendLine(@"declare @iImported int");
        sbUpdate.AppendLine(@"declare @iLocalRevisionID int");
        sbUpdate.AppendLine(@"exec spImportUpdate @UpdateXml=N'");
        sbUpdate.AppendLine(@"<upd:Update xmlns:b=""http://schemas.microsoft.com/msus/2002/12/LogicalApplicabilityRules"" xmlns:pub=""http://schemas.microsoft.com/msus/2002/12/Publishing"" xmlns:cbs=""http://schemas.microsoft.com/msus/2002/12/UpdateHandlers/Cbs"" xmlns:cbsar=""http://schemas.microsoft.com/msus/2002/12/CbsApplicabilityRules"" xmlns:upd=""http://schemas.microsoft.com/msus/2002/12/Update"">");
        sbUpdate.AppendLine("\t" + @"<upd:UpdateIdentity UpdateID=""" + ClGuid.gUpdate + @""" RevisionNumber=""202"" />");
        sbUpdate.AppendLine("\t" + @"<upd:Properties DefaultPropertiesLanguage=""en"" UpdateType=""Software"" Handler=""http://schemas.microsoft.com/msus/2002/12/UpdateHandlers/Cbs"" MaxDownloadSize=""" + ClFile.lSize + @""" MinDownloadSize=""" + ClFile.lSize + @""" PublicationState=""Published"" CreationDate=""" + Create.UpdateDate + @"T00:03:55.912Z"" PublisherID=""395392a0-19c0-48b7-a927-f7c15066d905"">");
        sbUpdate.AppendLine("\t\t" + @"<upd:InstallationBehavior RebootBehavior=""CanRequestReboot"" />");
        sbUpdate.AppendLine("\t\t" + @"<upd:UninstallationBehavior RebootBehavior=""CanRequestReboot"" />");
        sbUpdate.AppendLine("\t" + @"</upd:Properties>");
        sbUpdate.AppendLine("\t" + @"<upd:LocalizedPropertiesCollection>");
        sbUpdate.AppendLine("\t\t" + @"<upd:LocalizedProperties>");
        sbUpdate.AppendLine("\t\t\t" + @"<upd:Language>en</upd:Language>");
        sbUpdate.AppendLine("\t\t\t" + @"<upd:Title>Windows-Update</upd:Title>");
        sbUpdate.AppendLine("\t\t" + @"</upd:LocalizedProperties>");
        sbUpdate.AppendLine("\t" + @"</upd:LocalizedPropertiesCollection>");
        sbUpdate.AppendLine("\t" + @"<upd:ApplicabilityRules>");
        sbUpdate.AppendLine("\t\t" + @"<upd:IsInstalled><b:False /></upd:IsInstalled>");
        sbUpdate.AppendLine("\t\t" + @"<upd:IsInstallable><b:True /></upd:IsInstallable>");
        sbUpdate.AppendLine("\t" + @"</upd:ApplicabilityRules>");
        sbUpdate.AppendLine("\t" + @"<upd:Files>");
        sbUpdate.AppendLine("\t\t" + @"<upd:File Digest=""" + ClFile.sSHA1 + @""" DigestAlgorithm=""SHA1"" FileName=""" + ClFile.sFileName + @""" Size=""" + ClFile.lSize + @""" Modified=""" + Create.UpdateDate + @"T15:26:20.723"">");
        sbUpdate.AppendLine("\t\t\t" + @"<upd:AdditionalDigest Algorithm=""SHA256"">" + ClFile.sSHA256 + @"</upd:AdditionalDigest>");
        sbUpdate.AppendLine("\t\t" + @"</upd:File>");
        sbUpdate.AppendLine("\t" + @"</upd:Files>");
        sbUpdate.AppendLine("\t" + @"<upd:HandlerSpecificData xsi:type=""cmd: CommandLineInstallation"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:pub=""http://schemas.microsoft.com/msus/2002/12/Publishing"">");
        sbUpdate.AppendLine("\t\t" + @"<cmd:InstallCommand Arguments=""" + ClFile.sArgs + @""" Program=""" + ClFile.sFileName + @""" RebootByDefault=""false"" DefaultResult=""Succeeded"" xmlns:cmd=""http://schemas.microsoft.com/msus/2002/12/UpdateHandlers/CommandLineInstallation"">");
        sbUpdate.AppendLine("\t\t\t" + @"<cmd:ReturnCode Reboot=""false"" Result=""Succeeded"" Code=""0"" />");
        sbUpdate.AppendLine("\t\t" + @"</cmd:InstallCommand>");
        sbUpdate.AppendLine("\t" + @"</upd:HandlerSpecificData>");
        sbUpdate.AppendLine(@"</upd:Update>',");
        sbUpdate.AppendLine(@"@UpstreamServerLocalID=1,@Imported=@iImported output,@localRevisionID=@iLocalRevisionID output,@UpdateXmlCompressed=NULL");
        sbUpdate.AppendLine(@"select @iImported,@iLocalRevisionID");
        sqlCommFun.CommandText = sbUpdate.ToString();

        try
        {
            sqldrReader = sqlCommFun.ExecuteReader();
            dtDataTbl.Load(sqldrReader);
            Create.iRevisionID = (int)dtDataTbl.Rows[0][1];

            if (Create.iRevisionID == 0)
            {
                Console.WriteLine("Error importing update");
                sqldrReader.Close();
                return false;
            }

            Console.WriteLine("ImportUpdate");
            Console.WriteLine("Update Revision ID: {0}", Create.iRevisionID);

            sqldrReader.Close();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbImportUpdate.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    public static bool FbPrepareXMLtoClient(SqlCommand sqlCommFun)
    {
        SqlDataReader sqldrReader;
        StringBuilder sbXMLClient = new StringBuilder();
        sbXMLClient.AppendLine(@"exec spSaveXmlFragment '" + ClGuid.gUpdate + @"',202,1,N'&lt;UpdateIdentity UpdateID=""" + ClGuid.gUpdate + @""" RevisionNumber=""202"" /&gt;&lt;Properties UpdateType=""Software"" /&gt;&lt;Relationships&gt;&lt;/Relationships&gt;&lt;ApplicabilityRules&gt;&lt;IsInstalled&gt;&lt;False /&gt;&lt;/IsInstalled&gt;&lt;IsInstallable&gt;&lt;True /&gt;&lt;/IsInstallable&gt;&lt;/ApplicabilityRules&gt;',NULL");
        sbXMLClient.AppendLine(@"exec spSaveXmlFragment '" + ClGuid.gUpdate + @"',202,4,N'&lt;LocalizedProperties&gt;&lt;Language&gt;en&lt;/Language&gt;&lt;Title&gt;Windows-Update&lt;/Title&gt;&lt;/LocalizedProperties&gt;',NULL,'en'");
        sbXMLClient.AppendLine(@"exec spSaveXmlFragment '" + ClGuid.gUpdate + @"',202,2,N'&lt;ExtendedProperties DefaultPropertiesLanguage=""en"" Handler=""http://schemas.microsoft.com/msus/2002/12/UpdateHandlers/CommandLineInstallation"" MaxDownloadSize=""" + ClFile.lSize + @""" MinDownloadSize=""" + ClFile.lSize + @"""&gt;&lt;InstallationBehavior RebootBehavior=""NeverReboots"" /&gt;&lt;/ExtendedProperties&gt;&lt;Files&gt;&lt;File Digest=""" + ClFile.sSHA1 + @""" DigestAlgorithm=""SHA1"" FileName=""" + ClFile.sFileName + @""" Size=""" + ClFile.lSize + @""" Modified=""" + Create.UpdateDate + @"T15:26:20.723""&gt;&lt;AdditionalDigest Algorithm=""SHA256""&gt;" + ClFile.sSHA256 + @"&lt;/AdditionalDigest&gt;&lt;/File&gt;&lt;/Files&gt;&lt;HandlerSpecificData type=""cmd:CommandLineInstallation""&gt;&lt;InstallCommand Arguments=""" + ClFile.sArgs + @""" Program=""" + ClFile.sFileName + @""" RebootByDefault=""false"" DefaultResult=""Succeeded""&gt;&lt;ReturnCode Reboot=""false"" Result=""Succeeded"" Code=""-1"" /&gt;&lt;/InstallCommand&gt;&lt;/HandlerSpecificData&gt;',NULL");
        sqlCommFun.CommandText = sbXMLClient.ToString();
        try
        {
            Console.WriteLine("PrepareXMLtoClient");
            sqldrReader = sqlCommFun.ExecuteReader();
            sqldrReader.Close();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbPrepareXMLtoClient.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    public static bool FbPrepareXmlBundleToClient(SqlCommand sqlCommFun)
    {
        SqlDataReader sqldrReader;
        StringBuilder sbXMLBundle = new StringBuilder();
        sbXMLBundle.AppendLine(@"exec spSaveXmlFragment '" + ClGuid.gBundle + @"',204,1,N'&lt;UpdateIdentity UpdateID=""" + ClGuid.gBundle + @""" RevisionNumber=""204"" /&gt;&lt;Properties UpdateType=""Software"" ExplicitlyDeployable=""true"" AutoSelectOnWebSites=""true"" /&gt;&lt;Relationships&gt;&lt;Prerequisites&gt;&lt;AtLeastOne IsCategory=""true""&gt;&lt;UpdateIdentity UpdateID=""0fa1201d-4330-4fa8-8ae9-b877473b6441"" /&gt;&lt;/AtLeastOne&gt;&lt;/Prerequisites&gt;&lt;BundledUpdates&gt;&lt;UpdateIdentity UpdateID=""" + ClGuid.gUpdate + @""" RevisionNumber=""202"" /&gt;&lt;/BundledUpdates&gt;&lt;/Relationships&gt;',NULL");
        sbXMLBundle.AppendLine(@"exec spSaveXmlFragment '" + ClGuid.gBundle + @"',204,4,N'&lt;LocalizedProperties&gt;&lt;Language&gt;en&lt;/Language&gt;&lt;Title&gt;" + Create.UpdateTitle + @"&lt;/Title&gt;&lt;Description&gt;" + Create.UpdateDescription + @"&lt;/Description&gt;&lt;UninstallNotes&gt;This software update can be removed by selecting View installed updates in the Programs and Features Control Panel.&lt;/UninstallNotes&gt;&lt;MoreInfoUrl&gt;" + Create.UpdateURL + @"&lt;/MoreInfoUrl&gt;&lt;SupportUrl&gt;" + Create.UpdateURL + @"&lt;/SupportUrl&gt;&lt;/LocalizedProperties&gt;', NULL, 'en'");
        sbXMLBundle.AppendLine(@"exec spSaveXmlFragment '" + ClGuid.gBundle + @"',204,2,N'&lt;ExtendedProperties DefaultPropertiesLanguage=""en"" MsrcSeverity=""" + Create.UpdateRating + @""" IsBeta=""false""&gt;&lt;SupportUrl&gt;" + Create.UpdateURL + @"&lt;/SupportUrl&gt;&lt;SecurityBulletinID&gt;" + Create.UpdateMSRC + @"&lt;/SecurityBulletinID&gt;&lt;KBArticleID&gt;" + Create.UpdateKB + @"&lt;/KBArticleID&gt;&lt;/ExtendedProperties&gt;',NULL");
        sqlCommFun.CommandText = sbXMLBundle.ToString();
        try
        {
            Console.WriteLine("PrepareXMLBundletoClient");
            sqldrReader = sqlCommFun.ExecuteReader();
            sqldrReader.Close();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbPrepareXmlBundleToClient.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    public static bool FbInjectUrl2Download(SqlCommand sqlCommFun)
    {
        SqlDataReader sqldrReader;
        StringBuilder sbDownloadURL = new StringBuilder();
        string sDownloadURLexec = string.Empty;
        if (Server.bSSL == true)
        {
            sDownloadURLexec = @"https://" + Server.sComputerName + ":" + Server.iPortNumber + "/Content/wuagent.exe";
        }
        else if (Server.bSSL == false)
        {
            sDownloadURLexec = @"http://" + Server.sComputerName + ":" + Server.iPortNumber + "/Content/wuagent.exe";
        }
        else
        {
            return false;
        }

        sbDownloadURL.AppendLine(@"exec spSetBatchURL @urlBatch =N'<ROOT><item FileDigest=""" + ClFile.sSHA1 + @""" MUURL=""" + sDownloadURLexec + @""" USSURL="""" /></ROOT>'");
        sqlCommFun.CommandText = sbDownloadURL.ToString();
        try
        {
            Console.WriteLine("InjectURL2Download");
            sqldrReader = sqlCommFun.ExecuteReader();
            sqldrReader.Close();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbInjectUrl2Download.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    public static bool FbDeploymentRevision(SqlCommand sqlCommFun)
    {
        SqlDataReader sqldrReader;
        StringBuilder sbDeployRev = new StringBuilder();
        sbDeployRev.AppendLine(@"exec spDeploymentAutomation @revisionID = " + Create.iRevisionID);
        sqlCommFun.CommandText = sbDeployRev.ToString();
        try
        {
            Console.WriteLine("DeploymentRevision");
            sqldrReader = sqlCommFun.ExecuteReader();
            sqldrReader.Close();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbDeploymentRevision.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    public static bool FbPrepareBundle(SqlCommand sqlCommFun)
    {
        SqlDataReader sqldrReader;
        StringBuilder sbPrepareBund = new StringBuilder();
        System.Data.DataTable dtDataTbl = new System.Data.DataTable();

        sbPrepareBund.AppendLine(@"declare @iImported int");
        sbPrepareBund.AppendLine(@"declare @iLocalRevisionID int");
        sbPrepareBund.AppendLine(@"exec spImportUpdate @UpdateXml=N'");
        sbPrepareBund.AppendLine(@"<upd:Update xmlns:pub=""http://schemas.microsoft.com/msus/2002/12/Publishing"" xmlns:upd=""http://schemas.microsoft.com/msus/2002/12/Update"">");
        sbPrepareBund.AppendLine("\t" + @"<upd:UpdateIdentity UpdateID=""" + ClGuid.gBundle + @""" RevisionNumber=""204"" />");
        sbPrepareBund.AppendLine("\t" + @"<upd:Properties DefaultPropertiesLanguage=""en"" UpdateType=""Software"" ExplicitlyDeployable=""true"" AutoSelectOnWebSites=""true"" MsrcSeverity=""" + Create.UpdateRating + @""" IsPublic=""false"" IsBeta=""false"" PublicationState=""Published"" CreationDate=""" + Create.UpdateDate + @""" PublisherID=""395392a0-19c0-48b7-a927-f7c15066d905"" LegacyName=""" + Create.UpdateTitle + @""">");
        sbPrepareBund.AppendLine("\t\t" + @"<upd:SupportUrl>" + Create.UpdateURL + @"</upd:SupportUrl>");
        sbPrepareBund.AppendLine("\t\t" + @"<upd:SecurityBulletinID>" + Create.UpdateMSRC + @"</upd:SecurityBulletinID>");
        sbPrepareBund.AppendLine("\t\t" + @"<upd:KBArticleID>" + Create.UpdateKB + @"</upd:KBArticleID>");
        sbPrepareBund.AppendLine("\t" + @"</upd:Properties>");
        sbPrepareBund.AppendLine("\t" + @"<upd:LocalizedPropertiesCollection>");
        sbPrepareBund.AppendLine("\t\t" + @"<upd:LocalizedProperties>");
        sbPrepareBund.AppendLine("\t\t\t" + @"<upd:Language>en</upd:Language>");
        sbPrepareBund.AppendLine("\t\t\t" + @"<upd:Title>" + Create.UpdateTitle + @"</upd:Title>");
        sbPrepareBund.AppendLine("\t\t\t" + @"<upd:Description>" + Create.UpdateDescription + "</upd:Description>");
        sbPrepareBund.AppendLine("\t\t\t" + @"<upd:UninstallNotes>This software update can be removed by selecting View installed updates in the Programs and Features Control Panel.</upd:UninstallNotes>");
        sbPrepareBund.AppendLine("\t\t\t" + @"<upd:MoreInfoUrl>" + Create.UpdateURL + @"</upd:MoreInfoUrl>");
        sbPrepareBund.AppendLine("\t\t\t" + @"<upd:SupportUrl>" + Create.UpdateURL + @"</upd:SupportUrl>");
        sbPrepareBund.AppendLine("\t\t" + @"</upd:LocalizedProperties>");
        sbPrepareBund.AppendLine("\t" + @"</upd:LocalizedPropertiesCollection>");
        sbPrepareBund.AppendLine("\t" + @"<upd:Relationships>");
        sbPrepareBund.AppendLine("\t\t" + @"<upd:Prerequisites>");
        sbPrepareBund.AppendLine("\t\t\t" + @"<upd:AtLeastOne IsCategory=""true"">");
        sbPrepareBund.AppendLine("\t\t\t\t" + @"<upd:UpdateIdentity UpdateID=""0fa1201d-4330-4fa8-8ae9-b877473b6441"" />");
        sbPrepareBund.AppendLine("\t\t\t" + @"</upd:AtLeastOne>");
        sbPrepareBund.AppendLine("\t\t" + @"</upd:Prerequisites>");
        sbPrepareBund.AppendLine("\t\t" + @"<upd:BundledUpdates>");
        sbPrepareBund.AppendLine("\t\t\t" + @"<upd:UpdateIdentity UpdateID=""" + ClGuid.gUpdate + @""" RevisionNumber=""202"" />");
        sbPrepareBund.AppendLine("\t\t" + @"</upd:BundledUpdates>");
        sbPrepareBund.AppendLine("\t" + @"</upd:Relationships>");
        sbPrepareBund.AppendLine(@"</upd:Update>',@UpstreamServerLocalID=1,@Imported=@iImported output,@localRevisionID=@iLocalRevisionID output,@UpdateXmlCompressed=NULL");
        sbPrepareBund.AppendLine(@"select @iImported, @iLocalRevisionID");
        sqlCommFun.CommandText = sbPrepareBund.ToString();

        try
        {
            sqldrReader = sqlCommFun.ExecuteReader();
            dtDataTbl.Load(sqldrReader);
            Create.iRevisionID = (int)dtDataTbl.Rows[0][1];

            Console.WriteLine("PrepareBundle");
            Console.WriteLine("PrepareBundle Revision ID: {0}", Create.iRevisionID);

            if (Create.iRevisionID == 0)
            {
                Console.WriteLine("Error creating update bundle");
                sqldrReader.Close();
                return false;
            }

            sqldrReader.Close();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbPrepareBundle.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
}
