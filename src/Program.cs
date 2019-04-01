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
        private static readonly List<Regex> EXCLUDED_DIRECTORIES = new List<Regex>() {
            new Regex("[Oo]bj/"),
            new Regex("[Bb]in/")
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
            return EXCLUDED_DIRECTORIES.Any(d => d.Match(startPath).Success);
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
            foreach (var codeFile in codeFiles)
            {
                var treeDirectory = GetTreeDirectory(codeFilesPath, codeFile);
                var classNamespace = $"{rootNamespace}{treeDirectory.Replace("/", ".")}";
                var className = $"{Path.GetFileNameWithoutExtension(codeFile)}Test";
                var fileName = $"{className}.cs";
                var pathTestFile = testFilesPath + treeDirectory + "/" + fileName;
                Directory.CreateDirectory(testFilesPath + treeDirectory);
                if (File.Exists(pathTestFile))
                {
                    Console.WriteLine($"File already exist {pathTestFile}");
                    continue;
                }
                using (StreamWriter writer = new StreamWriter(pathTestFile, false, Encoding.UTF8))
                {
                    writer.Write(TEMPLATE_TEST_CLASS.Replace("$classNamespace", classNamespace).Replace("$className", className));
                    writer.Close();
                }
            }
            return 0;
        }
    }
}
