using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

namespace KS { 
    public class PlayerManager : CharacterManager
    {
        [Header("reference scripts")]
        public PlayerInputManager inputs;
        public PlayerAnimationManager playerAnimations;
        public PlayerLocomotionManager playerLocomotion;
        public PlayerCombatManager combatManager;
        public PlayerStatManager playerStats;
        public PlayerAnimationEvents animationEvents;
        public PlayerCombatAnimationEvents combatAnimationEvents;
        public CameraManager cameraHandler;
        public CameraCrosshairTarget crosshairTarget;
        public ModeManager modeManager;
        public PlayerSoundManager soundManager;
        public PlayerEffectManager effectManager;
        public PlayerInteractionManager interactionManager;
        public PlayerUniqueMechanicManager uniqueMechManager;
        public PlayerUniqueUI uniqueUI;

        public AnimatorOverrideController animatorOV;

        public FXMeshTrail trailEffect; //temp <- this needs to be more modular in the future.

        [Header("player movement booleans")]
        public bool isSprinting;
        public bool isDodging;

        [Header("Player Combat booleans")]
        public bool canDoCombo;
        public bool perfectTimedCombo;
        public bool isAiming;
        public bool isCharging;
        public bool animCancelled;
        public bool dodgeCancellable;
        public bool JustDodge;

        [Header("Player Combat floats")]
        public float CombatRange = 20;

        [Header("Celestial Clone")]
        public CelestialCloneManager Clone;

        protected override void Awake()
        {
            base.Awake();

            animator = GetComponentInChildren<Animator>();

            inputs = GetComponent<PlayerInputManager>();
            playerAnimations = GetComponentInChildren<PlayerAnimationManager>();
            playerLocomotion = GetComponent<PlayerLocomotionManager>();
            combatManager = GetComponent<PlayerCombatManager>();
            playerStats = GetComponent<PlayerStatManager>();
            animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
            combatAnimationEvents = GetComponentInChildren<PlayerCombatAnimationEvents>();

            soundManager = GetComponent<PlayerSoundManager>();
            effectManager = GetComponent<PlayerEffectManager>();

            cameraHandler = CameraManager.singleton;
            crosshairTarget = FindObjectOfType<CameraCrosshairTarget>();

            modeManager = GetComponent<ModeManager>();

            trailEffect = GetComponent<FXMeshTrail>();

            interactionManager = GetComponent<PlayerInteractionManager>();
            uniqueMechManager = GetComponent<PlayerUniqueMechanicManager>();

            uniqueUI = FindObjectOfType<PlayerUniqueUI>(); 
        }

        protected override void Start()
        {
            base.Start();

        }

        protected override void Update()
        {
            base.Update();

            if (cameraHandler != null)
            {
                cameraHandler.HandleCamera();
            }

            //works together with the animator for setting & resetting of animation based booleans.
            //get
            isInteracting = animator.GetBool("isInteracting");
            isJumping = animator.GetBool("isJumping");
            isUsingRootmotion = animator.GetBool("isUsingRootmotion");
            isDodging = animator.GetBool("isDodging");
            canDoCombo = animator.GetBool("canDoCombo");
            isHit = animator.GetBool("isDamaged");
            perfectTimedCombo = animator.GetBool("perfectTimedCombo");
            
            isCancellable = animator.GetBool("isCancellable");
            animCancelled = animator.GetBool("Cancelled");
            dodgeCancellable = animator.GetBool("isDodgeCancellable");

            playerLocomotion.useAdditionalMovement = animator.GetBool("AdMovement");

            //set
            animator.SetBool("FreeMode", modeManager.FreeMovement);
            animator.SetBool("StrafeMode", modeManager.StrafeMovement);
            animator.SetBool("LockedOn", inputs.lockOnFlag);
            animator.SetBool("isAiming", isAiming);
            animator.SetBool("isCharging", isCharging);
            
            inputs.HandleAllInputs(); // checks for inputs.

            playerLocomotion.HandleAllMovements();

            animationEvents.HandleAllAnimCancels();

            combatManager.CombatUpdate();

            uniqueMechManager.UniqueMechUpdate();

            UIManager.instance.hudManager.UpdateHUD();

        }

    }
}