using System;
using System.Net;
using System.Data.SqlClient;

public class Server
{
    public static bool bWSUSInstalled = true;
    public static string sOS;
    public static string sDatabaseInstance;
    public static string sDatabaseName;
    public static string sLocalContentCacheLocation;
    public static string sComputerName;
    public static int iPortNumber;
    public static bool bSSL;
    public static string sTargetComputerID;
    public static int sTargetComputerTargetID;

    public static void GetServerDetails()
    {
        FvCheckSSL();
        FvFullComputerName();
        FvOSDetails();
        FvDatabaseBaseName();
        FvContentDirectory();
    }
    public static void FvContentDirectory()
    {
        SqlCommand sqlComm = new SqlCommand();
        sqlComm.Connection = Connect.FsqlConnection();
        SqlDataReader sqldrReader;
        sqlComm.CommandText = "exec spConfiguration";
        try
        {
            //Gather Information via SQL
            sqldrReader = sqlComm.ExecuteReader();
            if (sqldrReader.Read())
            {
                Server.sLocalContentCacheLocation = (string)sqldrReader.GetValue(sqldrReader.GetOrdinal("LocalContentCacheLocation"));
                Console.WriteLine(Server.sLocalContentCacheLocation);
                sqldrReader.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FvContentDirectory.");

            Console.WriteLine($"Error Message: {e.Message}");
        }
    }
    public static void FvFullComputerName()
    {
        sComputerName = Dns.GetHostEntry("LocalHost").HostName;
    }
    public static void FvDatabaseBaseName()
    {
        string sDBServerTemp = string.Empty;
        sDBServerTemp = Reg.RegistryWOW6432.GetRegKey64(Reg.RegHive.HKEY_LOCAL_MACHINE, @"SOFTWARE\Microsoft\Update Services\Server\setup", "SqlServerName");
        if (sDBServerTemp == "ERROR_FILE_NOT_FOUND")
        {
            sDBServerTemp = Reg.RegistryWOW6432.GetRegKey32(Reg.RegHive.HKEY_LOCAL_MACHINE, @"SOFTWARE\Microsoft\Update Services\Server\setup", "SqlServerName");
            if (sDBServerTemp == "ERROR_FILE_NOT_FOUND")
            {
                bWSUSInstalled = false;
                Console.WriteLine("Something went wrong, unable to detect SQL details from registry.");
                return;
            }
        }
        sDBServerTemp = HEX2ASCII(sDBServerTemp);
        sDatabaseInstance = ReverseString(sDBServerTemp);

        string sDBNameTemp = string.Empty;            
        sDBNameTemp = Reg.RegistryWOW6432.GetRegKey64(Reg.RegHive.HKEY_LOCAL_MACHINE, @"SOFTWARE\Microsoft\Update Services\Server\setup", "SqlDatabaseName");
        if (sDBNameTemp == "ERROR_FILE_NOT_FOUND")
        {
            sDBNameTemp = Reg.RegistryWOW6432.GetRegKey32(Reg.RegHive.HKEY_LOCAL_MACHINE, @"SOFTWARE\Microsoft\Update Services\Server\setup", "SqlDatabaseName");
            if (sDBNameTemp == "ERROR_FILE_NOT_FOUND")
            {
                bWSUSInstalled = false;
                Console.WriteLine("Something went wrong, unable to detect SQL details from registry.");
                return;
            }
        }
        sDBNameTemp = HEX2ASCII(sDBNameTemp);
        sDatabaseName = ReverseString(sDBNameTemp);
        return;
    }
    public static void FvOSDetails()
    {
        string sOSTemp = string.Empty; 
        sOSTemp = Reg.RegistryWOW6432.GetRegKey64(Reg.RegHive.HKEY_LOCAL_MACHINE, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName");
        if (sOSTemp == "ERROR_FILE_NOT_FOUND")
        {
            sOSTemp = Reg.RegistryWOW6432.GetRegKey32(Reg.RegHive.HKEY_LOCAL_MACHINE, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName");
            if (sOSTemp == "ERROR_FILE_NOT_FOUND")
            {
                bWSUSInstalled = false;
                Console.WriteLine("Something went wrong, unable to detect OS version.");
                return;
            }
        }
        sOSTemp = HEX2ASCII(sOSTemp);
        sOS = ReverseString(sOSTemp);
    }
    public static void FvCheckSSL()
    {
        string sSSLTemp = string.Empty; 
        sSSLTemp = Reg.RegistryWOW6432.GetRegKey64(Reg.RegHive.HKEY_LOCAL_MACHINE, @"SOFTWARE\Microsoft\Update Services\Server\setup", "UsingSSL");
        if (sSSLTemp == "ERROR_FILE_NOT_FOUND")
        {
            sSSLTemp = Reg.RegistryWOW6432.GetRegKey32(Reg.RegHive.HKEY_LOCAL_MACHINE, @"SOFTWARE\Microsoft\Update Services\Server\setup", "UsingSSL");
            if (sSSLTemp == "ERROR_FILE_NOT_FOUND")
            {
                bWSUSInstalled = false;
                return;
            }
        }
        if (sSSLTemp == "01")
        {
            bSSL = true;
        }
        else
        {
            bSSL = false;
        }
    }
    //https://stackoverflow.com/questions/17637950/convert-string-of-hex-to-string-of-little-endian-in-c-sharp       
    public static string HEX2ASCII(string hex)
    {
        string res = String.Empty;
        for (int a = 0; a < hex.Length; a = a + 2)
        {
            string Char2Convert = hex.Substring(a, 2);
            int n = Convert.ToInt32(Char2Convert, 16);
            char c = (char)n;
            res += c.ToString();
        }
        return res;
    }
    //https://www.dotnetperls.com/reverse-string
    public static string ReverseString(string s)
    {
        char[] arr = s.ToCharArray();
        Array.Reverse(arr);
        return new string(arr);
    }
}
