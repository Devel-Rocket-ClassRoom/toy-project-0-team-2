using UnityEngine;

public class SettingUI : MonoBehaviour
{
    public GameObject settingPanel;

    public void OpenSetting()
    {
        settingPanel.SetActive(true);
    }

    public void CloseSetting()
    {
        settingPanel.SetActive(false);
    }

    public void ChangeVolume(float volume)
    {

        TitleAudioManager.Instance.SetVolume(volume);
    }

    public void ToggleMute(bool mute)
    {
        TitleAudioManager.Instance.Mute(mute);
    }
}