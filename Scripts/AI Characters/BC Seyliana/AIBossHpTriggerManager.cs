using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class AIBossHpTriggerManager : MonoBehaviour
    {
        AIBossManager manager;

        public BMech_HPTrigger hpTrigger;
        public AirshipStatus airshipStatus;

        private void Awake()
        {
            manager = GetComponent<AIBossManager>();

        }

        public void StartHpTrigger()
        {
            //the way this is done is going to change in the future
            StartCoroutine(StartInteraction());
        }

        IEnumerator StartInteraction()
        {
            manager.combatManager.BreakLockOn();
            manager.animationEvents.CharInvisible();
            manager.statManager.InvulnOFF();

            airshipStatus.SwapParts();
            yield return new WaitForSeconds(1);
            hpTrigger.PlayMechanic();
            yield return new WaitForSeconds(1);

        }

    }
}