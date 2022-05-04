using System;
using System.Data.SqlClient;

public class Connect
{
    public static SqlConnection FsqlConnection()
    {
        SqlConnection sqlcQuery = new SqlConnection();

        if (Server.sDatabaseInstance.Contains("##WID") || Server.sDatabaseInstance.Contains("##SSEE"))
        {
            if (Server.sOS.Contains("Server 2008"))
            {
                sqlcQuery.ConnectionString = @"Server=np:\\.\pipe\MSSQL$MICROSOFT##SSEE\sql\query;Database=" + Server.sDatabaseName + @";Integrated Security=True";
            }
            else
            {
               sqlcQuery.ConnectionString = @"Server=np:\\.\pipe\MICROSOFT##WID\tsql\query;Database=" + Server.sDatabaseName + @"; Integrated Security=True";
            }
        }
        else
        {
            sqlcQuery.ConnectionString = @"Server=" + Server.sDatabaseInstance + @";Database=" + Server.sDatabaseName + @"; Integrated Security=True";
        }

        try
        {
            sqlcQuery.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FsqlConnection.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return null;
        }
        return sqlcQuery;
    }
}
