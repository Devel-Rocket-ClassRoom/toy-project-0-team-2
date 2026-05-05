using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiGachaWindow : UiBaseWindow
{
    [Header("Player Toggle")]
    public Button PlayerToggleButton;
    public TMP_Text PlayerToggleText;
    public Image PlayerToggleImage;

    [Header("Gold & Gem")]
    public TMP_Text GoldText;
    public Image GoldImage;
    public Image GemImage;

    [Header("Gacha")]
    public Button NormalDrawButton;
    public Button RareDrawButton;

    private readonly int[] golds = { 1000, 500 };

    public int CurrentPlayerIndex { get; private set; } = 0;

    protected override void Awake()
    {
        base.Awake();

        PlayerToggleButton.onClick.AddListener(OnClickPlayerToggle);
        NormalDrawButton.onClick.AddListener(OnClickDraw);
        RareDrawButton.onClick.AddListener(OnClickDraw);
    }

    protected override void OnShow()
    {
        SetPlayer(0);
    }

    private void OnClickPlayerToggle()
    {
        SetPlayer(CurrentPlayerIndex == 0 ? 1 : 0);
    }

    private void SetPlayer(int playerIndex)
    {
        CurrentPlayerIndex = playerIndex;
        OnRefreshUi();
        OnRefreshGold();

        Debug.Log($"[GachaWindow] Player {CurrentPlayerIndex + 1} 로 전환");
    }

    private void OnRefreshUi()
    {
        bool isP1 = CurrentPlayerIndex == 0;

        if (PlayerToggleText != null)
        {
            PlayerToggleText.text = isP1 ? "Player 1" : "Player 2";
        }

        if (PlayerToggleImage != null)
        {
            PlayerToggleImage.color = isP1 ? Player1Color : Player2Color;
            GoldImage.color = isP1 ? Player1Color : Player2Color;
            GemImage.color = isP1 ? Player1Color : Player2Color;
        }
    }

    private void OnRefreshGold()
    {
        if (GoldText == null)
        {
            return;
        }

        int gold = golds[CurrentPlayerIndex];
        GoldText.text = gold.ToString("N0");
    }

    private void OnClickDraw()
    {
        Debug.Log($"[GachaWindow] Player {CurrentPlayerIndex + 1} — 1회 뽑기 실행");
    }
}