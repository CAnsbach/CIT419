using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{


    public AudioSource audio;
    Slider volumeSlider;
    TMP_Text percentage;

    void Start()
    {
        //Get the volume slider
        volumeSlider = GetComponent<Slider>();

        //Get the textbox to show the percentage
        percentage = GameObject.FindGameObjectWithTag("VolumePercentage").GetComponent<TMP_Text>();

        //Set the solume slider to the preferred volume
        volumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("VolumeLevel", .75f));

        volumeSlider.onValueChanged.AddListener(SetLevel);

        //Set the text to the preferred volume
        percentage.SetText((Math.Round(PlayerPrefs.GetFloat("VolumeLevel", .75f) / volumeSlider.maxValue, 2) * 100).ToString() + '%');

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Function sets the volume of the audio and updates player preferences.
    /// </summary>
    /// <param name="value">value of the new preferred volume</param>
    void SetLevel(float value)
    {

        percentage.SetText((Math.Round(value / volumeSlider.maxValue, 2) * 100).ToString() + '%');
        audio.volume = value;        
        PlayerPrefs.SetFloat("VolumeLevel", value);
        PlayerPrefs.Save();
    }
}
