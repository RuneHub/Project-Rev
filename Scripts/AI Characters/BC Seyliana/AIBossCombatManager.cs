using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace KS
{
    public class AIBossCombatManager : MonoBehaviour
    {
        private AIBossManager manager;
        private AnimatorOverrideController animatorOV;

        [Header("Melee")]
        public BossMeleeSO MeleeAttack1;
        public BossMeleeSO currentMeleeAttack;

        [Header("Fan")]
        public BossFanSO FanAttack;
        public BossFanSO currentFanAttack;

        [Header("Summon")]
        public BossSummonSO summonAttack;
        public BossSummonSO currentSummonAttack;

        [Header("Magic Casts")]
        public BossMagicCastsMechanicSO magicAttack;
        public BossMagicCastsMechanicSO currentMagicAttack;
        public GameObject magicCastLoopinVFX;

        [Header("Dash Attack")]
        public BossDashAttackSO DashAttackSO;
        public float dashAttackCastTime = 1.5f;

        [Header("Mechanics")]
        public List<BMech_Base> Mechanics;

        private void Awake()
        {
            manager = GetComponent<AIBossManager>();
            animatorOV = manager.animatorOV;
        }

        //setup everything needed for combat, e.g. hitboxes.
        public void SetupCombat()
        {
            manager.combatAnimationEvents.SetupMeleeHitboxes();
            currentMeleeAttack = MeleeAttack1;
        }

        //resets the animation to the original, sets it to the first attack in the list
        public void ResetCombatAnimations()
        {
            animatorOV[MeleeAttack1.attackAnim.name] = MeleeAttack1.attackAnim;
            animatorOV[FanAttack.attackAnim.name] = FanAttack.attackAnim;
            animatorOV[summonAttack.attackAnim.name] = summonAttack.attackAnim;
        }

        //checks if the player is lockon, if they are break it
        public void BreakLockOn()
        {
            if (manager.GetTarget() != null)
            {
                if (manager.GetTarget().cameraHandler.LockedOn)
                {
                    manager.GetTarget().inputs.lockOnInput = true;
                }
            }
        }

        #region Melee
        // Inserts the given Melee animation and plays it.
        public void HandleMeleeAttack(BossMeleeSO so)
        {
            animatorOV[MeleeAttack1.attackAnim.name] = so.attackAnim;
            manager.animator.runtimeAnimatorController = animatorOV;
            currentMeleeAttack = so;

            manager.bossAnimations.PlayTargetAnimation("Melee", true, true, CrossFadeSpeed: 0, layerNum: 2, normalizedTime: 0);

        }

        public GameObject GetMeleeBuildupVFX()
        {
            return currentMeleeAttack.meleeBuildUpVFX;
        }

        #endregion

        #region Fan

        //insert the given Fan Throw Animation, play it
        public void HandleFanAttack(BossFanSO so)
        {
            animatorOV[FanAttack.attackAnim.name] = so.attackAnim;
            manager.animator.runtimeAnimatorController = animatorOV;
            currentFanAttack = so;

            manager.bossAnimations.PlayTargetAnimation("FanAttack", true, true, CrossFadeSpeed: 0, layerNum: 2, normalizedTime: 0);
        }

        //creates the projectile, and initializes the hitbox on it
        public void HandleThrowFan()
        {
            GameObject fanProj = Instantiate(currentFanAttack.AerialBlitz, manager.transform);

            fanProj.GetComponentInChildren<MeleeDamageCollider>().
                Init(manager.combatAnimationEvents.DestroyHitbox, manager, manager.statManager.baseAttack);
        }

        #endregion

        #region Magic: Summon
        //uses the given SO to start an animation attack
        public void HandleMagicSummon(BossSummonSO so)
        {
            animatorOV[summonAttack.attackAnim.name] = so.attackAnim;
            manager.animator.runtimeAnimatorController = animatorOV;
            currentSummonAttack = so;

            manager.bossAnimations.PlayTargetAnimation("SummonAttack", true, true, CrossFadeSpeed: 0, layerNum: 2, normalizedTime: 0);

        }

        public void ActivateMagicSummon(List<GameObject> spawners)
        {
            StartCoroutine(ActivationMagicSummon(spawners, currentSummonAttack.ProjectileDelay));
        }

        //swaps out the build up vfx for the actual projectile
        IEnumerator ActivationMagicSummon(List<GameObject> spawners, float delay)
        {
            for (int i = 0; i < spawners.Count; i++)
            {
                if (spawners[i].transform.childCount == 1)
                {
                    Transform child = spawners[i].transform.GetChild(0);
                    Destroy(child.gameObject);

                    Vector3 tr = (manager.GetTarget().lockOnTransform.position - spawners[i].transform.position).normalized;
                    GameObject proj = Instantiate(currentSummonAttack.projectile, spawners[i].transform.position, Quaternion.LookRotation(tr), null);
                    FireMagicSummon(proj);
                    yield return new WaitForSeconds(delay);
                }
            }
        }

        //shoot the summoned magic projectile towards target
        private void FireMagicSummon(GameObject projectile)
        {
            projectile.GetComponent<BaseDamageCollider>().Init(manager.combatAnimationEvents.DestroyHitbox, manager, manager.statManager.baseAttack);
            projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * currentSummonAttack.ProjectileSpeed;
        }

        #endregion

        #region Magic: Casts
        //set given magic cast to the current one
        public void HandleMagicCast(BossMagicCastsMechanicSO so)
        {
            magicCastLoopinVFX = so.CastingVFX;
            currentMagicAttack = so;
        }

        //returns the casting vfx with the time
        public (GameObject, float) GetMagicCastLooping()
        {
            return (magicCastLoopinVFX, currentMagicAttack.CastTime);
        }

        #endregion

        #region Dash Attack

        //returns ths casting vfx with the time
        public (GameObject, float) GetDashAttackCast()
        {
            return (DashAttackSO.CastingVFX, dashAttackCastTime);
        }

        //handles the aftermath of the dash attack, calls fasttravel in locomotion,
        // play's a finishing animation and turns the compete bool to true.
        public void HandleDashAttackAftermath(Vector3 position)
        {
            //play animation
            manager.bossLocomotion.FastTravel(position, true, true);
            manager.bossAnimations.PlayTargetAnimation("Gale Flutter", true, true, 0, layerNum: 2);
            manager.DashAttackCompleted = true;
        }

        #endregion 

    }
}