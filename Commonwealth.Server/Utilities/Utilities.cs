using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Commonwealth.Shared.Common;
namespace Commonwealth.Server.Utilities;


public static partial class Util
{
    public static T? DeepCopyJSON<T>(T input)
    {
        var jsonString = JsonSerializer.Serialize(input);
        return JsonSerializer.Deserialize<T>(jsonString);
    }
    public static string ToLowerRemoveSpaces(this string source)
    {
        if (source is null) return string.Empty;
        string copy = source!.ToLower();
        return Regex.Replace(copy, @"\s+", string.Empty);
    }
    public static string TitleCase(string? source)
    {
        if (source is null) return string.Empty;
        if (char.IsUpper(source[0])) return source;

        // Convert first character to uppercase and append the rest
        return char.ToUpper(source[0]) + source[1..];
    }
    public static bool IsLegalWindowsFilename(string? filename)
    {
        if (string.IsNullOrWhiteSpace(filename)) return false;

        char[] invalidChars = Path.GetInvalidFileNameChars();
        foreach (char c in filename)
        {
            if (Array.Exists(invalidChars, invalidChar => invalidChar == c))
                return false;
        }
        return true;
    }
    // [GeneratedRegex("/s")]
    // private static partial Regex MyRegex();

    public static void Shuffle<T>(List<T>? source)
    {
        if (source is null) return;
        int n = source.Count;
        while (n > 1)
        {
            int k = Rand.Next(n--);
            (source[k], source[n]) = (source[n], source[k]);
        }
    }
    public static T? PickRandomFromList<T>(List<T>? source)
    {
        if (source is null || source.Count == 0) return default;
        int index = Rand.Next(0, source.Count);
        return source[index];
    }

    private static Random Rand = new Random();
    public static int GetRandomInclusive(Stat stat)
    {
        return Rand.Next(stat.Min, stat.Max);
    }

    public static int GetRandomInclusive(int min, int max)
    {
        return Rand.Next(min, max + 1);
    }
    public static bool GetRandomBool(decimal likelyhood)
    {
        if (likelyhood < 1) likelyhood *= 100;
        return (Rand.Next(0, 100) < likelyhood);
    }
    public static int GetUniqueRandomInt(List<int> existing, int max = 65535)
    {
        int test = 0;
        int attemptCounter = 0;
        int maxAttempts = 2 * max;
        do
        {
            test = GetRandomInclusive(0, max);
            attemptCounter++;
        } while (IsUnique(test) is false || attemptCounter < maxAttempts);
        if (attemptCounter >= maxAttempts) test = -1;
        return test;

        bool IsUnique(int test)
        {
            int index = existing.FindIndex(n => n == test);
            return (index == -1);
        }
    }
    public static List<E>? StringToEnumList<E>(List<string>? list)
    {
        if (list is null) return null;
        List<E> enumList = [];
        foreach (string item in list)
        {
            enumList.AddIfNotNull(StringToEnum<E>(item));
        }
        return enumList;
    }
    public static E? StringToEnum<E>(string? str)
    {
        if (str is null) return default(E);
        E? result;
        try
        {
            result = (E)Enum.Parse(typeof(E), str.ToUpper());
        }
        catch
        {
            result = default(E);
        }
        return result;
    }
    public static T IntToEnum<T>(int value)
    {
        if (Enum.IsDefined(typeof(T), value))
        {
            return (T)Enum.ToObject(typeof(T), value);
        }
        return default(T)!;
    }
    public static string GetEnumString<T>(T value) where T : struct, Enum
    {
        // Check if value is defined in the enum
        if (!Enum.IsDefined(typeof(T), value))
            return $"Undefined ({Convert.ToInt32(value)})";

        // Get field info
        var field = typeof(T).GetField(value.ToString());
        if (field != null)
        {
            // Check for Description attribute
            var attr = field.GetCustomAttribute<DescriptionAttribute>();
            if (attr != null)
                return attr.Description;
        }

        // Fallback to enum name
        return value.ToString();
    }
    public static string SinglePlural(int Count, string singular, string plural)
    {
        return (Count == 1) ? singular : plural;
    }
    public static string SignedWithPlus(int value)
    {
        return (value >= 0) ? $"+{value}" : value.ToString();
    }

