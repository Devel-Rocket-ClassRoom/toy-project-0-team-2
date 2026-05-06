using UnityEngine;
using UnityEngine.UI;

public abstract class UiBaseWindow : MonoBehaviour
{
    [Header("Bottom Buttons")]
    public Button BattleButton;   // 메인(전투 준비) 창으로 이동
    public Button CardDeckButton; // PlayerSelectButtons 팝업 열기
    public Button GachaButton;    // 가챠 창으로 이동

    protected virtual void Awake()
    {
        BattleButton.onClick.AddListener(OnClickBattle);
        CardDeckButton.onClick.AddListener(OnClickCardDeck);
        GachaButton.onClick.AddListener(OnClickGacha);
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        OnShow();
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        OnHide();
    }

    protected virtual void OnShow() { }
    protected virtual void OnHide() { }

    private void OnClickBattle()
    {
        UiManager.Instance.ShowMain();
    }

    private void OnClickCardDeck()
    {
        UiManager.Instance.ShowCard();
    }

    private void OnClickGacha()
    {
        UiManager.Instance.ShowGacha();
    }
}