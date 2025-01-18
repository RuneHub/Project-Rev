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

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public void PlaySoundFX(ref AudioSource audioSource, AudioClip soundFX, float volume = 1, bool randomizePitch = true,float pitchRandom = .1f)
        {
            audioSource.PlayOneShot(soundFX, volume);
            audioSource.pitch = 1;
            if (randomizePitch)
            {
                audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
            }
        }

        public void PlayActionSound(AudioClip clip)
        {
            PlaySoundFX(ref actionAS, clip);
        }

        public void PlayEffectSound(AudioClip clip)
        {
            PlaySoundFX(ref effectAS, clip);
        }

        public void PlayVoiceSound(AudioClip clip)
        {
            PlaySoundFX(ref voiceAS, clip);
        }

    }
}