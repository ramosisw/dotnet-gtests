using System;

namespace dotnet.gtests
{
    class Program
    {
        public static int Main(string[] args)
        {
            if (HelpCommand.HasHelpOption(args)) return 0;
            try
            {
                var options = OptionsCommand.Parse(args);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message + "\n");
                HelpCommand.WriteHelp();
            }

            Console.WriteLine("Hello World!");

            return 0;
        }
    }
}
