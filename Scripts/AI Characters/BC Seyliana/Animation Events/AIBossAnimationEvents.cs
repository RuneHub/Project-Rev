using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class AIBossAnimationEvents : MonoBehaviour
    {
        AIBossManager manager;

        public GameObject fanWeapon;
        [Space(10)]
        public GameObject LoopingCastVFX;
        public GameObject CastFinishVFX;
        [Space(10)]
        public GameObject PhaseTransitVFX;
        [Space(10)]
        public GameObject StormAura;
        [Space(10)]
        public GameObject buildUpShine;
        public float buildUpShineDestroyTimer = 1f;
        [Space(10)]
        [SerializeField] private SkylightManager skylightManager;
        [Space(10)]
        [SerializeField] private List<SkinnedMeshRenderer> SMR;
        [SerializeField] private MeshRenderer MR;
        [SerializeField] private List<GameObject> smrVFX;    

        private void Awake()
        {
            manager = GetComponentInParent<AIBossManager>();

            LoopingCastOff();
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
        public void LoopingCastOn()
        {
            LoopingCastVFX.gameObject.SetActive(true);
        }

        public void LoopingCastOff()
        {
            LoopingCastVFX.gameObject.SetActive(false);
            
        }

        public void CastFinish()
        {
            LoopingCastOff();
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

        public void CastBuildUp()
        {
            
        }

        public void PowerUnleashedVFX()
        {
            GameObject vfx = Instantiate(PhaseTransitVFX, manager.transform);
            Destroy(vfx, 15f);
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
            DisableStormAura();
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
            ActivateStormAura();
        }

        #endregion

        #endregion

        public void ActivateStormAura()
        {
            StormAura.SetActive(true);
        }

        public void DisableStormAura()
        {
            StormAura.SetActive(false);
        }

        public void StartTransition()
        {
            StormAura.SetActive(false);
            manager.hpTriggerManager.StartCutscene();
        }

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