using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS {
    public class CharacterManager : MonoBehaviour
    {
        [Header("properties references")]
        public CharacterController controller;
        public Animator animator;

        [Header("script references")]
        public CharacterAnimationManager charAnimationManger;
        public CharacterLocomotionManager charLocomotionManager;
        public CharacterStatsManager charStatManager;
        public CharacterSoundManager charAudioManager;
        public CharacterEffectManager charEffectManager;

        [Header("character booleans")]
        public bool isInteracting;
        public bool isCancellable;
        public bool isInvulnerable;
        public bool isUsingRootmotion;
        public bool isDamaged;
        public bool isDead;

        [Header("status booleans")]
        public bool isGrounded;
        public bool isAerial;
        public bool isJumping;
        public bool isHit;

        [Header("Reference transforms")]
        public Transform lockOnTransform;

        protected virtual void Awake()
        {
            controller = GetComponent<CharacterController>();

            charAnimationManger = GetComponentInChildren<CharacterAnimationManager>();
            charLocomotionManager = GetComponent<CharacterLocomotionManager>();
            charStatManager = GetComponent<CharacterStatsManager>();
            charAudioManager = GetComponent<CharacterSoundManager>();
            charEffectManager = GetComponent<CharacterEffectManager>();
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {
            
        }

    }
}
