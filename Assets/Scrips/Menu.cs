using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] Slider _sliderVolume;
    [SerializeField] Toggle _toggleMute;
    [SerializeField] float _maxValueVolume;
    private void Awake() => GetValues();
    
    private void GetValues()
    {
        _sliderVolume.value = PlayerPrefs.GetFloat("volume", _maxValueVolume);
        _toggleMute.isOn = (PlayerPrefs.GetInt("mute", 0) == 1 ? true : false);
    }

    public void ChangeLevel()
    {
        int sceneActive = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneActive>= SceneManager.sceneCount? sceneActive - 1 : sceneActive + 1);
    }

    public void SaveVolume() => PlayerPrefs.SetFloat("volume", _sliderVolume.value);
    
    public void SaveToggle() => PlayerPrefs.SetInt("mute", (_toggleMute.isOn ? 1 : 0));

    public void Reset() => PlayerPrefs.SetFloat("timeSound", 0);

    public void Default()
    {
        _toggleMute.isOn = false;
        _sliderVolume.value = _maxValueVolume;
    }

}
