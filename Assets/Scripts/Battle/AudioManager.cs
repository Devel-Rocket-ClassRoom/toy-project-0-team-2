using UnityEngine;

public class TitleAudioManager : MonoBehaviour
{
    public static TitleAudioManager Instance;

    public AudioSource audioSource;
    public AudioClip titleBgm;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        audioSource.volume = 1f;
        audioSource.mute = false;

        PlayTitleBgm();
    }

    public void PlayTitleBgm()
    {
        if (audioSource.isPlaying && audioSource.clip == titleBgm)
        {
            return;
        }

        Debug.Log("노래변경");

        audioSource.clip = titleBgm;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat("BGM_VOLUME", volume);
    }

    public void Mute(bool mute)
    {
        audioSource.mute = mute;
        PlayerPrefs.SetInt("BGM_MUTE", mute ? 1 : 0);
    }

    public void OnDestroy()
    {
        Instance = null;
    }
}