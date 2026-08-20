namespace Commonwealth.Shared.EndpointDTOs;

public class AdminRequest : RequestBase
{
    public required string Action { get; set; }
    public string? Arg1 { get; set; }
    public string? Arg2 { get; set; }
}

public class AdminResponse : ResponseBase
{
    public required string Action { get; set; }
    public string? Type { get; set; }
    public string? Arg1 { get; set; }
    public string? Arg2 { get; set; }
    public List<string>? Items { get; set; }
    public string? JsonString { get; set; }
}

public class ParmUploadRequest
{
    public string RootFileName { get; set; } = "";
    public string Type { get; set; } = "";
    public string Payload { get; set; } = "";
}
public class ParmUploadResponse : ResponseBase
{

}
public static class Actions
{
    public const string UpdateParms = "updateParms";
    
    public const string GetList = "getList";
    public const string ViewFile = "viewFile";
    public const string RemoveFile = "removeFile";
    public const string MakeAdmin = "makeAdmin";
    public const string MakeDev = "makeDev";
    public const string GetGames = "getGames";

    public const string GetUserGames = "getUserGames";
    public const string GetGameUsers = "getGameUsers";
    public const string RemoveGame = "removeGame";
    public const string RemoveGameFromUser = "removeGameUser";


}
public static class FileTypes
{
    public const string Game = "Game";
    public const string User = "User";
    public const string Parm = "Parm";
    public const string UserGames = "userGames";
    public const string AllFiles = "AllFiles";
}
public static class ParmFileType
{
    public const string Econ = "econ";
    public const string Season = "season";
    public const string Init = "init";
    public const string Geog = "geog";
    public const string Naming = "names";
}
public static class ParmFileTypes
{
    public static List<string> AcceptableTypes = new() { ParmFileType.Econ, ParmFileType.Geog, ParmFileType.Init, ParmFileType.Naming };
}