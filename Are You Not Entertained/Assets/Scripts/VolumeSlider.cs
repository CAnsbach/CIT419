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

        volumeSlider.onValueChanged.AddListener(SetLevel);
    }

    void SetLevel(float value)
    {
        //mixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);

        percentage.SetText((Math.Round(value / volumeSlider.maxValue, 2) * 100).ToString() + '%');
    }
}
