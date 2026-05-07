using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCardWindow : UiBaseWindow
{
    [Header("Player Display")]
    public TMP_Text PlayerNameText;
    public Image PlayerColorBar;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnShow()
    {
        OnRefreshCardList();
    }

    private void OnRefreshCardList()
    {
        Debug.Log($"[CardWindow] 카드 목록 갱신");
    }
}