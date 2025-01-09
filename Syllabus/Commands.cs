using System.Diagnostics;
using Syllabus.syllabus;

namespace Syllabus;

public class Commands
{
    public Total Total;
    
    
    
    
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