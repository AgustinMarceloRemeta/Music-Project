using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        SetVolume();
        SetMute();
    }

    public void SetVolume()
    {
        audioSource.volume = PlayerPrefs.GetFloat("volume", 1);
    }
    public void SetMute()
    {
        audioSource.mute = (PlayerPrefs.GetInt("mute", 0) == 1 ? true : false);
    } 
}
