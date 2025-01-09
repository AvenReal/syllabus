// See https://aka.ms/new-console-template for more information


using Syllabus;
using Syllabus.syllabus;
using Syllabus.files;



Total total = Syllabus.files.Files.ImportSyllabus("S1-moi.syllabus");

Console.WriteLine(total.ToString());


Console.WriteLine(Commands.GetClosestString(Console.ReadLine(), total.GetUENoms() ));


