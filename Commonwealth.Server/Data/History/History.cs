using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Commonwealth.Server.Utilities;

namespace Commonwealth.Server.Data;


public partial class History: IBlobObject
{
 //   public required Season Season { get; set; }
    public required List<Nation> Nations { get; set; }
//    public required List<NationStatus>? NationStatuses { get; set; }
    public required Game Game { get; set; }
    [JsonConstructor] public History(){}
    [SetsRequiredMembers]
    public History(Game game, List<Nation> nations)
    {
        Game = game;
        Nations = nations;
    }
    private const string HistorySuffix = "-history-";
    private static string FileName(string gameName, int seasonCount)
    {
        return gameName + HistorySuffix + seasonCount;
    }


    // public async Task SaveAsync(BlobService blobService)
    // {
    //     string fileName = FileName(Game.Name!, Season.SeasonCount);
    //     await blobService.SaveJsonAsync(Folders.Games, Game.Name, fileName, this);
    //     //   await FileIO.UpdateJsonFileAsync(Folders.Games, fileName, this);
    // }
    // public static async Task RemoveAllAsync(string? gameName)
    // {
    //     await FileIO.DeleteJsonFilesWildcardAsync(Folders.Games, gameName, HistorySuffix);
    // }
}