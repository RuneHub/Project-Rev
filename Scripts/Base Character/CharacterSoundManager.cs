using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class CharacterSoundManager : MonoBehaviour
    {
        private CharacterManager character;

        [Header("Action")]
        [SerializeField] protected AudioSource actionAS;

        [Header("Effect")]
        [SerializeField] protected AudioSource effectAS;

        [Header("Voice")]
        [SerializeField] protected AudioSource voiceAS;

        [Header("Foley")]
        [SerializeField] protected AudioSource FoleyAS;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public void PlaySoundFX(ref AudioSource audioSource, AudioClip soundFX, float volume = 1, bool randomizePitch = true,float pitchRandom = .1f)
        {
            audioSource.pitch = 1;
            if (randomizePitch)
            {
                audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
            }

            audioSource.PlayOneShot(soundFX, volume);
        }

        public void PlayActionSound(AudioClip clip,float vol = 1)
        {
            PlaySoundFX(ref actionAS, clip, volume: vol);
        }

        public void PlayEffectSound(AudioClip clip, float vol = 1)
        {
            PlaySoundFX(ref effectAS, clip, volume: vol);
        }

        public void PlayVoiceSound(AudioClip clip, float vol = 1)
        {
            PlaySoundFX(ref voiceAS, clip, volume: vol);
        }

        public void PlayFoleySound(AudioClip clip, float vol = 1)
        {
            PlaySoundFX(ref FoleyAS, clip, volume: vol);
        }

    }
}