using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class CharacterEffectManager : MonoBehaviour
    {
        private CharacterManager character;

        public Transform CharacterEffectTransform;

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
                position = CharacterEffectTransform.position;
            }
            else if (effect.useVector)
            {
                position = CharacterEffectTransform.position + effect.Location;
            }
            else
            {
                position = CharacterEffectTransform.position;
            }

            if (effect.useVFX)
            {
                var deployedEffect = Instantiate(effect.VFX, position, Quaternion.identity);
                Destroy(deployedEffect, effect.DestroyTimer);
            }
            if (effect.useSFX)
            {
                character.charAudioManager.PlayEffectSound(effect.SFX);
            }
        }

    }
}