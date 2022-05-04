using Microsoft.Win32;
using System;
using System.Data.SqlClient;

public class Enum
{
    public static bool FbGetWSUSConfigSQL(SqlCommand sqlCommFun)
    {
        SqlDataReader sqldrReader;
        sqlCommFun.CommandText = "exec spConfiguration";
        try
        {
            //Gather Information via SQL
            sqldrReader = sqlCommFun.ExecuteReader();
            if (sqldrReader.Read())
            {
                Server.sLocalContentCacheLocation = (string)sqldrReader.GetValue(sqldrReader.GetOrdinal("LocalContentCacheLocation"));
                Server.iPortNumber = (int)sqldrReader.GetValue(sqldrReader.GetOrdinal("ServerPortNumber"));
                Console.WriteLine("\r\n################# WSUS Server Enumeration via SQL ##################");
                Console.WriteLine("ServerName, WSUSPortNumber, WSUSContentLocation");
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("{0}, {1}, {2}\r\n", Environment.MachineName, Server.iPortNumber, Server.sLocalContentCacheLocation);
                sqldrReader.Close();
                return true;
            }
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetWSUSConfigSQL.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    public static bool FbEnumAllComputers(SqlCommand sqlCommFun)
    {
        SqlDataReader sqldrReader;
        sqlCommFun.CommandText = "exec spGetAllComputers";
        try
        {
            Console.WriteLine("\r\n####################### Computer Enumeration #######################");
            Console.WriteLine("ComputerName, IPAddress, OSVersion, LastCheckInTime");
            Console.WriteLine("---------------------------------------------------");
            sqldrReader = sqlCommFun.ExecuteReader();
            int count = sqldrReader.FieldCount;
            while (sqldrReader.Read())
            {
                Console.WriteLine("{0}, {1}, {2}, {3}", sqldrReader.GetValue(sqldrReader.GetOrdinal("FullDomainName")), sqldrReader.GetValue(sqldrReader.GetOrdinal("IPAddress")), sqldrReader.GetValue(sqldrReader.GetOrdinal("ClientVersion")), sqldrReader.GetValue(sqldrReader.GetOrdinal("LastReportedStatusTime")));
            }
            sqldrReader.Close();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbEnumAllComputers.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    public static bool FbEnumDownStream(SqlCommand sqlCommFun)
    {
        SqlDataReader sqldrReader;
        sqlCommFun.CommandText = "exec spGetAllDownstreamServers";
        try
        {
            Console.WriteLine("\r\n####################### Downstream Server Enumeration #######################");
            Console.WriteLine("ComputerName, OSVersion, LastCheckInTime");
            Console.WriteLine("---------------------------------------------------");
            sqldrReader = sqlCommFun.ExecuteReader();
            int count = sqldrReader.FieldCount;
            while (sqldrReader.Read())
            {
                Console.WriteLine("{0}, {1}, {2}", sqldrReader.GetValue(sqldrReader.GetOrdinal("AccountName")), sqldrReader.GetValue(sqldrReader.GetOrdinal("Version")), sqldrReader.GetValue(sqldrReader.GetOrdinal("RollupLastSyncTime")));
            }
            sqldrReader.Close();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbEnumDownStream.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }

    public static bool FbEnumGroups(SqlCommand sqlCommFun)
    {
        SqlDataReader sqldrReader;
        sqlCommFun.CommandText = "exec spGetAllTargetGroups";
        try
        {
            Console.WriteLine("\r\n####################### Group Enumeration #######################");
            Console.WriteLine("GroupName");
            Console.WriteLine("---------------------------------------------------");
            sqldrReader = sqlCommFun.ExecuteReader();
            int count = sqldrReader.FieldCount;
            while (sqldrReader.Read())
            {
                Console.WriteLine("{0}", sqldrReader.GetValue(sqldrReader.GetOrdinal("Name")));
            }
            sqldrReader.Close();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbEnumGroups.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }

    public static bool FbGetWSUSServer()
    {
        try
        {
            const string keyName = @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\WindowsUpdate";
            string WSUSServer = (string)Registry.GetValue(keyName, "WUServer", "WSUS Registry Key Not Found!");

            Console.WriteLine("WSUS Server: {0}", WSUSServer);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetWSUSServer.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
}
