using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    [SerializeField] float _timeSave;
    [SerializeField] Text _textSubtitle;
    [SerializeField] Scrollbar _realTimeMusic;
    [SerializeField] List<Subtitle> _subtitles;
    [SerializeField] List<float> _timeSubtitles;
    AudioSource _audioSource;
    Subtitle _actualSubtitle;

    void Awake()
    {
        foreach (Subtitle item in FindObjectsOfType<Subtitle>()) _subtitles.Add(item);
        _audioSource = GetComponent<AudioSource>();
        SetVolume();
        SetMute();
        _audioSource.time = PlayerPrefs.GetFloat("timeSound", 0);
    }

    private void Start() => StartCoroutine(Subtitles(PlayerPrefs.GetFloat("timeSound", 0)));
    

    private void Update() => _realTimeMusic.size = _audioSource.time / _audioSource.clip.length;

    public void SetVolume()
    {
        if(_audioSource != null) _audioSource.volume = PlayerPrefs.GetFloat("volume", 1);
    }

    public void SetMute()
    {
        if (_audioSource != null) _audioSource.mute = (PlayerPrefs.GetInt("mute", 0) == 1 ? true : false);
    } 

    public void SaveTime() => PlayerPrefs.SetFloat("timeSound", _audioSource.time); 

     IEnumerator Subtitles(float time)
    {
        Subtitle newSubtitle = GetNewSubtitle(time);
        _textSubtitle.text = newSubtitle.subtitleText;
        _actualSubtitle = newSubtitle;

        if (newSubtitle.end)
        {
            yield return new WaitForSeconds(_audioSource.clip.length - time);
            StartCoroutine(Subtitles(0));
        }
        else
        {
            yield return new WaitForSeconds(newSubtitle.nextSubtitle.timeToInit - time);
            StartCoroutine(Subtitles(_audioSource.time));
        }
    }

    private Subtitle GetNewSubtitle(float audioTime)
    {
        if (_actualSubtitle != null)  return _actualSubtitle.nextSubtitle;       
        else
        {
            if (_timeSubtitles != null) _timeSubtitles.Clear();

            foreach (Subtitle item in _subtitles) if (item.timeToInit <= audioTime) _timeSubtitles.Add(item.timeToInit);
            foreach (Subtitle item in _subtitles) if (item.timeToInit == _timeSubtitles.Max()) return item;
               
            return null;
        }
    }

    public void ResetMusic() => _audioSource.time = 0;

    public void InitSubtitles()
    {
        _actualSubtitle = null;
        StartCoroutine(Subtitles(_audioSource.time));
    }

    public void ButtonPlay()
    {
        if (!_audioSource.isPlaying) _audioSource.Play();
    }

    void OnApplicationQuit() => SaveTime();
    
} 