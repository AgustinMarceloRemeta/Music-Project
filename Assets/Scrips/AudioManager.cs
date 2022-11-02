using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [SerializeField]AudioSource audioSource;
    [SerializeField] float timeSave;
    [SerializeField] List<Subtitle> subtitles; 
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        SetVolume();
        SetMute();
        audioSource.time = PlayerPrefs.GetFloat("timeSound", 0);
    }
    private void Start()
    {
        InvokeRepeating("SaveTime", timeSave, timeSave);
    }
    private void Update()
    {
        
    }

    public void SetVolume()
    {
        if(audioSource != null)
        audioSource.volume = PlayerPrefs.GetFloat("volume", 1);
    }
    public void SetMute()
    {
        if (audioSource != null)
            audioSource.mute = (PlayerPrefs.GetInt("mute", 0) == 1 ? true : false);
    } 

    public void SaveTime()
    {
        PlayerPrefs.SetFloat("timeSound", audioSource.time); 
    }

    IEnumerator Subtitles()
    {
        foreach (var subtitle in subtitles)
        {
            
        }
        yield return new WaitForSeconds(2);
    }
}
public class Subtitle : MonoBehaviour
{
    float timeToInit;
    string subtitleText;
    Subtitle nextSubtitle;
}
