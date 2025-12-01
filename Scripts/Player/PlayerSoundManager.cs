using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class PlayerSoundManager : CharacterSoundManager
    {
        private PlayerManager player;

        [Header("Footsteps")]
        [SerializeField] private AudioSource footstepAS;
        [SerializeField] private List<AudioClip> footstepSounds = new List<AudioClip>();
        [SerializeField] private AudioClip jumpSound;
        [SerializeField] private AudioClip LandSound;
        [SerializeField] private AudioClip dashSound;
        [SerializeField] private AudioClip sprintStopSound;
        [SerializeField, Range(0, 1)] private float FootStepSFXVolume = 1;

        private FootstepSwapper swapper;

        [SerializeField] private AudioClip dodgeSound;
        [SerializeField, Range(0, 1)] private float dodgeSFXVolume = 1;
        [SerializeField] private AudioClip justDodgeSound; 
        [SerializeField, Range(0, 1)] private float justDodgeSFXVolume = 1;

        [Space(10), SerializeField, Range(0,1)] private float WeaponSFXVolume = 1;

        [Space(10), SerializeField] private AudioClip bf_HolsterUnequip;
        [Space(10), SerializeField] private AudioClip bf_HolsterEquip;
        [SerializeField, Range(0, 1)] private float BodyFoleyVolume = 1;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

            swapper = GetComponent<FootstepSwapper>();
        }

        #region Footsteps
        //clears the current footstep sound list and swaps it out with the newly received ones.
        public void SwapFootsteps(FootstepCollection collection)
        {
            footstepSounds.Clear();
            for (int i = 0; i < collection.footstepSounds.Count; i++)
            {
                footstepSounds.Add(collection.footstepSounds[i]);
            }
            jumpSound = collection.jumpSound;
            LandSound = collection.landSound;
        }

        //checks the current walking platform for source type
        //play a random footstep sound from the received list
        public void PlayFootstepAudio()
        {
            if (player.isGrounded && !player.isInteracting)
            {
                swapper.CheckLayers();
                int cfs = Random.Range(1, footstepSounds.Count);
                footstepAS.clip = footstepSounds[cfs];
                PlaySoundFX(ref footstepAS, footstepAS.clip, FootStepSFXVolume, true, .1f);

                footstepSounds[cfs] = footstepSounds[0];
                footstepSounds[0] = footstepAS.clip;
            }
        }
        #endregion

        #region Locomotion
        //checks the current platform that is jumped off
        //plays jumping sound
        public void PlayJumpSound()
        {
            swapper.CheckLayers();
            footstepAS.clip = jumpSound;
            //footstepAS.Play();
            PlaySoundFX(ref footstepAS, footstepAS.clip, FootStepSFXVolume, false, 0);
        }

        //checks current platform landed on
        //plays landing sound
        public void PlayLandingSound()
        {
            swapper.CheckLayers();
            footstepAS.clip = LandSound;

            PlaySoundFX(ref footstepAS, footstepAS.clip, FootStepSFXVolume, false, 0);
        }

        //plays the dashing sound
        public void PlayDashSound()
        {
            footstepAS.clip = dashSound;
            PlaySoundFX(ref footstepAS, footstepAS.clip, FootStepSFXVolume, false, 0);
        }

        //plays the sfx for stopping with sprinting
        public void PlayerSprintStop()
        {
            footstepAS.clip = sprintStopSound;
            PlaySoundFX(ref footstepAS, footstepAS.clip, FootStepSFXVolume, false, 0);
        }

        //plays the dodge sound when dodging
        public void PlayDodgeSound()
        {
            effectAS.clip = dodgeSound;
            PlaySoundFX(ref effectAS, effectAS.clip, dodgeSFXVolume, false, 0);
        }

        //plays the Just dodge sound
        public void PlayJustDodgeSound()
        {
            effectAS.clip = justDodgeSound;
            PlaySoundFX(ref effectAS, effectAS.clip, justDodgeSFXVolume, false, 0);
        }
        #endregion

        #region Action

        public void PlayWeaponSound(AudioClip clip)
        {
            PlaySoundFX(ref actionAS, clip, volume: WeaponSFXVolume);
        }


        #endregion

        #region Foley

        public void PlayHolsterSound(bool remove)
        {
            if (remove)
            {
                FoleyAS.clip = bf_HolsterUnequip;
            }
            else 
            {
                FoleyAS.clip = bf_HolsterEquip;
            }

            PlaySoundFX(ref FoleyAS, FoleyAS.clip, BodyFoleyVolume, true, .1f);
        }
        #endregion

    }
}