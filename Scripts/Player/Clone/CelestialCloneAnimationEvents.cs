using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class CelestialCloneAnimationEvents : MonoBehaviour
    {
        CelestialClone cc;

        private void Awake()
        {
            cc = GetComponentInParent<CelestialClone>();            
        }

        public void PerformAdditionAttack()
        {
            //cc.player.combatManager.ShootRaycastHitscan(cc.Output);
            cc.HandleAttack();
        }

        public void HandleDissove()
        {
            cc.HandleTurningTransparenting();
        }

    }
}