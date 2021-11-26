using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AudioManagerScript;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;
    [SerializeField] Button vibration;

    AudioManager audioMngReference;

    void Start()
    {
        audioMngReference = AudioManager.Instance;

        musicSlider.value = 0.1f;
        soundSlider.value = 0.1f;

        musicSlider.onValueChanged.AddListener(OnMusicSliderMoved);
        soundSlider.onValueChanged.AddListener(OnSoundSliderMoved);
    }

    public void OnMusicSliderMoved(float value)
    {
        audioMngReference.SetMasterVolMusic(value);
    }

    public void OnSoundSliderMoved(float value)
    {
        audioMngReference.SetMasterVolSound(value);
    }
}
