using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCardWindow : UiBaseWindow
{
    [Header("Player Display")]
    public TMP_Text PlayerNameText;
    public Image PlayerColorBar;

    public int CurrentPlayerIndex { get; private set; } = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetPlayer(int playerIndex)
    {
        CurrentPlayerIndex = playerIndex;
    }

    protected override void OnShow()
    {
        OnRefreshUi();
        OnRefreshCardList();
    }

    private void OnRefreshUi()
    {
        bool isP1 = CurrentPlayerIndex == 0;

        if (PlayerNameText != null)
        {
            PlayerNameText.text = isP1 ? "Player 1" : "Player 2";
        }

        if (PlayerColorBar != null)
        {
            PlayerColorBar.color = isP1 ? Player1Color : Player2Color;
        }

    }

    private void OnRefreshCardList()
    {
        Debug.Log($"[CardWindow] Player {CurrentPlayerIndex + 1} 카드 목록 갱신");
    }
}