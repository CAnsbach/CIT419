using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    AudioSource audio;

    private void Start()
    {
        audio = this.GetComponent<AudioSource>();

        audio.volume = PlayerPrefs.GetFloat("VolumeLevel", .75f);
    }
}
