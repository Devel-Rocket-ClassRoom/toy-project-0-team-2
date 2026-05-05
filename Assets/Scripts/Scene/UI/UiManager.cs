using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [Header("Windows")]
    public UiMainWindow MainWindow;
    public UiCardWindow CardWindow;
    public UiGachaWindow GachaWindow;

    private UiBaseWindow _currentWindow;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ShowMain();
    }

    public void ShowMain() => SwitchTo(MainWindow);

    public void ShowCard(int playerIndex)
    {
        CardWindow.SetPlayer(playerIndex);
        if (_currentWindow == CardWindow)
        {
            CardWindow.Show();
        }
        else
        {
            SwitchTo(CardWindow);
        }
    }

    public void ShowGacha() => SwitchTo(GachaWindow);

    private void SwitchTo(UiBaseWindow next)
    {
        if (_currentWindow == next) return;

        _currentWindow?.Hide();
        _currentWindow = next;
        _currentWindow.Show();
    }
}