    public static int? IntFromString(string? str)
    {
        if (string.IsNullOrEmpty(str)) return null;
        try
        {
            return int.Parse(str);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static List<string> ParseString(string source)
    {
        string cleaned = ToLowerRemoveSpaces(source);
        return cleaned.Split(",").ToList();
    }
    public static void AddIfNotNull<T>(this List<T> aList, T? obj)
    {
        if (obj is not null) aList.Add(obj);
    }
    public static bool AddIfNotDuplicate<T>(this List<T> aList, T? obj)
    {
        if (obj is null) return false;
        int index = aList.FindIndex(i => EqualityComparer<T>.Default.Equals(i, obj));
        if (index >= 0) return false;
        aList.Add((T)obj);
        return true;
    }
    public static void AddListWithoutDuplicate<T>(this List<T> target, List<T>? source)
    {
        foreach (T item in source ?? [])
        {
            target.AddIfNotDuplicate(item);
        }
    }
    public static string Dashed0(this int amount)
    {
        return (amount == 0) ? "-" : amount.ToString();
    }
    public static List<T> AssembleListFrom<T, U>(this List<U> source, Func<U, T?> extractFcn)
    {
        List<T> result = [];
        foreach (U item in source)
        {
            result.AddIfNotNull(extractFcn(item));
        }
        return result;
    }

    public static TDerived CopyToDerived<TBase, TDerived>(this TBase baseObj)
        where TDerived : TBase, new()
    {
        TDerived derivedObj = new TDerived();
        var baseProperties = typeof(TBase).GetProperties();
        foreach (var property in baseProperties)
        {
            if (property.CanRead && property.CanWrite)
            {
                var value = property.GetValue(baseObj);
                property.SetValue(derivedObj, value);
            }
        }
        return derivedObj;
    }
    public static int RoundWithGranularity(this int source, int granularity)
    {
        return (int)Math.Round(source / (float)granularity) * granularity;
    }
    public static double RoundDouble(this double source, double granularity)
    {
        return Math.Round(source / granularity, MidpointRounding.AwayFromZero) * granularity;
    }

    public static List<string> FindAnyNullProperties(object obj)
    {
        List<string> missing = [];
        if (obj == null) return missing;

        List<PropertyInfo> requiredProperties = obj.GetType().GetProperties()
        .Where(p => p.GetCustomAttribute<RequiredAttribute>() != null)
        .ToList();
        foreach (PropertyInfo info in requiredProperties)
        {
            var value = info.GetValue(obj);
            if (value is null)
            {
                missing.Add(info.Name);
            }
        }
        return missing;
        //  .All(prop =>
        //  {
        //      var value = prop.GetValue(obj);
        //      return value == null || (value is string str && string.IsNullOrEmpty(str));
        //  });
    }
    public static double Lookup(this List<LookupPoint> points, double inputX)
    {
        points.Sort((a, b) => a.X.CompareTo(b.X));
        if (inputX < points[0].X) return points[0].Y;
        int last = points.Count - 1;
        if (inputX > points[last].X) return points[last].Y;
        for (int i = 0; i < last; i++)
        {
            var (x1, y1) = points[i];
            var (x2, y2) = points[i + 1];

            if (inputX >= x1 && inputX <= x2)
            {
                // Linear interpolation formula
                return y1 + (inputX - x1) * (y2 - y1) / (x2 - x1);
            }
        }
        return points[last].Y;
    }

}
public class Stat
{
    public int Min { get; set; }
    public int Max { get; set; }
    [JsonConstructor] public Stat() { }
    [SetsRequiredMembers]
    public Stat(int min, int max)
    {
        Min = min;
        Max = max;
    }
}
