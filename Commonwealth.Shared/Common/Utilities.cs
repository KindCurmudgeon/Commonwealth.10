namespace Commonwealth.Shared.Common;
 public static partial class Utilities {   
    public static string Pluralize(this string singularForm, int howMany, string? pluralForm = null)
    {
        pluralForm ??= singularForm + "s";
        return howMany.ToString() + " " + ((howMany == 1) ? singularForm : pluralForm);
    }
 }