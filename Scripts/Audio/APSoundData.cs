using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace KS
{
    [Serializable]
    public class APSoundData
    {
        public AudioClip clip;
        public AudioMixerGroup mixerGroup;
        public bool loop;
        public bool playOnAwake;
        public bool frequentSound;

        public bool mute;
        public bool bypassEffect;
        public bool bypassListenerEffects;
        public bool bypassReverbZones;

        [Range(0,256)] public int priority = 128;
        [Range(0, 1)]  public float volume = 1f;
        [Range(-3, 3)] public float pitch = 1f;
        [Range(0, 1)]  public float pitchRandomAmount = 0.1f;
        [Range(-1, 1)] public float panStereo = 0f;
        [Range(0, 1)]  public float spatialBlend = 0f;
        [Range(0, 1.1f)] public float reverbZoneMix = 1f;
        [Range(0, 5)] public float dopperLevel = 1f;
        [Range(0, 360)] public float spread = 0;

        public float minDistance = 1f;
        public float maxDistance = 500f;

        public bool ignoreListenerVolume;
        public bool ignoreListenersPause;

        public AudioRolloffMode rollOffMode = AudioRolloffMode.Logarithmic;


    }
}