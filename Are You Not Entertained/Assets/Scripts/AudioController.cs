using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    AudioSource audio;

    private void Start()
    {
        audio = this.GetComponent<AudioSource>();

        //Set the volume equal to the player's preferred volume level
        audio.volume = PlayerPrefs.GetFloat("VolumeLevel", .75f);
    }
}
