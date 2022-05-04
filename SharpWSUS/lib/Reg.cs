using System;
using System.Text;
using System.Runtime.InteropServices;

public class Reg
{
    public enum RegSAM
    {
        QueryValue = 0x0001,
        SetValue = 0x0002,
        CreateSubKey = 0x0004,
        EnumerateSubKeys = 0x0008,
        Notify = 0x0010,
        CreateLink = 0x0020,
        WOW64_32Key = 0x0200,
        WOW64_64Key = 0x0100,
        WOW64_Res = 0x0300,
        Read = 0x00020019,
        Write = 0x00020006,
        Execute = 0x00020019,
        AllAccess = 0x000f003f
    }
    public static class RegHive
    {
        public static UIntPtr HKEY_LOCAL_MACHINE = new UIntPtr(0x80000002u);
        public static UIntPtr HKEY_CURRENT_USER = new UIntPtr(0x80000001u);
    }
    public static class RegistryWOW6432
    {
        [DllImport("Advapi32.dll")]
        static extern uint RegOpenKeyEx(UIntPtr hKey, string lpSubKey, uint ulOptions, int samDesired, out int phkResult);

        [DllImport("Advapi32.dll")]
        static extern uint RegCloseKey(int hKey);

        [DllImport("advapi32.dll", EntryPoint = "RegQueryValueEx")]
        public static extern uint RegQueryValueEx(int hKey, string lpValueName, int lpReserved, ref uint lpType, System.Text.StringBuilder lpData, ref uint lpcbData);

        static public string GetRegKey64(UIntPtr inHive, String inKeyName, string inPropertyName)
        {
            return GetRegKey64(inHive, inKeyName, RegSAM.WOW64_64Key, inPropertyName);
        }

        static public string GetRegKey32(UIntPtr inHive, String inKeyName, string inPropertyName)
        {
            return GetRegKey64(inHive, inKeyName, RegSAM.WOW64_32Key, inPropertyName);
        }

        public static string GetRegKey64(UIntPtr inHive, String inKeyName, RegSAM in32or64key, string inPropertyName)
        {
            //UIntPtr HKEY_LOCAL_MACHINE = (UIntPtr)0x80000002;
            int hkey = 0;

            try
            {
                uint lResult = RegOpenKeyEx(RegHive.HKEY_LOCAL_MACHINE, inKeyName, 0, (int)RegSAM.QueryValue | (int)in32or64key, out hkey);
                if (0 != lResult)
                {
                    return "ERROR_FILE_NOT_FOUND";
                }
                uint lpType = 0;
                uint lpcbData = 1024;
                StringBuilder AgeBuffer = new StringBuilder(1024);
                uint lResultv = RegQueryValueEx(hkey, inPropertyName, 0, ref lpType, AgeBuffer, ref lpcbData);
                if (lResultv != 0)
                {
                    return "ERROR_FILE_NOT_FOUND";
                }
                byte[] arr = System.Text.Encoding.ASCII.GetBytes(AgeBuffer.ToString());
                return ByteArrayToString(arr);
            }
            finally
            {
                if (0 != hkey) RegCloseKey(hkey);
            }
        }
        public static string ByteArrayToString(byte[] ba)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(ba);

            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }
    }
}
