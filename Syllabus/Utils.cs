namespace Syllabus;

public class Utils
{
    public static string MultiplyChar(char c, int i)
    {
        string result = "";
        for (int j = 0; j < i; j++)
        {
            result += c;
        }

        return result;
    }
}