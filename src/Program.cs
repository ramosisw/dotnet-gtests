using System.Text.RegularExpressions;
using System.Collections.Generic;
using dotnet.gtests.Command;
using System.Linq;
using System.IO;
using System;
using System.Text;
using System.Threading;
using Microsoft.CSharp;
using System.Xml.Linq;

namespace dotnet.gtests
{
    class Program
    {
        private static readonly List<Regex> EXCLUDE_PATTERN = new List<Regex>() {
            new Regex("[Oo]bj/"),
            new Regex("[Bb]in/"),
            new Regex("Test.cs$")
        };

        private static readonly string TEMPLATE_TEST_CLASS = "\nnamespace $classNamespace \n{\n\tclass $className\n\t{\n\t}\n}";
        public static int Main(string[] args)
        {
            if (HelpCommand.HasHelpOption(args)) return 0;
            try
            {
                var options = OptionsCommand.Parse(args);
                Console.WriteLine("Working...");
                return Run(options);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message + "\n");
                HelpCommand.WriteHelp();
            }
            return 0;
        }

        private static bool IsExcluded(string searchPath, string target)
        {
            var startPath = target.Replace(searchPath, string.Empty).Replace(@"\", "/");
            return EXCLUDE_PATTERN.Any(d => d.Match(startPath).Success);
        }

        private static string GetTreeDirectory(string searchPath, string target)
        {

            var startPath = Path.GetDirectoryName(target).Replace(searchPath, string.Empty).Replace(@"\", "/");
            return startPath;
        }

        private static string GetRootNamespace(string fullProjectPath)
        {
            var rootNamespace = "";
            try
            {
                var projDefinition = XDocument.Load(fullProjectPath);
                rootNamespace = projDefinition
                    .Element("Project")
                    .Elements("PropertyGroup")
                    .Elements("RootNamespace")
                    .Select(e => e.Value)
                    .FirstOrDefault();
            }
            catch (Exception)
            {
                rootNamespace = Regex.Replace(new DirectoryInfo(Path.GetDirectoryName(fullProjectPath)).Name, @"[^\w.]", "_");
            }
            return rootNamespace;
        }

        private static int Run(OptionsCommand options)
        {
            var codeFilesPath = Path.GetDirectoryName(options.CodeProject);
            var testFilesPath = Path.GetDirectoryName(options.TestProject);
            var codeFiles = Directory.GetFiles(codeFilesPath, "*.cs", SearchOption.AllDirectories).Where(d => !IsExcluded(codeFilesPath, d));
            var rootNamespace = GetRootNamespace(options.TestProject);
            if (!string.IsNullOrEmpty(options.OutputDir))
            {
                rootNamespace = $"{rootNamespace}.{Regex.Replace(options.OutputDir, @"[^\w.]", "_")}";
            }

            foreach (var codeFile in codeFiles)
            {
                var treeDirectory = GetTreeDirectory(codeFilesPath, codeFile);
                var classNamespace = $"{rootNamespace}{treeDirectory.Replace("/", ".")}";
                var className = $"{Path.GetFileNameWithoutExtension(codeFile)}Tests";
                var fileName = $"{className}.cs";
                var fileDirectory = testFilesPath + "/" + options.OutputDir + treeDirectory;
                var filePath = fileDirectory + "/" + fileName;
                Directory.CreateDirectory(fileDirectory);
                if (File.Exists(filePath))
                {
                    Console.WriteLine($"File already exist {filePath}");
                    continue;
                }
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    Console.WriteLine($"Writing file: {filePath}");
                    writer.Write(TEMPLATE_TEST_CLASS.Replace("$classNamespace", classNamespace).Replace("$className", className));
                    writer.Close();
                }
            }
            return 0;
        }
    }
}
