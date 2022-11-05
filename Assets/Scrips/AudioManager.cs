using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    [SerializeField] float timeSave;
    [SerializeField] Text textSubtitle;
    [SerializeField] Scrollbar realTimeMusic;
    [SerializeField] List<Subtitle> subtitles;
    [SerializeField] List<float> timeSubtitles;
    AudioSource audioSource;
    Subtitle actualSubtitle;

    void Awake()
    {
        foreach (Subtitle item in FindObjectsOfType<Subtitle>()) subtitles.Add(item);
        audioSource = GetComponent<AudioSource>();
        SetVolume();
        SetMute();
        audioSource.time = PlayerPrefs.GetFloat("timeSound", 0);
    }

    private void Start()
    {
        InvokeRepeating("SaveTime", timeSave, timeSave);
        StartCoroutine(Subtitles(PlayerPrefs.GetFloat("timeSound", 0)));
    }

    private void Update() => realTimeMusic.size = audioSource.time / audioSource.clip.length;

    public void SetVolume()
    {
        if(audioSource != null) audioSource.volume = PlayerPrefs.GetFloat("volume", 1);
    }

    public void SetMute()
    {
        if (audioSource != null) audioSource.mute = (PlayerPrefs.GetInt("mute", 0) == 1 ? true : false);
    } 

    public void SaveTime() => PlayerPrefs.SetFloat("timeSound", audioSource.time); 

     IEnumerator Subtitles(float time)
    {
        Subtitle newSubtitle = GetNewSubtitle(time);
        textSubtitle.text = newSubtitle.subtitleText;
        actualSubtitle = newSubtitle;

        if (newSubtitle.end)
        {
            yield return new WaitForSeconds(audioSource.clip.length - time);
            StartCoroutine(Subtitles(0));
        }
        else
        {
            yield return new WaitForSeconds(newSubtitle.nextSubtitle.timeToInit - time);
            StartCoroutine(Subtitles(audioSource.time));
        }
    }

    private Subtitle GetNewSubtitle(float audioTime)
    {
        if (actualSubtitle != null)  return actualSubtitle.nextSubtitle;       
        else
        {
            if (timeSubtitles != null) timeSubtitles.Clear();

            foreach (Subtitle item in subtitles) if (item.timeToInit <= audioTime) timeSubtitles.Add(item.timeToInit);
            foreach (Subtitle item in subtitles) if (item.timeToInit == timeSubtitles.Max()) return item;
               
            return null;
        }
    }

    public void ResetMusic() => audioSource.time = 0;

    public void InitSubtitles()
    {
        actualSubtitle = null;
        StartCoroutine(Subtitles(audioSource.time));
    }


}

