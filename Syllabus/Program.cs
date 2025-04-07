// See https://aka.ms/new-console-template for more information



using Syllabus;
using Syllabus.syllabus;
using Syllabus.files;
using Spectre.Console;


Total tt = Files.ImportSyllabus("files/S2-moi.syllabus");

Commands.Total = tt;
tt.Print();
var a = new List<(string, List<(string, List<(string, string)> )> )>{
    ("Agir", new List<(string, List<(string, string)>)>
    {
        ( "Comprehention" , new List<(string, string)>
        {
            ("Eval-B3", "16,5"), 
            ("Exam-B3", "17,2")
        }),
        ( "Expression" , new List<(string, string)>
        {
            ("Projet-B4", "16,5")
        }),
        ( "Synthese" , new List<(string, string)>
        {
            ("QCM", "10,5"),
            ("EXPOSE", "13"),
            ("TOEIC", "14,65"),
            ("written", "14"),
        }),
        ( "AR / VR" , new List<(string, string)>
        {
            ("PROJET", "20"),
            ("QCM1", "20"),
        }),
    })
};

foreach (var (ue, ecues) in a)
{
    foreach (var (ecue, notes) in ecues)
    {
        foreach (var (note, number) in notes)
        {
            Commands.GetCommand($"set {ue} {ecue} {note} {number}");
        }
    }
}


Console.WriteLine(tt.GetAllMinToValidate());
while (true)
{
    Commands.GetCommand(Console.ReadLine());
    Console.Clear();
}

Total total = Syllabus.files.Files.ImportSyllabus("S1-moi.syllabus");

Console.WriteLine(total.ToString());


Console.WriteLine(Utils.GetClosestString(Console.ReadLine(), total.GetUENoms() ));


