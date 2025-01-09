using Newtonsoft.Json.Linq;

namespace Syllabus.syllabus;
using Syllabus;

public class ECUE
{
    public string Nom;
    public int Coef;

    private int _seuil;
    public int Seuil
    {
        set
        {
            if (value > 20 || value < 0)
            {
                throw new ArgumentException("Seuil doit etre entre 0 et 20");
            }
            else
            {
                _seuil = value;
            }
        }
        get
        {
            return _seuil;
        }
    }

    private List<Note> Notes;
    public UE Ue;

    public double? MinToValidate
    {
        get
        {
            double allCoef = 0;
            double knownNotesTimesCoef = 0;
            double unkownNotesCoef = 0;
            foreach (var note in Notes)
            {
                allCoef += note.Coef;
                if (note.TempNote != null)
                {
                    knownNotesTimesCoef += ((double)note.TempNote) * note.Coef;
                }
                else
                {
                    unkownNotesCoef += note.Coef;
                }
            }

            double? result = unkownNotesCoef == 0 ? null : (Seuil * allCoef - knownNotesTimesCoef) / unkownNotesCoef;
            return result < 0? 0: (result > 20? null: result);
        }
    }
    public float? MaxMoyenne
    {
        get
        {
            {
                float sum = 0;
                float divisor = 0;
                foreach (var note in Notes)
                {
                    if (note.TempNote != null)
                    {
                        sum += (float)note.TempNote! * note.Coef;
                        divisor += note.Coef;
                    }
                    else
                    {
                        sum += 20 * note.Coef;
                        divisor += note.Coef;
                    }

                }
                return divisor == 0 ? null : sum / divisor;
            }
        }
    }
    public bool? MaxValide
    {
        get
        {
            if (MaxMoyenne != null)
            {
                return MaxMoyenne >= Seuil;
            }

            return null;
        }
    }
    
    public float? TempMoyenne
    {
        get
        {
            float sum = 0;
            float divisor = 0;
            foreach (var note in Notes)
            {
                if (note.TempNote != null)
                {
                    sum += (float)note.TempNote! * note.Coef;
                    divisor +=  note.Coef;
                }
            
            }

            return divisor == 0 ? null : sum / divisor;
        }
    }
    public bool? TempValide
    {
        get
        {
            if (TempMoyenne != null)
            {
                return TempMoyenne >= Seuil;
            }

            return null;
        }
    }
    
    public float? MinMoyenne
    {
        get
        {
            float sum = 0;
            float divisor = 0;
            foreach (var note in Notes)
            {
                if (note.TempNote != null)
                {
                    sum += (float)note.TempNote! * note.Coef;
                    divisor +=  note.Coef;
                }
                else
                {
                    sum += 0;
                    divisor +=  note.Coef;
                }
            
            }

            return divisor == 0 ? null : sum / divisor;
        }
    }
    public bool? MinValide
    {
        get
        {
            if (MinMoyenne != null)
            {
                return MinMoyenne >= Seuil;
            }

            return null;
        }
    }

    public ECUE(string nom, int coef, int seuil, UE ue, List<Note>? notes = null)
    {
        Nom = nom;
        Coef = coef;
        Seuil = seuil;
        Ue = ue;
        if (notes != null)
        {
            Notes = notes;
        }
        else
        {
            Notes = new List<Note>();
        }
        
    }
    public bool IsInNotes(string NameOfNote)
    {
        foreach (var note in Notes)
        {
            if (note.Nom == NameOfNote)
            {
                return true;
            }
        }

        return false;
    }
    public Note GetNote(string NameOfNote)
    {
        foreach (var note in Notes)
        {
            if (note.Nom == NameOfNote)
            {
                return note;
            }
        }

        throw new ArgumentException("Note not found");
    }
    public void AddNote(Note note)
    {
        if (IsInNotes(note.Nom))
        {
            Notes.Remove(note);
        }
        Notes.Add(note);
        note.Ecue = this;
        note.Ue = Ue;
    }
    public void RemoveNote(Note note)
    {
        if (Notes.Contains(note))
        {
            Notes.Remove(note);
        }
    }
    public override string ToString()
    {
        List<List<string>> result = new List<List<string>>
        {
            new List<string>{Nom, $"Seuil: {Seuil}"},
            new List<string>{"SEPARATOR"},
            new List<string>{$"Moyenne MAX: {MaxMoyenne}", $"Valide: {MaxValide}"},
            new List<string>{$"Moyenne TEMP: {TempMoyenne}", $"Valide: {TempValide}"},
            new List<string>{$"Moyenne MIN: {MinMoyenne}", $"Valide: {MinValide}"},
            new List<string>{"SEPARATOR"},
            new List<string>{"Nom:", "Note:", "Coef:"}
        };

        foreach (var note in Notes)
        {
            result.Add( new List<string>{note.Nom, note.TempNote.ToString(), note.Coef.ToString()});
        }

        return Utils.FormatTable(result);
    }

    public JObject ToJson()
    {
        JObject jObject = new JObject();
        jObject.Add("Nom", Nom);
        jObject.Add("Coef", Coef);
        jObject.Add("Seuil", Seuil);

        JArray jArray = new JArray();
        foreach (var note in Notes)
        {
            jArray.Add(note.ToJson());
        }
        jObject.Add("Notes", jArray);

        return jObject;
    }
    public string ToSyllabus()
    {
        string notes = "";
        foreach (var note in Notes)
        {
            notes += note.ToSyllabus() + "+";
        }
        return $"{Nom.Replace(',', ' ').Replace('.', ' ')}:{Coef}:{Seuil}:{notes}";
    }

    public List<Note> GetNotes()
    {
        return Notes;
    }
}