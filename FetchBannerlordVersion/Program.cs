using CommandLine;

using FetchBannerlordVersion.Options;

using System;
using System.IO;

namespace FetchBannerlordVersion
{
    public static partial class Program
    {
        private const string Assembly = "TaleWorlds.Library.dll";

        public static void Main(string[] args) => Parser
            .Default
            .ParseArguments<VersionTypeOptions, VersionOptions, ChangeSetOptions>(args)
            .WithParsed<VersionTypeOptions>(o =>
            { 
                try
                {
                    var libPath = Path.Combine(o.Directory, Assembly);

                    Console.WriteLine(GetVersionType(libPath).ToString());
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.ToString());
                    Environment.Exit(1);
                }
            })
            .WithParsed<VersionOptions>(o =>
            { 
                try
                {
                    var libPath = Path.Combine(o.Directory, Assembly);

                    Console.WriteLine(GetVersion(libPath));
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.ToString());
                    Environment.Exit(1);
                }
            })
            .WithParsed<ChangeSetOptions>(o =>
            { 
                try
                {
                    var libPath = Path.Combine(o.Directory, Assembly);

                    Console.WriteLine(GetChangeSet(libPath).ToString());
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.ToString());
                    Environment.Exit(1);
                }
            })
            .WithNotParsed(e =>
            {
                Console.Error.WriteLine(e.ToString());
                Environment.Exit(1);
            });
    }
}