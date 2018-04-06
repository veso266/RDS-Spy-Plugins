# RDS Spy Plugins
This repository contains my plugins that I wrote for RDS Spy using C# instead of Delphi

I used a nice library called [Unmanaged Exports](https://www.nuget.org/packages/UnmanagedExports/)
to be able to exports functions that RDS Spy calls from C#

I've used RDS Spy [documentation](http://rdsspy.com/download/pdf/spyapi.pdf) for writing plugins and [stackedit.io](https://stackedit.io/) to generate this readme 

Because I don't know Delphi nor Pascal I had to google how syntax between Delphi and C# differs. To make easier for others to do the same, you have a table here on what is diferent (what you need to write a basic plugin, because a lot is missing)


| Delphi | C# |
| ------------- | ------------- |
| word | ushort  |
| Pchar | string  |
| integer | int  |
| ss | ss  |
| = | ==  |
| <> | !=  |
|  |  |
|  |  |
|  |  |
| UpperCase("string") | "string".ToUpper(); |
| IntToHex('FFFF',4)) | 'FFFF'.ToString("X4") |
| Result: "Function will return this" | return "Function will return this" |
| function PluginName: PChar;| string PluginName() |
| procedure | void |

To be able to compile this sample plugin you first need to add [Unmanaged Exports](https://www.nuget.org/packages/UnmanagedExports/) as NuGet package and set your build target to eather x86 or x64 (AnyCPU will not work and as RDS Spy is x86 I've used x86 here)
