using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class AIBoss_AerialBlitz : MonoBehaviour
    {
        AIBossManager manager;

        private void Awake()
        {
            manager = GetComponentInParent<AIBossManager>();
        }

        //animation event, sets the booleans to true, to stop idling and continue behaviour
        public void FanCatch()
        {
            Debug.Log("Catch");
            manager.FanCatchFlag = true;
            manager.animator.SetBool("FanCatch", true);
            Destroy(this.gameObject);
        }
    }
}
