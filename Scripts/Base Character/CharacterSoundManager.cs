using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class CharacterSoundManager : MonoBehaviour
    {
        private CharacterManager character;

        [Header("Effect")]
        [SerializeField] protected AudioSource effectAS;

        [Header("Foley")]
        [SerializeField] protected AudioSource FoleyAS;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public void PlaySoundFX(ref AudioSource audioSource, AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = .1f)
        {
            audioSource.pitch = 1;
            if (randomizePitch)
            {
                audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
            }

            audioSource.PlayOneShot(soundFX, volume);
        }

        public void PlayEffectSound(AudioClip clip, float vol = 1)
        {
            PlaySoundFX(ref effectAS, clip, volume: vol);
        }

        public void PlayFoleySound(AudioClip clip, float vol = 1)
        {
            PlaySoundFX(ref FoleyAS, clip, volume: vol);
        }

        public void PlayCharacterSound(APSoundData data, Vector3 position)
        { 
            APSoundManager.Instance.CreateSound()
                .WithSoundData(data)
                .WithRandomPitch()
                .WithPosition(position)
                .Play();
        }

    }
}