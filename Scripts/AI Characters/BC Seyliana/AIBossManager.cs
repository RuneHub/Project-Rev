using KS.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public enum BossMode { NormalMode, HPTriggerMode, BreakMode }

    public class AIBossManager : CharacterManager
    {
        [SerializeField] private PlayerManager target;

        [Header("Reference Scripts")]
        public BehaviourTreeRunner behaviourRunner;
        public UtilityAI utilityAi;
        public AIBossAnimationManager bossAnimations;
        public AIBossLocomotionManager bossLocomotion;
        public AIBossCombatManager combatManager;
        public AIBossStatManager statManager;
        public AIBossAnimationEvents animationEvents;
        public AIBossCombatAnimationEvents combatAnimationEvents;
        public AIBossSoundManager soundManager;
        public AIBossEffectManager effectManager;
        public AIBossHpTriggerManager hpTriggerManager;
        public AIBossBattleData battleData;

        public AnimatorOverrideController animatorOV;

        [Header("Mode")]
        public BossMode currentMode = BossMode.NormalMode;
        public float breakModeTime = 15f;
        public float HpTriggerPercentage = 40f;
        public bool spendHPTrigger;

        [Header("Flags")]
        public bool comboFlag;
        public bool FanCatchFlag;
        public bool MagicSummonFlag;
        public bool HpTriggerFlag;

        [Header("Combat Booleans")]
        public bool performingAttackAction;
        public bool TeleportingCompleted;
        public bool LongCastFinish;
        public bool DashAttackCompleted;
        public bool ActiveMechanic;

        [Header("Combat positions")]
        public Vector3 CentralPosition; //this is the position where the boss is whilst doing mechanics.
        public Quaternion CentralRotation; //this is the rotation where the boss is facing whilst doing mechanics.
        public float CombatRange = 30f;
        public BoxCollider field;

       

        protected override void Awake()
        {
            base.Awake();

            target = FindObjectOfType<PlayerManager>();

            animator = GetComponentInChildren<Animator>();

            behaviourRunner = GetComponent<BehaviourTreeRunner>();
            utilityAi = GetComponent<UtilityAI>();

            bossAnimations = GetComponentInChildren<AIBossAnimationManager>();
            bossLocomotion = GetComponent<AIBossLocomotionManager>();
            combatManager = GetComponent<AIBossCombatManager>();
            statManager = GetComponent<AIBossStatManager>();
            animationEvents = GetComponentInChildren<AIBossAnimationEvents>();
            combatAnimationEvents = GetComponentInChildren<AIBossCombatAnimationEvents>();
            soundManager = GetComponent<AIBossSoundManager>();
            effectManager = GetComponent<AIBossEffectManager>();
            hpTriggerManager = GetComponent<AIBossHpTriggerManager>();
            battleData = GetComponent<AIBossBattleData>();
        }

        protected override void Start()
        {
            base.Start();

            CentralPosition = transform.position;
            CentralRotation = transform.rotation;


            combatManager.SetupCombat();
        }

        protected override void Update()
        {
            base.Update();

            //works together with the animator for setting animation based booleans.
            //get
            isInteracting = animator.GetBool("isInteracting");
            isHit = animator.GetBool("isDamaged");
            performingAttackAction = animator.GetBool("PerformingAttackAction");
            MagicSummonFlag = animator.GetBool("MagicSummonFlag");

            //set
            TeleportingCompleted = animator.GetBool("TeleportingCompleted");

            //function updates
            bossLocomotion.HandleAllMovement();

        }

        public void SwapMode(BossMode mode)
        {
            Debug.Log("swap Modes");
            switch (mode)
            {
                case BossMode.NormalMode:
                    currentMode = BossMode.NormalMode;
                    break;
                case BossMode.HPTriggerMode:
                    currentMode = BossMode.HPTriggerMode;
                    break;
                case BossMode.BreakMode:
                    currentMode = BossMode.BreakMode;
                    break;
            }

        }

        public void RestoreBreakMode()
        {
            if (currentMode != BossMode.BreakMode)
                return;

            Debug.Log("Break done");

            animator.SetBool("Staggered", false);
            //play restore vfx

            //add armour back to 100
            statManager.AddArmour(100);
            
            SwapMode(BossMode.NormalMode);

        }

        public PlayerManager GetTarget()
        {
            return target;
        }

        public BehaviourContext GetContext()
        {
            return behaviourRunner.GetContext();
        }

    }
}