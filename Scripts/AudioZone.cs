using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class AudioZone
{
    public string name = "AudioZone";

    public float volume = 1f;

    public AudioMixerSnapshot snapshot;
    public AudioClip ambientSound;

    public float blendTime = 5f;
}
