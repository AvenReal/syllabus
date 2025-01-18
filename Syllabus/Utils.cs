using Spectre.Console;

namespace Syllabus;

public class Utils
{
    public static string FormatBoolean(bool? b, string param = "")
    {
        return $"[{param + (param != "" ? " " : "")}{(b == true ? "green" : "red")}]{b}[/]";
    }
    public static void PrintList(List<(string param, List<string> sublist)> list)
    {
        var header = list[0];
        bool headerhasparam = header.param != "";
        
        Table table = new Table();
        foreach (var elem in header.sublist)
        {
            table.AddColumn(new TableColumn($"{(headerhasparam ? $"[{header.param}]" : "")}{elem}{(headerhasparam ? "[/]" : "")}").Centered());
        }
        
        foreach (var (param, sublist) in list.GetRange(1, list.Count-1))
        {
            foreach (var elem in sublist)
            {
                bool hasparam = header.param != "";
                table.AddRow($"{(hasparam ? $"[{header.param}]" : "")}{elem}{(hasparam ? "[/]" : "")}");
            }
        }
        
        AnsiConsole.Write(table);
    }
    public static void SortListOfTuple(List<(int n, string)> l)
    {
        static bool IsSorted(List<(int n, string)> l)
        {
            for (var i = 0; i < l.Count-1; i++)
            {
                if (l[i].n > l[i + 1].n)
                {
                    return false;
                }
            }

            return true;
        }

        while (!IsSorted(l))
        {
            for (var i = 0; i < l.Count-1; i++)
            {
                if (l[i].n > l[i + 1].n)
                {
                    (l[i], l[i + 1]) = (l[i + 1], l[i]);
                }
            }
        }
    }
    public static string FormatTable(List<List<string>> data)
    {
        // Calculer la largeur maximale de chaque colonne
        int maxColumns = data.Max(row => row.Count);
        var columnWidths = new int[maxColumns];

        foreach (var row in data)
        {
            for (int i = 0; i < row.Count; i++)
            {
                columnWidths[i] = Math.Max(columnWidths[i], row[i].Length);
            }
        }

        // Construire le tableau formaté
        var result = new List<string>();

        // Ajouter la bordure supérieure
        int totalWidth = columnWidths.Sum() + (columnWidths.Length - 1) * 3; // 3 pour " | "
        result.Add(new string('-', totalWidth + 4)); // 4 pour les bords gauche et droit

        foreach (var row in data)
        {
            if (row.Count == 1 && row[0] == "SEPARATOR")
            {
                // Ligne de séparation
                result.Add("|" + new string('-', totalWidth + 2) + "|"); // 2 pour les espaces entre les bords
            }
            else
            {
                // Ligne normale
                var formattedRow = new List<string>();
                for (int i = 0; i < maxColumns; i++)
                {
                    string cell = i < row.Count ? row[i] : ""; // Si la cellule n'existe pas, utiliser une chaîne vide
                    formattedRow.Add(cell.PadRight(columnWidths[i]));
                }
                result.Add("| " + string.Join(" | ", formattedRow) + " |");
            }
        }

        // Ajouter la bordure inférieure
        result.Add(new string('-', totalWidth + 4));

        // Retourner le tableau formaté sous forme de chaîne de caractères
        return string.Join("\n", result);
    }
    
    public static string ReformatString(string a)
    {
        return a.ToLower().Replace('é', 'e').Replace('ï', 'i').Replace('è', 'e').Replace('à', 'a').Replace('\'', ' ').Replace('-', ' ').Replace('’', ' ');
    }
    
    public static int GetStringDistances(string a, string b)
    {
        (a, b) = (ReformatString(a), ReformatString(b));
        
        if (string.IsNullOrEmpty(a) && string.IsNullOrEmpty(b))
        {
            return 0;
        }
 
        if (string.IsNullOrEmpty(a))
        {
            return b.Length;
        }
 
        if (string.IsNullOrEmpty(b))
        {
            return a.Length;
        }
 
        int lengthA = a.Length;
        int lengthB = b.Length;
        var distances = new int[lengthA + 1, lengthB + 1];
 
        for (int i = 0; i <= lengthA; distances[i, 0] = i++);
        for (int j = 0; j <= lengthB; distances[0, j] = j++);
 
        for (int i = 1; i <= lengthA; i++)
        {
            for (int j = 1; j <= lengthB; j++)
            {
                int cost = b[j - 1] == a[i - 1] ? 0 : 1;
            
                distances[i, j] = Math.Min(
                    Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                    distances[i - 1, j - 1] + cost
                );
            }
        }
 
        return distances[lengthA, lengthB];
    }
}