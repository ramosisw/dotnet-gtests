using System;
using System.Linq;

namespace dotnet.gtests.Command
{
    public static class HelpCommand
    {
        public static bool HasHelpOption(string[] args)
        {
            if (args.Contains("-h") || args.Contains("--help"))
            {
                WriteHelp();
                return true;
            }
            return false;
        }

        public static void WriteHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  dotnet-gtests [options] <PROJECT>");
            Console.WriteLine("  ");
            Console.WriteLine("Arguments:");
            Console.WriteLine("  <PROJECT>   The project file to operate on, where be add tests classes. If a file is not specified, the command will search the current directory for one.");
            Console.WriteLine("");
            Console.WriteLine("Options:");
            Console.WriteLine("  -h, --help            Show command line help.");
            Console.WriteLine("  -s, --source-project  The project file where the classes will be searched to generate tests.");
            Console.WriteLine("  -m, --gmethods        Public methods must be included in the test class. (default true).");
            Console.WriteLine("  -o, --output-dir      The ouput folder where be add tests classes, relative to <PROJECT>. (default is root on <PROJECT>)");
        }
    }
}