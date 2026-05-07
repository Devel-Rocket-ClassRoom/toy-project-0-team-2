using UnityEngine;
using UnityEngine.UI;

public class UiMainWindow : UiBaseWindow
{
    public Button StartGameButton;

    protected override void Awake()
    {
        base.Awake();

        StartGameButton.onClick.AddListener(OnClickStartGame);
    }

    protected override void OnShow()
    {
        base.OnShow();
    }

    private void OnClickStartGame()
    {
        UiManager.Instance.CardWindow.SetIntoDeck();
        Debug.Log("[MainWindow] 게임 시작");
    }
}