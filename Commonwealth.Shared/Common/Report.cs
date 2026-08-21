
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Commonwealth.Shared.Common;

public partial class Report
{
    public required string Title { get; set; }
    public required List<ReportEntry> Entries { get; set; }

    [JsonConstructor] public Report() { }
    [SetsRequiredMembers]
    public Report(string title)
    {
        Title = title;
        Entries = [];
    }
    public void AddTextEntry(string? text)
    {
        if (text is null) return;
        Entries.Add(new ReportEntry(ENTRYTYPE.TEXT) { Text = text});
    }
    public void AddTitledList(string title, List<string>? listItems)
    {
        Entries.Add(new ReportEntry(ENTRYTYPE.LIST) { Text = title, ListItems = listItems });
    }
    public void AddTable(ReportTableData tableData)
    {
        Entries.Add(new ReportEntry(ENTRYTYPE.TABLE) { TableData = tableData });
    }


}
public class ReportEntry
{
    public ENTRYTYPE RowType { get; set; }
    public string? Text { get; set; }
    public List<string>? ListItems { get; set; }
    public ReportTableData? TableData { get; set; }
    public ReportEntry(ENTRYTYPE type) { RowType = type; }
    [JsonConstructor] public ReportEntry(){}
}
public class ReportListItem
{
    public string? Text { get; set; }
    public List<ReportListItem>? Children { get; set; }
    public ReportListItem(string? text, List<ReportListItem>? children = null)
    {
        Text = text;
        Children = children;
    }
}
public class ReportTableData
{
    public string Title { get; set; }
    public required string FirstHeading { get; set; }
    public required List<string> ColumnHeadings { get; set; }
    public required List<RowData> RowDatas { get; set; }
    public ReportTableData(string title)
    {
        Title = title;
    }
}
public enum ENTRYTYPE { NONE, TEXT, LIST, TABLE }
public record RowData(string Header, List<string> Items);
public static class ReportExtenstions
{
    //public static void AddItem(this List<ReportListItem> items, string item)
    //{
    //    items.Add(new ReportListItem(item));
    //}
    //public static void AddTitledList(this Report report, string title, List<string>? strings)
    //{
    //    List<ReportListItem> reportItems = [];
    //    if (strings is null || strings.Count == 0) reportItems.Add(new ReportListItem("None"));
    //    else
    //    {
    //        foreach (string s in strings)
    //        {
    //            reportItems.Add(new ReportListItem(s));
    //        }
    //    }
    //    report.AddItem(title, reportItems);
    //}
    //public static void AddTitledItems(this Report report, string title, List<ReportItem> items)
    //{
    //    report.AddItem(title, items);
    //}
    // public static string Pluralize(this string singularForm, int howMany, string? pluralForm = null)
    // {
    //     pluralForm ??= singularForm + "s";
    //     return howMany.ToString() + " " + ((howMany == 1) ? singularForm : pluralForm);
    // }

}