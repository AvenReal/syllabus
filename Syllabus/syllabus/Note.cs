using System.Runtime.InteropServices.ComTypes;
using Newtonsoft.Json.Linq;

namespace Syllabus.syllabus;

public class Note
{
    public string Nom;

    private double? _tempNote;
    public double? TempNote
    {
        set
        {
            if (value > 20 || value < 0)
            {
                throw new ArgumentException("Note doit etre entre 0 et 20");
            }
            else
            {
                _tempNote = value;
            }
        }
        get
        {
            return _tempNote;
        }
    }
    public int Coef;

    public ECUE Ecue;
    public UE Ue;

    public double? MinToValidate
    {
        get
        {
            if (TempNote != null)
            {
                return TempNote;
            }
            
            
            
            double? ueMinToValidate = Ue.MinToValidate;
            double? ecueMinToValidate = Ecue.MinToValidate;

            //if (ecueMinToValidate < 0)
            //{
            //    return 0;
            //}
            //else
            //{
            //    return ecueMinToValidate;
            //}
            
            double? result = null;
            
            switch ((ueMinToValidate, ecueMinToValidate))
            {
                case (null, null):
                    return 0;
                case (null, _):
                    return ecueMinToValidate;
                case (_, null):
                    return ecueMinToValidate;
                default:
                    double maxMinToValidate = Double.Max((double)ueMinToValidate, (double)ecueMinToValidate);
                    if (maxMinToValidate > 20)
                    {
                        return null;
                    }
                    else
                    {
                        return maxMinToValidate;
                    }
            }
        }
    }
    
    
    
    public Note(string nom, int coef, ECUE ecue, UE ue, double? tempNote = null)
    {
        Nom = nom;
        TempNote = tempNote;
        Coef = coef;
        Ecue = ecue;
        Ue = ue;
    }
    

    public JObject ToJson()
    {
        JObject jObject = new JObject();
        jObject.Add("Nom", Nom);
        jObject.Add("Coef", Coef);
        jObject.Add("TempNote", TempNote);
        return jObject;
    }

    public string ToSyllabus()
    {
        return $"{Nom.Replace(',', ' ').Replace('.', ' ')}*{Coef}*{((TempNote == null) ? ("null") : (TempNote))}";
    }
}