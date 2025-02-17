using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class StartBossFightInteraction : Interactible
    {

        public List<Collider> ArenaInvisibleWalls = new List<Collider>();

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            for (int i = 0; i < ArenaInvisibleWalls.Count; i++) 
            {
                ArenaInvisibleWalls[i].gameObject.SetActive(false);
            }

        }
        public override void Interact(PlayerManager player)
        {
            base.Interact(player); 
            StartBossFight();
        }

        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
        }

        public override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
        }

        private void StartBossFight()
        {
            Debug.Log("Boss Fight Start");

            for (int i = 0; i < ArenaInvisibleWalls.Count; i++)
            {
                ArenaInvisibleWalls[i].gameObject.SetActive(true);
            }

            //start intro cutscene
        }

    }
}