 using System.Text.Json;
using Newtonsoft.Json.Linq;
using Spectre.Console;

namespace Syllabus.syllabus;

public class Total
{
    public List<UE> UEs;

    public float? MaxMoyenne
    {
        get
        {
            {
                float sum = 0;
                float divisor = 0;
                foreach (var ue in UEs)
                {
                    if (ue.MaxMoyenne != null)
                    {
                        sum += (float)ue.MaxMoyenne!;
                        divisor += 1;
                    }
                    else
                    {
                        sum += 20;
                        divisor += 1;
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
            bool? result = null;
            foreach (var ue in UEs)
            {
                if (ue.MaxValide != null)
                {
                    result = (result == null ? true : (bool)result) && (bool)ue.MaxValide!;
                }
            }

            return result;
        }
    }
    
    public float? TempMoyenne
    {
        get
        {
            {
                float sum = 0;
                float divisor = 0;
                foreach (var ue in UEs)
                {
                    if (ue.TempMoyenne != null)
                    {
                        sum += (float)ue.TempMoyenne!;
                        divisor += 1;
                    }
                }
                return divisor == 0 ? null : sum / divisor;
            }
        }
    }
    public bool? TempValide
    {
        get
        {
            bool? result = null;
            foreach (var ue in UEs)
            {
                if (ue.TempValide != null)
                {
                    result = (result == null ? true : (bool)result) && (bool)ue.TempValide!;
                }
            }

            return result;
        }
    }
    
    public float? MinMoyenne
    {
        get
        {
            {
                float sum = 0;
                float divisor = 0;
                foreach (var ue in UEs)
                {
                    if (ue.MinMoyenne != null)
                    {
                        sum += (float)ue.MinMoyenne!;
                        divisor += 1;
                    }
                    else
                    {
                        divisor += 1;
                    }

                }
                return divisor == 0 ? null : sum / divisor;
            }
        }
    }
    public bool? MinValide
    {
        get
        {
            bool? result = null;
            foreach (var ue in UEs)
            {
                if (ue.MinValide != null)
                {
                    result = (result == null ? true : (bool)result) && (bool)ue.MinValide!;
                }
            }

            return result;
        }
    }

    public Total(List<UE>? ues = null)
    {
        if (ues == null)
        {
            UEs = new List<UE>();
        }
        else
        {
            UEs = ues;
        }
    }


    public override string ToString()
    {
        List<List<string>> result = new List<List<string>>
        {
            new List<string> { "", "Minimum", "Minimum Valide", "Temporaire", "Temporaire Valide", "Maximum", "Maximum Valide" },
            new List<string> { "SEPARATOR" },
            new List<string> { "Total", $"{MinMoyenne}", $"{MinValide}", $"{TempMoyenne}", $"{TempValide}", $"{MaxMoyenne}", $"{MaxValide}" },
            new List<string> { "SEPARATOR" }
        };

        foreach (var ue in UEs)
        {
            result.Add(new List<string>
            {
                ue.Nom, 
                ue.MinMoyenne.ToString()!, ue.MinValide.ToString()!,
                ue.TempMoyenne.ToString()!, ue.TempValide.ToString()!,
                ue.MaxMoyenne.ToString()!, ue.MaxValide.ToString()!
                
            });
        }

        return Utils.FormatTable(result);
    }
    public JObject ToJson()
    {
        JObject jObject = new JObject();

        JArray jArray = new JArray();

        foreach (var ue in UEs)
        {
            jArray.Add(ue.ToJson());
        }
        jObject.Add("UEs", jArray);

        return jObject;
    }

    public void Print()
    {
        Table table = new Table(); 
        table.AddColumns(new string[] { "", "", "Minimum", "Minimum Valide", "", "Temporaire", "Temporaire Valide", "", "Maximum", "Maximum Valide" });
        table.AddRow("[bold]Total[/]", "", $"[bold]{MinMoyenne}[/]", Utils.FormatBoolean(MinValide, "bold"), "", $"[bold]{TempMoyenne}[/]", Utils.FormatBoolean(TempValide, "bold"), "", $"[bold]{MaxMoyenne}[/]", Utils.FormatBoolean(MaxValide, "bold"));
        table.AddEmptyRow();
        
        
        foreach (var ue in UEs)
        {
            table.AddRow(ue.Nom, "", ue.MinMoyenne.ToString()!, Utils.FormatBoolean(ue.MinValide), "", ue.TempMoyenne.ToString()!,
                Utils.FormatBoolean(ue.TempValide), "", ue.MaxMoyenne.ToString()!, Utils.FormatBoolean(ue.MaxValide));
        }
        
        table.Width(null).Centered();
        AnsiConsole.Write(table);
    }

    public UE GetUE(string nom)
    {
        foreach (var ue in UEs)
        {
            if (ue.Nom == nom)
            {
                return ue;
            }
        }

        throw new ArgumentException();
    }

    public string GetAllMinToValidate()
    {
        List<List<string>> result = new List<List<string>>();
        foreach (var ue in UEs)
        {
            foreach (var ecue in ue.GetECUEs())
            {
                foreach (var note in ecue.GetNotes())
                {
                    if (note.TempNote == null)
                    {
                        double? minToValidade = note.MinToValidate;
                        result.Add(new List<string>{ ue.Nom, ecue.Nom, note.Nom, (minToValidade == ecue.Seuil ? "=":(minToValidade > ecue.Seuil? "-":"+")), minToValidade.ToString() });
                    }
                }
            }
        }

        return Utils.FormatTable(result);
    }

    public List<string> GetUENoms()
    {
        List<string> result = new List<string>();
        foreach (var ue in UEs)
        {
            result.Add(ue.Nom);
        }

        return result;
    }
}