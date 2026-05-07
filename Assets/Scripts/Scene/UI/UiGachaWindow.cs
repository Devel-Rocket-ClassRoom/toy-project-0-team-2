using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiGachaWindow : UiBaseWindow
{
    [Header("Gold & Gem")]
    public TMP_Text GoldText;
    public Image GoldImage;
    public Image GemImage;

    [Header("Gacha")]
    public Button NormalDrawButton;
    public Button RareDrawButton;

    private readonly int gold = 500;

    protected override void Awake()
    {
        base.Awake();

        NormalDrawButton.onClick.AddListener(OnClickDraw);
        RareDrawButton.onClick.AddListener(OnClickDraw);
    }

    protected override void OnShow()
    {
        OnRefreshGold();
    }

    private void OnRefreshGold()
    {
        if (GoldText == null)
        {
            return;
        }

        GoldText.text = gold.ToString("N0");
    }

    private void OnClickDraw()
    {
        Debug.Log($"[GachaWindow] — 1회 뽑기 실행");
    }
}