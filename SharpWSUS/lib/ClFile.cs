using System;
using System.IO;
using System.Security.Cryptography;
using System.Web;

public class ClFile
{
    public static string sFileName;
    public static string sPayload;
    public static string sFilePath;
    public static string sArgs;
    public static long lSize;
    public static string sSHA1;
    public static string sSHA256;

    public ClFile(string sPFilePath, string sPArgs, string sContentLocation, bool bPCopyFile)
    {
        sFilePath = sPFilePath;
        sFileName = System.IO.Path.GetFileName(sFilePath);
        sArgs = HttpUtility.HtmlEncode(HttpUtility.HtmlEncode(sPArgs));
        if (bPCopyFile == true)
        {
            FbCopyFile(sFilePath, sContentLocation);
        }
        lSize = new System.IO.FileInfo(sFilePath).Length;
        sSHA1 = GetBase64EncodedSHA1Hash(sFilePath);
        sSHA256 = GetBase64EncodedSHA256Hash(sFilePath);
    }
    public static bool FbCopyFile(string sFilePath, string sContentLocation)
    {
        try
        {
            //Console.WriteLine(sFilePath);
            //Console.WriteLine(sContentLocation);
            File.Copy(sFilePath, sContentLocation + @"\wuagent.exe", true);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\r\nFunction error - FbCopyFile.");
            
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
    //https://stackoverflow.com/questions/19150468/get-sha1-binary-base64-hash-of-a-file-on-c-sharp/19150543
    public string GetBase64EncodedSHA1Hash(string filename)
    {
        FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
        SHA1Managed sha1 = new SHA1Managed();
        {
            return Convert.ToBase64String(sha1.ComputeHash(fs));
        }
    }
    public string GetBase64EncodedSHA256Hash(string filename)
    {
        FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
        SHA256Managed sha256 = new SHA256Managed();
        {
            return Convert.ToBase64String(sha256.ComputeHash(fs));
        }
    }
}
