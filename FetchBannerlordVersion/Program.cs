using System;
using System.IO;

namespace FetchBannerlordVersion {

  public static partial class Program {

    public static int Main(string[] args) {
      if (args.Length < 1 || string.IsNullOrEmpty(args[0]))
        throw new ArgumentNullException(nameof(args));

      try {
        var libPath = Path.Combine(args[0], "TaleWorlds.Library.dll");

        if (!File.Exists(libPath))
          throw new FileNotFoundException(libPath);

        var appVersion = ExtractVersionFromTaleWorldsLibrary(libPath, out var changeSet);

        Console.WriteLine($"{appVersion}.{changeSet}");
        return 0;
      }
      catch (Exception ex) {
        Console.Error.WriteLine(ex.ToString());
        return 1;
      }
    }

  }

}