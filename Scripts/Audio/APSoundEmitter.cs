using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    [RequireComponent(typeof(AudioSource))]
    public class APSoundEmitter : MonoBehaviour
    { 
        public APSoundData Data { get; private set; }

        AudioSource audioSource;
        Coroutine playingCoroutine;

        private void Awake()
        {
            if(audioSource == null)
                audioSource = GetComponent<AudioSource>();
        }

        public void Initialize(APSoundData data)
        {
            Data = data;
            audioSource.clip = data.clip;
            audioSource.outputAudioMixerGroup = data.mixerGroup;
            audioSource.loop = data.loop;
            audioSource.playOnAwake = data.playOnAwake;

            audioSource.mute = data.mute;
            audioSource.bypassEffects = data.bypassEffect;
            audioSource.bypassListenerEffects = data.bypassListenerEffects;
            audioSource.bypassReverbZones = data.bypassReverbZones;

            audioSource.priority = data.priority;
            audioSource.volume = data.volume;
            audioSource.pitch = data.pitch;
            audioSource.panStereo = data.panStereo;
            audioSource.spatialBlend = data.spatialBlend;
            audioSource.reverbZoneMix = data.reverbZoneMix;
            audioSource.dopplerLevel = data.dopperLevel;
            audioSource.spread = data.spread;

            audioSource.minDistance = data.minDistance;
            audioSource.maxDistance = data.maxDistance;

            audioSource.ignoreListenerVolume = data.ignoreListenerVolume;
            audioSource.ignoreListenerPause = data.ignoreListenersPause;

            audioSource.rolloffMode = data.rollOffMode;
        }
        
        #region Play
        public void Play()
        {
            if (playingCoroutine != null) 
            {
                StopCoroutine(playingCoroutine);
            }

            audioSource.Play();
            playingCoroutine = StartCoroutine(WaitForSoundToEnd());
        }

        IEnumerator WaitForSoundToEnd()
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            Stop();
        }

        public void WithRandomPitch()
        {
            audioSource.pitch += Random.Range(-Data.pitchRandomAmount, Data.pitchRandomAmount);
        }

        #endregion

        #region Stop
        public void Stop()
        {
            if (playingCoroutine != null)
            {
                StopCoroutine(playingCoroutine);
                playingCoroutine = null;
            }

            audioSource.Stop();
            APSoundManager.Instance.ReturnToPool(this);
        }
        #endregion

    }
}