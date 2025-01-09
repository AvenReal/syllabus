using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Syllabus.syllabus;

namespace Syllabus.files;

public class Files
{
    public static void ExportJson(Total total, string path)
    {
        JObject totalJson = total.ToJson();
        StreamWriter file = new StreamWriter($"{path}.json");
        JsonWriter jsonWriter = new JsonTextWriter(file);
        totalJson.WriteTo(jsonWriter);
        jsonWriter.Close();
        file.Close();
    }
    public static void ExportSyllabus(Total total, string path)
    {
        StreamWriter file = new StreamWriter(path + ".syllabus");
        foreach (var ue in total.UEs)
        {
            file.WriteLine(ue.ToSyllabus());
        }
        file.Close();
    }
    
    

    public static Total ImportJson(string path)
    {
        StreamReader file = new StreamReader(path);
        JsonReader jsonReader = new JsonTextReader(file);

        Total total = JsonConvert.DeserializeObject<Total>(file.ReadToEnd());
        
        
        
        jsonReader.Close();
        file.Close();
        return total;
    }

    public static Total ImportSyllabus(string path)
    {
        StreamReader file = new StreamReader(path);

        Total result = new Total();
        foreach (var ues in file.ReadToEnd().Split('\n'))
        {
            if (ues != "")
            {
                string[] ueArray = ues.Split('|');
                string nomUe = ueArray[0];
                string ecuesString = ueArray[1];


                UE ue = new UE(nomUe);
                result.UEs.Add(ue);
                foreach (var ecues in ecuesString.Split(';'))
                {
                    if (ecues != "")
                    {
                        string[] ecueArray = ecues.Split(':');
                        string nomEcue = ecueArray[0];
                        string coefEcue = ecueArray[1];
                        string seuilEcue = ecueArray[2];
                        string notesString = ecueArray[3];

                        ECUE ecue = new ECUE(nomEcue, int.Parse(coefEcue), int.Parse(seuilEcue), ue);
                        ue.AddECUE(ecue);
                        foreach (var notes in notesString.Split('+'))
                        {
                            if (notes != "")
                            {
                                string[] noteArray = notes.Split('*');
                                string nomNote = noteArray[0];
                                string coefNote = noteArray[1];
                                string tempnoteNote = noteArray[2];

                                Note note = new Note(nomNote, int.Parse(coefNote), ecue, ue, tempnoteNote == "null" ? null : double.Parse(tempnoteNote));
                                ecue.AddNote(note);
                            }
                            
                        }
                    }
                    

                }
            }
            
        }
        file.Close();
        return result;
    }
}