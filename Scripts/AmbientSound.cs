using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    AudioSource source;

    [SerializeField]
    float minPitch = 0.9f,
        maxPitch = 1.1f;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        source.pitch *= Random.Range(minPitch, maxPitch);

        source.time = Random.Range(0, source.clip.length);
    }
}
