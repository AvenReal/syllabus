namespace Syllabus.syllabus;
using Newtonsoft.Json.Linq;

public class UE
{
    public string Nom;
    private List<ECUE> ECUEs;
    
    public float? MaxMoyenne{
        get{
            float sum = 0;
            float divisor = 0;
            foreach (var ecue in ECUEs)
            {
                if (ecue.MaxMoyenne != null)
                {
                    sum += (float)ecue.MaxMoyenne! * ecue.Coef;
                    divisor +=  ecue.Coef;
                }
                else
                {
                    sum += 20 * ecue.Coef;
                    divisor +=  ecue.Coef;
                }
            }
            return divisor == 0 ? null : sum / divisor;
            }
        }
    public bool? MaxValide
    {
        get
        {
            if (MaxMoyenne != null)
            {
                bool valide = MaxMoyenne >= 10;
                foreach (var ecue in ECUEs)
                {
                    if (ecue.MaxMoyenne != null)
                    {
                        valide = valide && ((bool)ecue.MaxValide!);
                    }
                }
                
                return valide;
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
            foreach (var ecue in ECUEs)
            {
                if (ecue.TempMoyenne != null)
                {
                    sum += (float)ecue.TempMoyenne! * ecue.Coef;
                    divisor +=  ecue.Coef;
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
                bool valide = TempMoyenne >= 10;
                foreach (var ecue in ECUEs)
                {
                    if (ecue.TempMoyenne != null)
                    {
                        valide = valide && ((bool)ecue.TempValide!);
                    }
                }
                return valide;
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
            foreach (var ecue in ECUEs)
            {
                if (ecue.MinMoyenne != null)
                {
                    sum += (float)ecue.MinMoyenne! * ecue.Coef;
                    divisor +=  ecue.Coef;
                }
                else
                {
                    sum += 0;
                    divisor +=  ecue.Coef;
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
                bool valide = MinMoyenne >= 10;
                foreach (var ecue in ECUEs)
                {
                    if (ecue.MaxMoyenne != null)
                    {
                        valide = valide && ((bool)ecue.MinValide!);
                    }
                }
                return valide;
            }
            return null;
        } 
    }
    
    public double? MinToValidate
    {
        get
        {
            double allCoef = 0;
            double knownNotesTimesCoef = 0;
            double unkownNotesCoef = 0;
            
            foreach (var ecue in ECUEs)
            {
                foreach (var note in ecue.GetNotes())
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
            }
            
            double? result = unkownNotesCoef == 0 ? null : (10 * allCoef - knownNotesTimesCoef) / unkownNotesCoef;
            return result < 0? 0: (result > 20? null: result);
        }
    }
    
    
    
    public UE(string nom, List<ECUE>? ecues = null)
    {
        Nom = nom;
        if (ecues == null)
        {
            ECUEs = new List<ECUE>();
        }
        else
        {
            ECUEs = ecues;
        }
    }


    public bool IsInECUEs(string NameOfECUE)
    {
        foreach (var ecue in ECUEs)
        {
            if (ecue.Nom == NameOfECUE)
            {
                return true;
            }
        }

        return false;
    }
    public ECUE GetECUE(string NameOfECUE)
    {
        foreach (var ecue in ECUEs)
        {
            if (ecue.Nom == NameOfECUE)
            {
                return ecue;
            }
        }

        throw new ArgumentException("ECUE not found");
    }
    public void AddECUE(ECUE ecue)
    {
        if (IsInECUEs(ecue.Nom))
        {
            RemoveECUE(ecue.Nom);
        }
        ECUEs.Add(ecue);
        ecue.Ue = this;
    }

    public List<ECUE> GetECUEs()
    {
        return ECUEs;
    }
    
    
    public void RemoveECUE(string NameOfECUE)
    {
        for (int i = 0; i < ECUEs.Count; i++)
        {
            if (ECUEs[i].Nom == NameOfECUE)
            {
                ECUEs.RemoveAt(i);
            }
        }
    }

    public override string ToString()
    {
        List<List<string>> result = new List<List<string>>
        {
            new List<string>{Nom},
            new List<string>{"SEPARATOR"},
            new List<string>{"Nom", $"Moyenne MIN", $"Moyenne Temp", "Moyenne Max"},
            new List<string>{"Movenne", $"{MinMoyenne}", $"{TempMoyenne}", $"{MaxMoyenne}"},
            new List<string>{"Valide", $"{MinValide}", $"{TempValide}", $"{MaxValide}"},
            new List<string>{"SEPARATOR"},
        };

        foreach (var ecue in ECUEs)
        {
            result.Add( new List<string>{ecue.Nom, ecue.MinMoyenne.ToString(), ecue.TempMoyenne.ToString(), ecue.MaxMoyenne.ToString()});
        }

        return Utils.FormatTable(result);
    }

    public JObject ToJson()
    {
        JObject jObject = new JObject();
        
        jObject.Add("Nom", Nom);
        
        JArray jArray = new JArray();
        foreach (var ecue in ECUEs)
        {
            jArray.Add(ecue.ToJson());
        }
        jObject.Add("ECUEs", jArray);

        return jObject;
    }

    public string ToSyllabus()
    {
        string ecues = "";
        foreach (var ecue in ECUEs)
        {
            ecues += ecue.ToSyllabus() + ";";
        }

        return $"{Nom.Replace(',', ' ').Replace('.', ' ')}|{ecues}";
    }
}