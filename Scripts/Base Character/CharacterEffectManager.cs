using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class CharacterEffectManager : MonoBehaviour
    {
        private CharacterManager character;

        public Transform characterEffectTransform;
        public Transform spineTransform;

        protected BaseEffectSO effect;
        private Vector3 position = Vector3.zero;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {
            
        }

        public virtual void DeployEffect(BaseEffectSO _effect)
        {
            effect = _effect;
            Setup();
        }

        //sets up the effect
        //goes through the possible positions, if everythings fails it will be at 0,0,0 world space
        protected virtual void Setup()
        {
            if (effect.useEffectTransform)
            {
                position = characterEffectTransform.position;
            }
            else if(effect.useSpineTransform)
            {
                position = spineTransform.position;
            }
            else if (effect.useVector)
            {
                position = characterEffectTransform.position + effect.Location;
            }
            else
            {
                position = characterEffectTransform.position;
            }

            if (effect.useVFX)
            {
                var deployedEffect = Instantiate(effect.VFX, position, Quaternion.identity);
                Destroy(deployedEffect, effect.DestroyTimer);
            }
            if (effect.useSFX)
            {
                character.charAudioManager.PlayCharacterSound(effect.soundData, position);
            }
        }

    }
}