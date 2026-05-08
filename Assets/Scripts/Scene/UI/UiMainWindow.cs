using UnityEngine;
using UnityEngine.UI;

public class UiMainWindow : UiBaseWindow
{
    public Button StartGameButton;



    protected override void OnShow()
    {
        base.OnShow();
    }

    public void OnClickStartGame()
    {
        UiManager.Instance.CardWindow.SetIntoDeck();
        Debug.Log("[MainWindow] 게임 시작");

    }
}