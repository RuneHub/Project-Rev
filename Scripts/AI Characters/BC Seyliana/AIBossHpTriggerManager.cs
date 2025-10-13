using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace KS
{
    public class AIBossHpTriggerManager : MonoBehaviour
    {
        AIBossManager manager;

        public BMech_HPTrigger hpTrigger;
        public AirshipStatus airshipStatus;

        [SerializeField] private CutsceneManager cutsceneManager;
        [SerializeField] private PlayableAsset StormsEyeCutscene;

        private void Awake()
        {
            manager = GetComponent<AIBossManager>();

        }

        public void TurnOffBoss()
        {
            manager.combatManager.BreakLockOn();
            manager.animationEvents.CharInvisible();
            manager.statManager.InvulnOFF();
        }

        public void StartPhaseTransition()
        {
            //starts buildup animation
            manager.bossAnimations.PlayTargetAnimation("StormEye Buildup", true, layerNum: 2);
        }

        public void StartCutscene()
        {
            cutsceneManager.PlayCutscene(StormsEyeCutscene);
        }

        public void StartHpTrigger()
        {
            StartCoroutine(StartInteraction());
        }

        IEnumerator StartInteraction()
        {
            hpTrigger.PlayMechanic();
            yield return new WaitForSeconds(1);

        }

    }
}