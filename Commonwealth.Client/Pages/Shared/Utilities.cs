
using System.Reflection;

namespace Commonwealth.Client;

public static partial class Utilities
{
    public static void CheckStringEntry(this List<string> errors, string? entry, string name, int max = 20, int min = 0)
    {
        if (string.IsNullOrWhiteSpace(entry)) errors!.Add($"Please Enter a {name}.");
        else
        {
            if (entry.Length > max) errors.Add($"{name} cannot be longer than {max} characters");
            if (entry.Length < min) errors.Add($"{name} cannot be shorter than {min} characters");
        }
    }
    public static void CheckForEmailAddress(this List<string> errors, string? target)
    {

        try
        {
            var addr = new System.Net.Mail.MailAddress(target ?? string.Empty);
            if (addr.Address == target) return; // Ensures no normalization changes
        }
        catch { }
        errors.Add("Must enter a valid Email address.");
    }

    //public static string Pluralize(this string singularForm, int howMany, string? pluralForm = null)
    //{
    //    pluralForm ??= singularForm + "s";
    //    return howMany.ToString() + " " + ((howMany == 1) ? singularForm : pluralForm);
    //}public static class ObjectConverter

    /// <summary>
    /// Converts an object of one type to another by copying matching public properties.
    /// </summary>
    public static T? ConvertTo<T>(this object source)
        where T : class, new()
    {
        if (source == null) return null;

        T target = new T();

        var sourceProps = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var targetProps = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in targetProps)
        {
            if (!prop.CanRead) continue;

            var sourceProp = Array.Find(sourceProps, p =>
                p.Name == prop.Name &&
                p.PropertyType == prop.PropertyType &&
                p.CanWrite);

            if (sourceProp != null)
            {
                var value = sourceProp.GetValue(source, null);
                prop.SetValue(target, value, null);
            }
        }

        return target;
    }
    private static readonly List<string> WordList = new List<string>
    {
        "apple", "river", "mountain", "sky", "forest", "ocean", "sun", "moon",
        "cloud", "tree", "flower", "stone", "wind", "fire", "earth", "star",
        "light", "shadow", "rain", "snow"
    };
    public static string GetRandomGameName()
    {
        Random rnd = new Random();

        // Pick first word
        string firstWord = WordList[rnd.Next(WordList.Count)].Capitalize();

        // Pick second word ensuring it's different
        string secondWord;
        do
        {
            secondWord = WordList[rnd.Next(WordList.Count)].Capitalize();
        } while (secondWord == firstWord);

        return $"{firstWord}{secondWord}";

    }
    public static string Capitalize(this string input)
    {
        return char.ToUpper(input[0]) + input.Substring(1);
    }
}



