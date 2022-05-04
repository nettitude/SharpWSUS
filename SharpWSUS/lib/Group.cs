using System;
using System.Data.SqlClient;

public class Group
{
    public static bool GroupExists = false;
    public static bool FbGetComputerTarget(SqlCommand sqlCommFun, string sTargetComputer)
    {
        SqlDataReader sqldrReader;
        sqlCommFun.CommandText = "exec spGetComputerTargetByName @fullDomainName = N'" + sTargetComputer + "'";
        try
        {
            Console.WriteLine("\r\nTargeting {0}", sTargetComputer);
            Console.WriteLine("TargetComputer, ComputerID, TargetID");
            Console.WriteLine("------------------------------------");
            sqldrReader = sqlCommFun.ExecuteReader();
            if (sqldrReader.Read())
            {
                Server.sTargetComputerID = (string)sqldrReader.GetValue(sqldrReader.GetOrdinal("ComputerID"));
                if (Server.sTargetComputerID.Length != 0)
                {
                    sqldrReader.Close();
                    sqlCommFun.CommandText = "SELECT dbo.fnGetComputerTargetID('" + Server.sTargetComputerID + "')";
                    sqldrReader = sqlCommFun.ExecuteReader();
                    if (sqldrReader.Read())
                    {
                        Server.sTargetComputerTargetID = (int)sqldrReader.GetValue(0);
                        Console.WriteLine("{0}, {1}, {2}", sTargetComputer, Server.sTargetComputerID, Server.sTargetComputerTargetID);
                        sqldrReader.Close();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Internal WSUS database error - Target computer {0} has ComputerID {1} but does not have TargetID", sTargetComputer.Length, Server.sTargetComputerID);
                        sqldrReader.Close();
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Target computer cannot be found: {0}", sTargetComputer);
                    sqldrReader.Close();
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Target computer cannot be found: {0}", sTargetComputer);
                return false;
            }

        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetComputerTarget.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }

    public static bool FbGetGroupID(SqlCommand sqlCommFun, string GroupName)
    {
        SqlDataReader sqldrReader;
        sqlCommFun.CommandText = @"exec spGetAllTargetGroups";
        try
        {
            sqldrReader = sqlCommFun.ExecuteReader();
            int count = sqldrReader.FieldCount;
            while (sqldrReader.Read())
            {
                string TargetGroupName = (string)sqldrReader.GetValue(sqldrReader.GetOrdinal("Name"));
                if (TargetGroupName == GroupName)
                {
                    ClGuid.gTargetGroup = (Guid)sqldrReader.GetValue(sqldrReader.GetOrdinal("TargetGroupID"));
                    GroupExists = true;
                }
            }
        }
        catch
        {
            Console.WriteLine("\r\nGroup does not exist already.");
            return true;
        }
        sqldrReader.Close();
        return true;
    }

    public static bool FbCreateGroup(SqlCommand sqlCommFun, string GroupName)
    {
        SqlDataReader sqldrReader;
        sqlCommFun.CommandText = @"exec spCreateTargetGroup @name='" + GroupName + "', @id='" + ClGuid.gTargetGroup + "'";
        try
        {
            sqldrReader = sqlCommFun.ExecuteReader();
            sqldrReader.Close();
                
            Console.WriteLine("Group Created: {0}", GroupName);
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbCreateGroup.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
        sqldrReader.Close();
        return true;
    }

    public static bool FbRemoveGroup(SqlCommand sqlCommFun)
    {
        SqlDataReader sqldrReader;
        sqlCommFun.CommandText = @"exec spDeleteTargetGroup @targetGroupID='" + ClGuid.gTargetGroup + "'";
        try
        {
            sqldrReader = sqlCommFun.ExecuteReader();
            sqldrReader.Close();

            Console.WriteLine("Remove Group");
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbRemoveGroup.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
        sqldrReader.Close();
        return true;
    }

    public static bool FbAddComputerToGroup(SqlCommand sqlCommFun, int ComputerTargetID)
    {
        SqlDataReader sqldrReader;
        sqlCommFun.CommandText = @"exec spAddTargetToTargetGroup @targetGroupID='" + ClGuid.gTargetGroup + "', @targetID=" + ComputerTargetID;
        try
        {
            sqldrReader = sqlCommFun.ExecuteReader();
            sqldrReader.Close();

            Console.WriteLine("Added Computer To Group");

        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbAddComputerToGroup.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
        sqldrReader.Close();
        return true;
    }

    public static bool FbRemoveComputerFromGroup(SqlCommand sqlCommFun, int ComputerTargetID)
    {
        SqlDataReader sqldrReader;
        sqlCommFun.CommandText = @"exec spRemoveTargetFromTargetGroup @targetGroupID='" + ClGuid.gTargetGroup + "', @targetID=" + ComputerTargetID;
        try
        {
            sqldrReader = sqlCommFun.ExecuteReader();
            sqldrReader.Close();

            Console.WriteLine("Removed Computer From Group");

        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbRemoveComputerFromGroup.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
        sqldrReader.Close();
        return true;
    }
}
