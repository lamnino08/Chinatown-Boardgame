using Colyseus.Schema;

public class LobbyPopupManager : BasePopupManager
{
    public new static LobbyPopupManager instance
    {
        get { return BasePopupManager.instance as LobbyPopupManager; }
    }

    public void ShowChoseColorPopup(ArraySchema<bool> availableColors)
    {
        ChoseColorManagerPopup popup = GetPopup<ChoseColorManagerPopup>();
        if (popup != null)
        {
            popup.OnShow(availableColors);
        }
    }

    public void Toast (string content)
    {
        Toast popup = GetPopup<Toast>();
        if (popup != null)
        {
            popup.Show(content);
        }
    }

    public void OpenLobby()
    {
        MenuUIManager popup = GetPopup<MenuUIManager>();
        if (popup != null)
        {
            popup.Close();
        }
        LobbyUIManager popuploby = GetPopup<LobbyUIManager>();
        if (popuploby != null)
        {
            popuploby.Show();
        }
    }
}
