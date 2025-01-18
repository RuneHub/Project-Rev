using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class AIManager : CharacterManager
    {
        [SerializeField] private PlayerManager target;

        [Header("Reference scripts")]
        public AICombatManager combatManager;

        //temp, chould have a locomotion script
        public Rigidbody rb;

        [Header("AI Combat floats")]
        public float CombatRange = 35f;
        public float CombatAttackPercentage = 100f;
        public float combatCooldown = 15f;

        protected override void Awake()
        {
            base.Awake();

            target = FindObjectOfType<PlayerManager>();

            combatManager = GetComponent<AICombatManager>();

            rb = GetComponent<Rigidbody>();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        public PlayerManager GetTarget()
        {
            return target;
        }

        public BehaviourContext GetContext()
        {
            return GetComponent<BehaviourTreeRunner>().GetContext();
        }
    }
}