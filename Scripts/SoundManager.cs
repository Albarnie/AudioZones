using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Albarnie.AmbientSound
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField]
        AudioMixer mixer;

        AudioSource mainAudioSource;
        AudioSource blendingAudioSource;

        [SerializeField, Range(0, 1)]
        float blend;

        [SerializeField]
        AudioZone currentZone;
        AudioZone lastZone;

        [SerializeField]
        AudioZone defaultZone;

        public bool useDefaultZone;

        private void Awake()
        {
            AudioSource[] sources = GetComponents<AudioSource>();

            mainAudioSource = sources[0];
            blendingAudioSource = sources[1];
        }

        private void Update()
        {
            if (blend < 1 && lastZone != null)
            {
                DoBlend();
            }
        }

        public void DoBlend()
        {
            float mainVolume;
            float blendingVolume;

            blend += Time.deltaTime / currentZone.blendTime;

            //lerp the volumes based on the blend
            // at a blend of 0, the main volume will be 0 and the blending volume will be full
            // at a blend of 1, it is reversed
            mainVolume = Mathf.Lerp(0, currentZone.volume, blend);
            blendingVolume = Mathf.Lerp(lastZone.volume, 0, blend);

            mainAudioSource.volume = mainVolume;
            blendingAudioSource.volume = blendingVolume;
        }


        public void StartBlend(AudioZone newZone)
        {
            //Update the current and last zones
            lastZone = currentZone;
            currentZone = newZone;

            blendingAudioSource.volume = mainAudioSource.volume;
            blendingAudioSource.clip = lastZone.ambientSound;

            //Set the blending audio time to where the last clip was
            float audioTime = mainAudioSource.time;

            mainAudioSource.volume = 0;
            mainAudioSource.clip = currentZone.ambientSound;
            mainAudioSource.time = 0;

            mainAudioSource.Play();
            blendingAudioSource.Play();

            blendingAudioSource.time = audioTime;

            currentZone.snapshot.TransitionTo(lastZone.blendTime);

            blend = 0;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("AudioVolume"))
            {
                AudioVolume volume = other.GetComponent<AudioVolume>();
                if (currentZone != volume.zone)
                {
                    StartBlend(volume.zone);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("AudioVolume"))
            {
                //if we want to use a default zone and the zone we are leaving is the last one we entered, blend to default
                AudioVolume volume = other.GetComponent<AudioVolume>();
                if (currentZone == volume.zone)
                {
                    if (useDefaultZone)
                    {
                        StartBlend(defaultZone);
                    }
                    else
                    {
                        StartBlend(lastZone);
                    }
                }
            }
        }
    }
}