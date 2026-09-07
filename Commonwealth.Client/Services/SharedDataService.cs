using Microsoft.AspNetCore.Components;
using Commonwealth.Shared.EndpointDTOs;
using Commonwealth.Client.Layout;
using Commonwealth.Shared.Common;


namespace Commonwealth.Client.Services;

public class SharedDataService
{
    public string? PlayerName { get; private set; } = null;
    public string? Token { get; private set; } = null;
    public NationIdentity? NationIdentity { get; set; }
    //  public Guid? GameId { get; set; }

    private MainLayout? _mainLayout;
    private ApiService? _apiService;

    public void SetPlayerName(string? name)
    {
        PlayerName = name;
    }
    // public bool IsSuccessfulSignin()
    // {
    //     if (PlayerProfile is null || Token is null) return false;
    //     return true;
    // }
    public void SetToken(string? token)
    {
        Token = token;
    }
    public void SetMainLayout(MainLayout layout)
    {
        _mainLayout = layout;
    }
    public void SetApiService(ApiService apiService)
    {
        _apiService = apiService;
    }
    public void CheckAlerts(ResponseBase? response)
    {
        if (response is null)
        {
            string? nav = NavigateToAfterAlert;
            NavigateToAfterAlert = null;
            if (nav is not null) NavMgr?.NavigateTo(nav);
            return;
        }
        if (response.Response.Messages.Count > 0)
        {
            _mainLayout?.ShowAlert(response);
        }
    }
    public void ShowErrorAlert(string message)
    {
        ResponseBase responseBase = new();
        responseBase.Response.Messages.Add(message);
        responseBase.Response.Status = false;
        _mainLayout?.ShowAlert(responseBase);
    }
    public string? NavigateToAfterAlert { get; set; }

    public NavigationManager? NavMgr { get; set; }
    public string? GameName { get; set; }

    //public bool AllowEdit { get; set; }
    //public void ShowGameEditor(string? gameName, bool allowEdit = false)
    //{
    //    GameName = gameName;
    //    AllowEdit = allowEdit;
    //    NavMgr?.NavigateTo(PageLink.Game);
    //}

    public void ShowReport(Report? report)
    {
        _mainLayout?.ReportPopupRef.Show(report);
    }
}

