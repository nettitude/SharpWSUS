using System;
using System.Data.SqlClient;

public class Status
{
    public static bool FbApproveUpdate(SqlCommand sqlCommFun, string UpdateID, string Approver)
    {
        SqlDataReader sqldrReader;
        sqlCommFun.CommandText = @"exec spDeployUpdate @updateID='" + UpdateID + "',@revisionNumber=204,@actionID=0,@targetGroupID='" + ClGuid.gTargetGroup + "',@adminName=N'" + Approver + "',@isAssigned=1,@downloadPriority=1,@failIfReplica=0,@isReplica=0";
        try
        {
            sqldrReader = sqlCommFun.ExecuteReader();
            sqldrReader.Close();
            Console.WriteLine("Approved Update");
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbApproveUpdate.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
        sqldrReader.Close();
        return true;
    }

    public static bool FbDeleteUpdate(SqlCommand sqlCommFun, string sBundleID)
    {
        SqlDataReader sqldrReader;
        sqlCommFun.CommandText = @"exec spDeclineUpdate @updateID='" + sBundleID + "'";
        try
        {
            sqldrReader = sqlCommFun.ExecuteReader();
            sqldrReader.Close();
            Console.WriteLine("\r\n[*] Update declined.\r\n");
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbDeleteUpdate.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
        sqlCommFun.CommandText = @"exec spDeleteUpdateByUpdateID @updateID='" + sBundleID + "'";
        try
        {
            sqldrReader = sqlCommFun.ExecuteReader();
            sqldrReader.Close();
            Console.WriteLine("\r\n[*] Update deleted.\r\n");
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbDeleteUpdate.");
            Console.WriteLine($"Error Message: {e.Message}");
            Console.WriteLine("If you are in a lab and this timed out, this could occur if there are too many old patches in the database causing performance issues.");
            return false; 
        }
        sqldrReader.Close();
        return true;
    }

    public static bool FbGetUpdateStatus(SqlCommand sqlCommFun, string UpdateID)
    {
        int LocalUpdateID = 0;
        SqlDataReader sqldrReader;
        sqlCommFun.CommandText = @"SELECT LocalUpdateID FROM dbo.tbUpdate WHERE UpdateID = '" + UpdateID + "'";
        try
        {
            sqldrReader = sqlCommFun.ExecuteReader();
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetUpdateStatus.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }

        if (sqldrReader.Read())
        {
            LocalUpdateID = (int)sqldrReader.GetValue(sqldrReader.GetOrdinal("LocalUpdateID"));
            sqldrReader.Close();
        }
        else
        {
            Console.WriteLine("\r\nUpdate ID " + UpdateID + " cannot be found.");
            return false;
        }

        sqlCommFun.CommandText = @"SELECT SummarizationState FROM dbo.tbUpdateStatusPerComputer WHERE LocalUpdateID='" + LocalUpdateID + "' AND TargetID='" + Server.sTargetComputerTargetID + "'";
        try
        {
            sqldrReader = sqlCommFun.ExecuteReader();
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbGetUpdateStatus2.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }

        if (sqldrReader.Read())
        {
            int SummarizationState = (int)sqldrReader.GetValue(sqldrReader.GetOrdinal("SummarizationState"));
            if (SummarizationState == 1)
            {
                Console.WriteLine("\r\n[*] Update is not needed");
                return true;
            }
            if (SummarizationState == 2)
            {
                Console.WriteLine("\r\n[*] Update is not installed");
                return true;
            }
            if (SummarizationState == 3)
            {
                Console.WriteLine("\r\n[*] Update is downloaded");
                return true;
            }
            if (SummarizationState == 4)
            {
                Console.WriteLine("\r\n[*] Update is installed");
                return true;
            }
            if (SummarizationState == 5)
            {
                Console.WriteLine("\r\n[*] Update failed");
                return true;
            }
            if (SummarizationState == 6)
            {
                Console.WriteLine("\r\n[*] Reboot Required");
                return true;
            }
            else
            {
                Console.WriteLine("\r\n[*] Update Info was found but state is unknown");
                return true;
            }
        }
        else
        {
            Console.WriteLine("\r\nUpdate Info cannot be found.");
        }

        sqldrReader.Close();
        return true;
    }
}
