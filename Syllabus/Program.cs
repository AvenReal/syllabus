// See https://aka.ms/new-console-template for more information



using Syllabus;
using Syllabus.syllabus;
using Syllabus.files;
using Avalonia;
using Spectre.Console;


//AnsiConsole.Markup("[underline red]hello[/] world");

Total tt = Files.ImportSyllabus("files/S1-moi.syllabus");
tt.Print();


while (true)
{
    Commands.GetCommand(Console.ReadLine());
    Console.Clear();
}

Total total = Syllabus.files.Files.ImportSyllabus("S1-moi.syllabus");

Console.WriteLine(total.ToString());


Console.WriteLine(Commands.GetClosestString(Console.ReadLine(), total.GetUENoms() ));


