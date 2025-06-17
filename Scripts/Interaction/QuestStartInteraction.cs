using KS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace KS
{
    public class QuestStartInteraction : Interactible
    {
        //UI
        public PlayerUIHUDManager playerHUD;
        //player
        public GameObject csPlayer;
        //boss
        public AIBossManager boss;
        public AIBossHpTriggerManager hpTriggerManager;
        public GameObject csBoss;
        //Cutscene
        public CutsceneManager cutsceneManager;
        public PlayableAsset QuestStartCutscene;
        //Environmental
        public List<Collider> ArenaInvisibleWalls = new List<Collider>();

        protected override void Awake()
        {
            base.Awake();
        }

        //turn the invisible walls off from the get go.
        protected override void Start()
        {
            base.Start();

            for (int i = 0; i < ArenaInvisibleWalls.Count; i++)
            {
                ArenaInvisibleWalls[i].gameObject.SetActive(false);
            }

        }

        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
        }

        public override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);
            //disable player input exept for start.
            cutsceneManager.TurnOffPlayerInput();
            StartCoroutine(StartCSAfterDark());
        }

        IEnumerator StartCSAfterDark()
        {

            cutsceneManager.BlackScreenFadeOut();
            yield return new WaitForSeconds(1.5f); //fade timer;
            playerHUD.ToggleHUD(false);
            //play cutscene
            cutsceneManager.PlayCutscene(QuestStartCutscene);
        }

    }
}