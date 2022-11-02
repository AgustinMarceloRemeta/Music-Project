using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] Slider sliderVolume;
    [SerializeField] Toggle toggleMute;
    [SerializeField] float maxValueVolume;
    private void Awake()
    {
        GetValues();
    }

    private void GetValues()
    {
        sliderVolume.value = PlayerPrefs.GetFloat("volume", maxValueVolume);
        toggleMute.isOn = (PlayerPrefs.GetInt("mute", 0) == 1 ? true : false);
    }

    public void ChangeLevel()
    {
        int sceneActive = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneActive>= SceneManager.sceneCount? sceneActive - 1 : sceneActive + 1);
    }
    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("volume", sliderVolume.value);
    }
    public void SaveToggle()
    {
        PlayerPrefs.SetInt("mute", (toggleMute.isOn ? 1 : 0));
    }
}
