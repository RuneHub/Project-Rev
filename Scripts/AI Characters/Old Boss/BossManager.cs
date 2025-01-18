using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class BossManager : CharacterManager
    {

        [Header("booleans")]
        public bool isPlayingAnimation;
        public bool galeFlutterActive;

        [Header("reference scripts")]
        public BehaviourTreeRunner behaviourRunner;

        public BossAnimationManager animationManager;
        public BossStatManager bossStat;
        public BossUIManager uiManager;
        public BossObjectSpawner spawner;
        public RepeatingSpawner repeatingSpawner;
        public BossWaypointHQ waypointHQ;

        public PlayerManager player;

        public AtlosMain Atlos;

        protected override void Awake()
        {
            base.Awake();

            animator = GetComponentInChildren<Animator>();

            behaviourRunner = GetComponent<BehaviourTreeRunner>();

            animationManager = GetComponentInChildren<BossAnimationManager>();
            bossStat = GetComponent<BossStatManager>();

            uiManager = FindObjectOfType<BossUIManager>();

            spawner = GetComponentInChildren<BossObjectSpawner>();
            repeatingSpawner = GetComponent<RepeatingSpawner>();
            waypointHQ = GetComponent<BossWaypointHQ>();

            player = FindObjectOfType<PlayerManager>();

            if (Atlos == null)
            {
                Debug.LogError("Boss Manager does not have Atlos");
            }

        }

        protected override void Start()
        {
            base.Start();


        }

        protected override void Update()
        {
            base.Update();

            animator.SetBool("galeFlutterActive", galeFlutterActive);

            isPlayingAnimation = animator.GetBool("isPlayingAnimation");
            isInteracting = animator.GetBool("isInteracting");
            isUsingRootmotion = animator.GetBool("isUsingRootmotion");

        }

    }
}
