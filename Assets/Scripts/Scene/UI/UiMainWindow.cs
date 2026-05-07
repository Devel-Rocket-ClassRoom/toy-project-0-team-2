using UnityEngine;
using UnityEngine.UI;

public class UiMainWindow : UiBaseWindow
{
    public Button StartGameButton;

    [Header("Sound")]
    public AudioClip audioClip;
    public AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();

        StartGameButton.onClick.AddListener(OnClickStartGame);
        audioSource.clip = audioClip;
    }

    protected override void OnShow()
    {
        base.OnShow();
    }

    private void OnClickStartGame()
    {
        UiManager.Instance.CardWindow.SetIntoDeck();
        Debug.Log("[MainWindow] 게임 시작");
        audioSource.Play();
    }
}