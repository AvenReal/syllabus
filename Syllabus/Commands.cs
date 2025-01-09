using System.Diagnostics;
using System.Reflection.Metadata;
using Syllabus.files;
using Syllabus.syllabus;

namespace Syllabus;

public class Commands
{
    public static Total? Total;

    public static void GetCommand(string input)
    {
        string[] arguments = input.Split(' ')[1..];
        string command = Utils.ReformatString(input.Split(' ')[0]);
        string result = "";
        bool help = false;
        
        if (command == "help")
        {
            command = arguments[0];
            arguments = arguments[1..];
            help = true;
        }
        
        
        
        Console.Clear();
        switch (command)
        {
            
            
            case "import":
                result = CommandImport(arguments, help);
                break;
            case "export":
                result = CommandExport(arguments, help);
                break;
            
            default:
                Console.WriteLine($"Command {input} not Found");
                break;
            
        }

        Console.WriteLine(result);
        Console.WriteLine();
    }

    public static string CommandImport(string[] arguments, bool help = false)
    {
        if (help || arguments.Length != 1)
        {
            return (arguments.Length != 1 ? "Error: " : "") +
                   "import [<name>.json/<name.syllabus>] \nRead the <name> file.";
        }
        if (arguments[0].Contains(".syllabus"))
        {
            Total = Files.ImportSyllabus(arguments[0]);
            return Total.ToString();
        }
        else if (arguments[0].Contains(".json"))
        {
            Total = Files.ImportJson(arguments[0]);
            return Total.ToString();
        }
        else
        {
            return $"Extention {arguments[0]} introuvable.";
        }
    }
    public static string CommandExport(string[] arguments, bool help = false)
    {
        if (help || arguments.Length != 2)
        {
            return (arguments.Length != 2 ? "Error: " : "") +
                   "export [json/syllabus] <name> \nSave the curent syllabus to a file .json ou .syllabus";
        }

        if (Total != null)
        {
            if (arguments[0] == "json")
            {
                Files.ExportJson(Total, arguments[1]);
                return "File Exported Successfully";
            }
            else if (arguments[0] == "syllabus")
            {
                Files.ExportSyllabus(Total, arguments[1]);
                return "File Exported Successfully";
            }
            else
            {
                return $"Error No such {arguments[0]} file extention (only json or syllabus is possible)";
            }
        }
        else
        {
            return "No file to export";
        }
    }
    public static string GetClosestString(string input, List<string> l)
    {
        List<(int dist, string word)> dict = new List<(int dist, string word)>();
        foreach (var word in l)
        {
            int dist = Utils.GetStringDistances(input, word);
            if (dist == 0)
            {
                return word;
            }
            else
            {
                dict.Add((dist, word));
            }
        }
        
        Utils.SortListOfTuple(dict);
        while (true)
        {
            foreach (var (_, word) in dict)
            {
                Console.Write($"You wrote {input} did\n you mean {word}? [y/n]:");
                if (Console.ReadLine() == "y")
                {
                    return word;
                }
            }
        }
    }
}