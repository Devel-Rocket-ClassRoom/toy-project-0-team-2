using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiMainWindow : UiBaseWindow
{
    public Button StartGameButton;

    [Header("Player Display")]
    public TMP_Text PlayerNameText;
    public TMP_Text PlayerLevelText;
    public Image PlayerNameImage;
    public Image PlayerLevelImage;

    [Header("Player Toggle")]
    public Button PlayerToggleButton;
    public TMP_Text PlayerToggleText;
    public Image PlayerToggleImage;

    [Header("Gold & Gem")]
    public TMP_Text GoldText;
    public Image GoldImage;
    public Image GemImage;

    public int CurrentPlayerIndex { get; private set; } = 0;

    protected override void Awake()
    {
        base.Awake();

        StartGameButton.onClick.AddListener(OnClickStartGame);
        PlayerToggleButton.onClick.AddListener(OnClickPlayerToggle);
    }

    protected override void OnShow()
    {
        SetPlayer(0);
    }

    private void OnClickStartGame()
    {
        Debug.Log("[MainWindow] 게임 시작");
    }

    private void OnClickPlayerToggle()
    {
        SetPlayer(CurrentPlayerIndex == 0 ? 1 : 0);
    }

    private void SetPlayer(int playerIndex)
    {
        CurrentPlayerIndex = playerIndex;
        OnRefreshUi();

        Debug.Log($"[GachaWindow] Player {CurrentPlayerIndex + 1} 로 전환");
    }

    private void OnRefreshUi()
    {
        bool isP1 = CurrentPlayerIndex == 0;

        if (PlayerNameText != null)
        {
            PlayerNameText.text = isP1 ? "Player 1" : "Player 2";
        }
        if (PlayerToggleText != null)
        {
            PlayerToggleText.text = isP1 ? "Player 1" : "Player 2";
        }

        if (PlayerNameImage != null && PlayerLevelImage != null && PlayerToggleImage != null &&GoldImage != null && GemImage != null)
        {
            PlayerNameImage.color = isP1 ? Player1Color : Player2Color;
            PlayerLevelImage.color = isP1 ? Player1Color : Player2Color;
            PlayerToggleImage.color = isP1 ? Player1Color :Player2Color;
            GoldImage.color = isP1 ? Player1Color : Player2Color;
            GemImage.color = isP1 ? Player1Color : Player2Color;
        }

    }
}