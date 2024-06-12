using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;

internal class AssetDumper
{
    private static readonly Regex LANG_CONVERSION = new Regex(@"(?:%)([0-9])+(?:\$s)");

    private static void ExtractAssets(string path, string jarpath, List<string> onlyIncludeList, bool invert)
    {
        string sourcePath = jarpath;
        string assetsDir = Path.Combine(path, "assets");
        Console.WriteLine($"Assets source path: {sourcePath}");
        Console.WriteLine($"Assets path: {assetsDir}");

        if (!Directory.Exists(assetsDir)) Directory.CreateDirectory(assetsDir);
        FileStream stream = File.OpenRead(sourcePath);
        ZipArchive archive = new ZipArchive(stream);
        Console.WriteLine($"Extracting assets from {sourcePath} @ {assetsDir}");

        foreach (ZipArchiveEntry entry in archive.Entries)
        {
            string name = entry.FullName;
            if (name.StartsWith("META-INF/") || name.EndsWith(".class")) continue;
            if (invert)
            {
                if (onlyIncludeList != null && !onlyIncludeList.Contains(name)) continue;
            }
            else 
            {
                if (onlyIncludeList != null && onlyIncludeList.Contains(name)) continue;
            }

            string extractPath = Path.Combine(assetsDir, name);
            string dirName = Path.GetDirectoryName(extractPath);

            Console.WriteLine($"^ {name} -> {dirName}");
            if (!Directory.Exists(dirName)) Directory.CreateDirectory(dirName);

            if (name.EndsWith(".lang"))
            {
                Stream content = entry.Open();
                StreamReader reader = new StreamReader(content);
                FileStream fileOut = File.Open(extractPath, FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fileOut);

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string convertedLine = LANG_CONVERSION.Replace(line, (Match match) =>
                    {
                        return $"{{{int.Parse(match.Groups[1].Value) - 1}}}";
                    }).Replace("%s", "{0}");
                    writer.WriteLine(convertedLine);
                }

                writer.Flush();
                writer.Close();
                reader.Close();
            }
            else
            {
                entry.ExtractToFile(extractPath, true);
            }
        }

        stream.Close();
        stream.Dispose();
    }

    //the sha256 checksum of the b1.7.3 jar
    private const string jar_hash = "AF1FA04B8006D3EF78C7E24F8DE4AA56F439A74D7F314827529062D5BAB6DB4C";
    private static readonly List<string> EXCLUDES = new List<string>();
    static AssetDumper() 
    {
        //list of files to be excluded from client but included in core, and then included in client during build
        EXCLUDES.Add("achievement/map.txt");
        EXCLUDES.Add("font.txt");
        EXCLUDES.Add("lang/en_US.lang");
        EXCLUDES.Add("lang/stats_US.lang");
    }

    private static void Main(string[] args)
    {
        if (args.Length != 3) 
        {
            Console.Error.WriteLine("usage: assetdumper.exe <client jar path> <client destination assets path> <core destination assets path>");
            Console.Error.WriteLine("where core destination assets path is project_root/Core/");
            Console.Error.WriteLine("where client destination assets path is project_root/Client/");
            Environment.Exit(1);
        }
        string jarpath = args[0];
        string str = BitConverter.ToString(SHA256.HashData(File.ReadAllBytes(jarpath))).Replace("-", "");
        if (!str.Equals(jar_hash, StringComparison.InvariantCulture)) 
        {
            Console.Error.WriteLine("ERROR: The jar's hash doesn't match the expected value!");
            Environment.Exit(1);
        }
        string strclient = args[1]; //"..\\..\\..\\..\\Client\\";
        string strcore = args[2]; //"..\\..\\..\\..\\Core\\";
        ExtractAssets(strclient, jarpath, EXCLUDES, false);
        ExtractAssets(strcore, jarpath, EXCLUDES, true);
        Console.WriteLine("Assets extracted.");
    }
}
