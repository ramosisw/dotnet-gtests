using System;
using System.IO;
using System.Linq;

namespace dotnet.gtests.Command
{
    public class OptionsCommand
    {
        public string TestProject { get; set; }
        public string CodeProject { get; set; }
        public bool GenerateMethods { get; set; }
        public string OutputDir { get; set; }

        public OptionsCommand()
        {
            TestProject = Path.GetFullPath(".");
            CodeProject = Path.GetFullPath(".");
            GenerateMethods = true;
        }

        public override string ToString()
        {
            return $"Options [TestProject={TestProject}, CodeProject={CodeProject}, GenerateMethods={GenerateMethods}]";
        }

        public static OptionsCommand Parse(string[] args)
        {
            Console.WriteLine(string.Join("|", args));
            var validOptions = new[] { "-s", "--source-project", "-m", "--gmethods", "-o", "--output-dir" };
            var optionsCommand = new OptionsCommand();
            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i].Trim();
                var argSplit = arg.Split("=");
                var argName = argSplit[0];
                var argValue = args[i];
                if (arg.StartsWith("-") && !validOptions.Contains(argName)) throw new ArgumentException($"Unknown option {argName}");
                if (arg.StartsWith("-"))
                {
                    argValue = arg.Contains("=") && !string.IsNullOrEmpty(argSplit[1]) ? argSplit[1] : (i + 1) < args.Length ? args[i++ + 1] : args[i];
                }
                switch (argName)
                {
                    case "-s":
                    case "--source-project":
                        optionsCommand.CodeProject = Path.GetFullPath(argValue);
                        break;
                    case "-m":
                    case "--gmethods":
                        try
                        {
                            optionsCommand.GenerateMethods = bool.Parse(argValue);
                        }
                        catch (Exception)
                        {
                            throw new ArgumentException($"Unknown value {argValue} for option {argName}, only acept true or false");
                        }
                        break;
                    case "-o":
                    case "--output-dir":
                        optionsCommand.OutputDir = argValue;
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