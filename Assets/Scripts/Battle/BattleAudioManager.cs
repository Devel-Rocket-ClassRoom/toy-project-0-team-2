using UnityEngine;

public class BattleAudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip battleBgm;

    private void Start()
    {
        float volume = PlayerPrefs.GetFloat("BGM_VOLUME", 1f);
        bool mute = PlayerPrefs.GetInt("BGM_MUTE", 0) == 1;

        audioSource.volume = volume;
        audioSource.mute = mute;

        audioSource.clip = battleBgm;
        audioSource.loop = true;
        audioSource.Play();
    }
}