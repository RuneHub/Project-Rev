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

        private FootstepSwapper swapper;

        [SerializeField] private AudioClip DodgeSound;
        [SerializeField] private AudioClip DashSound;

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
                PlaySoundFX(ref footstepAS, footstepAS.clip, 1, true);

                footstepSounds[cfs] = footstepSounds[0];
                footstepSounds[0] = footstepAS.clip;
            }
        }

        //checks the current platform that is jumped off
        //plays jumping sound
        public void PlayJumpSound()
        {
            swapper.CheckLayers();
            footstepAS.clip = jumpSound;
            footstepAS.Play();
        }

        //checks current platform landed on
        //plays landing sound
        public void PlayLandingSound()
        {
            swapper.CheckLayers();
            footstepAS.clip = LandSound;
            footstepAS.PlayOneShot(LandSound);
        }

        //plays the dodge sound when dodging
        public void PlayDodgeSound()
        {
            effectAS.PlayOneShot(DodgeSound);
        }
        #endregion

    }
}