using System;
using System.IO;
using System.Linq;

namespace dotnet.gtests
{
    public class OptionsCommand
    {
        public string TestProject { get; set; }
        public string CodeProject { get; set; }
        public bool GenerateMethods { get; set; }

        public OptionsCommand()
        {
            TestProject = Path.GetFullPath(".");
            CodeProject = Path.GetFullPath(".");
        }

        public override string ToString()
        {
            return $"Options [TestProject={TestProject}, CodeProject={CodeProject}, GenerateMethods={GenerateMethods}]";
        }

        public static OptionsCommand Parse(string[] args)
        {
            var validOptions = new[] { "-p", "--project", "-m", "--gmethods" };
            var optionsCommand = new OptionsCommand();
            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                var argName = arg.Split("=")[0];
                if (arg.StartsWith('-') && !validOptions.Contains(argName)) throw new ArgumentException($"Unknown option {argName}");
                var argValue = arg.Contains("=") ? arg.Split("=")[1] : i + 1 < args.Length && argName != "-m" ? args[i++] : args[i];
                switch (argName)
                {
                    case "-p":
                    case "--project":
                        optionsCommand.CodeProject = Path.GetFullPath(argValue);
                        break;
                    case "-m":
                    case "--gmethods":
                        optionsCommand.GenerateMethods = true;
                        break;
                    default:
                        optionsCommand.TestProject = Path.GetFullPath(argValue);
                        break;
                }
            }

            optionsCommand.CodeProject = ResolveProjectFile(optionsCommand.CodeProject);
            optionsCommand.TestProject = ResolveProjectFile(optionsCommand.TestProject);
            Console.WriteLine(optionsCommand);
            return optionsCommand;
        }

        public static string ResolveProjectFile(string path)
        {
            var isFile = File.Exists(path);
            var isDirectory = Directory.Exists(path);

            if (isFile && !Path.GetExtension(path).Equals(".csproj", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"Only accept *.csproj files, actual {path}.");

            if (!isFile && !isDirectory)
                throw new ArgumentException($"The path not found, actual {path}");

            if (isDirectory)
            {
                var csprojs = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories);
                if (csprojs.Length == 0) throw new ArgumentException($"The *.csproj file not found in {path}");
                if (csprojs.Length > 1) throw new ArgumentException($"Are more that one *.csproj file in {path}");
                return csprojs[0];
            }

            if (isFile) return path;
            return path;
        }
    }
}