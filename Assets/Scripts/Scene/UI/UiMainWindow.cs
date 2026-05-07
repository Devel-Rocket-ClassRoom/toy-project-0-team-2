using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiMainWindow : UiBaseWindow
{
    public Button StartGameButton;

    [Header("Player Display")]
    public TMP_Text PlayerLevelText;
    public Image PlayerLevelImage;

    [Header("Sound")]
    public AudioClip audio;
    public AudioSource audioSource;


    [Header("Gold & Gem")]
    public TMP_Text GoldText;
    public Image GoldImage;
    public Image GemImage;


    protected override void Awake()
    {
        base.Awake();

        StartGameButton.onClick.AddListener(OnClickStartGame);
        audioSource.clip = audio;
    }

    protected override void OnShow()
    {
        base.OnShow();
    }

    private void OnClickStartGame()
    {
        Debug.Log("[MainWindow] 게임 시작");
        audioSource.Play();
    }
}