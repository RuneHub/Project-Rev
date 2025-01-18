using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class AIBossAnimationEvents : MonoBehaviour
    {
        AIBossManager manager;

        public GameObject fanWeapon;

        public GameObject CastFinishVFX;

        [SerializeField] private List<SkinnedMeshRenderer> SMR;
        [SerializeField] private MeshRenderer MR;
        [SerializeField] private List<GameObject> smrVFX;    

        private void Awake()
        {
            manager = GetComponentInParent<AIBossManager>();
        }

        #region Visuals & VFX

        #region Handfan
        //animation event function, makes the handfan visible
        public void FanVisible()
        {
            fanWeapon.SetActive(true);
        }

        //animation event function, makes the handfan invisible
        public void FanInvisible()
        {
            fanWeapon.SetActive(false);
        }
        #endregion

        #region Magic

        public void CastLoopingVFX()
        {

            GameObject Cast;
            float timer;
            (Cast, timer) = manager.combatManager.GetMagicCastLooping();

            GameObject vfx = Instantiate(Cast, transform);
            Destroy(vfx.gameObject, timer);
        }

        public void CastFinish()
        {
            GameObject vfx = Instantiate(CastFinishVFX, transform);
            Destroy(vfx.gameObject, .5f);
        }

        public void DashAttackVFX()
        {
            GameObject Cast;
            float timer;
            (Cast, timer) = manager.combatManager.GetDashAttackCast();

            manager.combatManager.BreakLockOn();

            GameObject vfx = Instantiate(Cast, transform);
            Destroy(vfx.gameObject, timer);
        }

        #endregion

        #region Teleport
        //animation event function, calls the coroutine for the teleport.
        public void ActivateTeleport()
        {
            StartCoroutine(manager.bossLocomotion.ExecuteTeleport());
        }

        //loops through list of visuals to turn them OFF.
        public void CharInvisible()
        {
            for (int i = 0; i < SMR.Count; i++)
            {
                SMR[i].enabled = false;
            }
            MR.enabled = false;

            for (int i = 0; i < smrVFX.Count; i++)
            {
                smrVFX[i].SetActive(false);
            }
        }

        //loops through list of visuals to turn them ON.
        public void CharVisible()
        {
            for (int i = 0; i < SMR.Count; i++)
            {
                SMR[i].enabled = true;
            }
            MR.enabled = true;
            for (int i = 0; i < smrVFX.Count; i++)
            {
                smrVFX[i].SetActive(true);
            }
        }

        #endregion

        #endregion

        public void RestoreStagger()
        {
            if (manager.statManager.CurrentHealthPercantage < manager.HpTriggerPercentage &&
                manager.spendHPTrigger == false)
            {
                manager.SwapMode(BossMode.HPTriggerMode);
            }
            else
            {
                manager.SwapMode(BossMode.NormalMode);
            }
        }

    }
}