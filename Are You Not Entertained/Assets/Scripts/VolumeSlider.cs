using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{

    public AudioMixer mixer;
    Slider volumeSlider;
    TMP_Text percentage;

    void Start()
    {
        volumeSlider = GetComponent<Slider>();
        percentage = GameObject.FindGameObjectWithTag("VolumePercentage").GetComponent<TMP_Text>();

        volumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("VolumeLevel", .75f));

        volumeSlider.onValueChanged.AddListener(SetLevel);

        percentage.SetText((Math.Round(PlayerPrefs.GetFloat("VolumeLevel", .75f) / volumeSlider.maxValue, 2) * 100).ToString() + '%');

        PlayerPrefs.Save();
    }

    void SetLevel(float value)
    {
        //mixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);

        percentage.SetText((Math.Round(value / volumeSlider.maxValue, 2) * 100).ToString() + '%');
        PlayerPrefs.SetFloat("VolumeLevel", value);
        PlayerPrefs.Save();
    }
}
