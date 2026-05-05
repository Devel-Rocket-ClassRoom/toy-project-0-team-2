using UnityEngine;
using UnityEngine.UI;

public abstract class UiBaseWindow : MonoBehaviour
{
    [Header("Bottom Buttons")]
    public Button BattleButton;   // 메인(전투 준비) 창으로 이동
    public Button CardDeckButton; // PlayerSelectButtons 팝업 열기
    public Button GachaButton;    // 가챠 창으로 이동

    [Header("PlayerSelect Buttons")]
    public GameObject PlayerSelectButtons;
    public Button Player1Button;
    public Button Player2Button;

    [Header("Player Colors")]
    public Color Player1Color = Color.cornflowerBlue;
    public Color Player2Color = Color.tomato;

    protected virtual void Awake()
    {
        PlayerSelectButtons.SetActive(false);

        BattleButton.onClick.AddListener(OnClickBattle);
        CardDeckButton.onClick.AddListener(OnClickCardDeck);
        GachaButton.onClick.AddListener(OnClickGacha);

        Player1Button.onClick.AddListener(() => OnSelectPlayer(0));
        Player2Button.onClick.AddListener(() => OnSelectPlayer(1));
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        OnShow();
    }

    public virtual void Hide()
    {
        PlayerSelectButtons.SetActive(false);
        gameObject.SetActive(false);
        OnHide();
    }

    protected virtual void OnShow() { }
    protected virtual void OnHide() { }

    private void OnClickBattle()
    {
        PlayerSelectButtons.SetActive(false);
        UiManager.Instance.ShowMain();
    }

    private void OnClickCardDeck()
    {
        PlayerSelectButtons.SetActive(true);
    }

    private void OnClickGacha()
    {
        PlayerSelectButtons.SetActive(false);
        UiManager.Instance.ShowGacha();
    }

    private void OnSelectPlayer(int playerIndex)
    {
        PlayerSelectButtons.SetActive(false);
        UiManager.Instance.ShowCard(playerIndex);
    }
}