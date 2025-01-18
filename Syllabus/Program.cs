// See https://aka.ms/new-console-template for more information



using Syllabus;
using Syllabus.syllabus;
using Syllabus.files;
using Spectre.Console;


//AnsiConsole.Markup("[underline red]hello[/] world");

Total tt = Files.ImportSyllabus("files/S1-moi.syllabus");
//tt.GetUE("Agir").GetECUE("Cybersécurité").GetNote("TP").TempNote = 20;
//tt.GetUE("Concevoir").GetECUE("Suites réelles").GetNote("EXAM-B2-SR").TempNote = 13;
tt.Print();
Console.WriteLine(tt.GetAllMinToValidate());
foreach (var ue in tt.UEs)
{
    ue.Print();
    foreach (var ecue in ue.GetECUEs())
    {
        ecue.Print();
    }
}

while (true)
{
    Commands.GetCommand(Console.ReadLine());
    Console.Clear();
}

Total total = Syllabus.files.Files.ImportSyllabus("S1-moi.syllabus");

Console.WriteLine(total.ToString());


Console.WriteLine(Commands.GetClosestString(Console.ReadLine(), total.GetUENoms() ));


