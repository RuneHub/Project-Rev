using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class APSoundBuilder
    {
        readonly APSoundManager soundManager;
        APSoundData soundData;
        Vector3 position = Vector3.zero;
        bool randomPitch;

        public APSoundBuilder(APSoundManager soundManager)
        {
            this.soundManager = soundManager;
        }

        public APSoundBuilder WithSoundData(APSoundData soundData) 
        {
            this.soundData = soundData;
            return this;
        }

        public APSoundBuilder WithPosition(Vector3 position)
        {
            this.position = position;
            return this;
        }

        public APSoundBuilder WithRandomPitch()
        {
            this.randomPitch = true;
            return this;
        }

        public void Play()
        {
            if (!soundManager.CanPlaySound(soundData))
                return;

            APSoundEmitter soundEmitter = soundManager.Get();
            soundEmitter.Initialize(soundData);
            soundEmitter.transform.position = position;
            soundEmitter.transform.parent = soundManager.transform;

            if (randomPitch)
            {
                soundEmitter.WithRandomPitch();
            }

            if (soundData.frequentSound)
            {
                soundManager.frequentSoundEmitters.AddLast(soundEmitter);
            }

            soundEmitter.Play();

        }

    }
}