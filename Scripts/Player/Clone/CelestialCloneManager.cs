using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEditor.PlayerSettings;

namespace KS
{
    public class CelestialCloneManager : MonoBehaviour
    {
        [SerializeField] private PlayerManager player;
        private Animator animator;
        private CelestialCloneCombatAnimationEvents animationEvents;

        public bool isUsingRootmotion;
        public bool isInteracting;

        [Header("Clone")]
        public bool Active;
        public AttackStandardSO followUpSO;
        public AttackStandardSO finisherSO;
        public GameObject appearVFX;
        public float appearTime;
        public AudioSource audioHolder;

        private float followUp;

        [Header("Stats")]
        public float cloneAttackPower;
        public float cloneBuffAttackPower;
        public Color cloneHUDDisplayColor;

        [Header("Dissolve Effects")]
        public float dissolveRate = 0.0125f;
        public float refreshRate = 0.025f;
        public bool dissolving = false;
        [SerializeField] private SkinnedMeshRenderer[] SMR;
        [SerializeField] private MeshRenderer[] MR;
        private float animTime;
        public float idleTimeBeforeDissolve = 5;
        [SerializeField] float currentIdleTime;

        [Header("Audio")]
        public AudioClip appearSFX;
        public AudioClip disappearSFX;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            animationEvents = GetComponentInChildren<CelestialCloneCombatAnimationEvents>();
        }

        private void Start()
        {
            HandleInvisible();
            currentIdleTime = idleTimeBeforeDissolve;
            appearVFX.SetActive(false);
        }

        public void ActivateClone(Vector3 pos, float followUpNum)
        {
            appearVFX.SetActive(false);
            PositionClone(pos);
            followUp = followUpNum;

            if (!Active)
            {
                StartCoroutine(ActivatedClone());
            }

            HandleAttack();

        }

        public void ActiveCloneFinisher(Vector3 pos)
        {
            PositionClone(pos);

            HandleFinisher();
        }

        public void Update()
        {
            isUsingRootmotion = animator.GetBool("isUsingRootmotion");
            isInteracting = animator.GetBool("isInteracting");

            if (!isInteracting && Active)
            {
                currentIdleTime -= Time.deltaTime;
            }
            else
            {
                currentIdleTime = idleTimeBeforeDissolve;
            }

            if (currentIdleTime < 0)
            {
                currentIdleTime = 0;
                if (!dissolving)
                {
                    HandleDissolve();
                }
            }

        }
        
        #region Positioning & Activation
        private IEnumerator ActivatedClone()
        {
            Active = true;

            //use appearing vfx
            appearVFX.SetActive(true);

            //PLay Appearing SFX
            audioHolder.PlayOneShot(appearSFX);

            //wait x time for vfx
            yield return new WaitForSeconds(appearTime);
            
            //make visibile
            HandleVisibile();
        }

        private void PositionClone(Vector3 pos)
        {
            transform.position = player.transform.position + pos;
            transform.rotation = player.transform.rotation;
        }
        
        #endregion

        #region Animations
        private void HandleAttack()
        {
            string attackName = "";

            if (followUp == 2)
            {
                attackName = "Attack_G1";
                PlayTargetAnimation(attackName, true);
            }
            else if (followUp == 3)
            {
                attackName = "Attack_G3";
                PlayTargetAnimation(attackName, true, isUsingRootmotion = true);
            }
        }

        private void HandleFinisher()
        {
            PlayTargetAnimation("Attack_Finisher", true, UseRootmotion: true, CrossFadeSpeed: 0);
        }

        //function that play's the given animation
        //has parameters that have to be filled and ones that have a default value if left empty.
        private void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool UseRootmotion = false, float CrossFadeSpeed = 0.2f, int layerNum = 0, float normalizedTime = 0f)
        {
            animator.SetBool("isInteracting", isInteracting);
            animator.SetBool("isUsingRootmotion", UseRootmotion);
            animator.CrossFade(targetAnimation, CrossFadeSpeed, layerNum, normalizedTime);
        }
        #endregion

        #region Actual Attack

        public void HandleShooting(Transform output)
        {
            //Debug.Log("Shoot");
            RaycastShot(output); 
        }

        private void RaycastShot(Transform output)
        {
            // 1. create muzzleflash
            if (followUpSO.FX_Muzzleflash != null)
            {
                var ReleaseFX = Instantiate(followUpSO.FX_Muzzleflash);
                Destroy(ReleaseFX, 2);
                ReleaseFX.transform.position = output.transform.position;
                ReleaseFX.transform.rotation = output.transform.rotation;
            }

            // 2. set sfx up
            if (followUpSO.ReleaseSFX != null)
            {
                player.soundManager.PlayWeaponSound(followUpSO.ReleaseSFX);
            }

            // 4. set up hitscan direction/distance
            Vector3 Direction = Vector3.zero;
            float distanceFromTarget = 0;

            //check what playemode is active and change the direction based on that
            if (player.modeManager.currentMode == PlayMode.LockOnMode
                && player.cameraHandler.currentLockOnTarget != null)
            {
                Direction = player.cameraHandler.currentLockOnTarget.transform.position - output.transform.position;
            }
            else if (player.modeManager.currentMode == PlayMode.FreeMode)
            {
                Direction = player.transform.forward;
            }

            // 5. the hitscan
            RaycastHit hit;
            if (Physics.Raycast(output.transform.position, Direction, out hit))
            {
                //get distance from target.
                distanceFromTarget = Vector3.Distance(output.transform.position, hit.point);

                //Create trailrenderer vfx
                var projectile = Instantiate(followUpSO.FX_FakeProjectile);
                projectile.transform.position = output.position;
                projectile.transform.rotation = output.rotation;

                //projectile.transform.position = hit.point;
                StartCoroutine(SpawnBullet(projectile, hit.point, distanceFromTarget));

                //return if it is too far away.
                if (distanceFromTarget >= player.combatManager.MaxHitDistance)
                {
                    return;
                }

                // 6. create the impact vfx
                if (followUpSO.FX_Impact != null) //temp, can remove the if statements later
                {
                    var impactVFX = Instantiate(followUpSO.FX_Impact);
                    Destroy(impactVFX, 2);
                    impactVFX.transform.position = hit.point;
                }

                // 7. add damage to object that got hit
                // if the attack is a perfect timed combo add the perfect timed buff to the attack
                if (hit.collider.tag.Contains("Hurtbox"))
                {
                    //Debug.Log("2 - hit: " + hit.transform.root);
                    if (hit.transform.root.gameObject.tag == "Enemy" &&
                        hit.transform.root.gameObject.GetComponent<CharacterStatsManager>() != null)
                    {
                        float atkDmg = cloneAttackPower / 100 * (cloneBuffAttackPower + 100);
                        var (damage, isCrit) = StatCalculator.CalculateDamage(player, atkDmg, hit.transform.root.GetComponent<CharacterManager>());

                        float hitAngle = Vector3.SignedAngle(transform.forward, hit.transform.forward, Vector3.up);

                        hit.transform.root.gameObject.GetComponent<CharacterStatsManager>().TakeDamage(damage, isCrit, cloneHUDDisplayColor, hitAngle);
                    }
                }


            }


            IEnumerator SpawnBullet(GameObject fakeProjectile, Vector3 hitpoint, float distance)
            {
                var startPos = fakeProjectile.transform.position;
                var remainingDistance = distance;

                while (remainingDistance > 0)
                {
                    fakeProjectile.transform.position = Vector3.Lerp(startPos, hitpoint, 1 - (remainingDistance / distance));
                    remainingDistance -= player.combatManager.FakeBulletSpeed * Time.deltaTime;
                    yield return null;
                }

                fakeProjectile.transform.position = hitpoint;
                yield return new WaitForSeconds(2);
                Destroy(fakeProjectile, 3);
            }
        }

        #endregion

        #region Visibility & Dissolve
        //setting the dissolve to 0 making the clone visible
        public void HandleVisibile()
        {
            for (int x = 0; x < SMR.Length; x++)
            {
                SMR[x].materials[0].SetFloat("_DissolveAmount", 0);
                SMR[x].materials[1].SetFloat("_DissolveAmount", 0);
            }

            for (int x = 0; x < MR.Length; x++)
            {
                MR[x].materials[0].SetFloat("_DissolveAmount", 0);
            }
        }

        //setting the dissolve to 1, to make the instant invsible
        public void HandleInvisible()
        {
            for (int x = 0; x < SMR.Length; x++)
            {
                SMR[x].materials[0].SetFloat("_DissolveAmount", 1);
                SMR[x].materials[1].SetFloat("_DissolveAmount", 1);
            }

            for (int x = 0; x < MR.Length; x++)
            {
                MR[x].materials[0].SetFloat("_DissolveAmount", 1);
            }
        }

        //start the coroutine to for the dissove effect.
        public void HandleDissolve()
        {
            StartCoroutine(StartDissolve());
        }

        //the dissove effect,
        //uses all of the SkinnedMeshRenderers & MeshRenderers components,
        //starts adding to the "DissolveAmount" for the effect to happen.
        //the speed it dissoves depends on the "dissolveRate" variable.
        private IEnumerator StartDissolve()
        {
            dissolving = true;
            Active = false;
            PlayTargetAnimation("Anim_Away", false);

            audioHolder.PlayOneShot(disappearSFX);

            if (SMR.Length > 0)
            {
                float counter = 0;
                while (SMR[0].material.GetFloat("_DissolveAmount") < 1)
                {
                    counter += dissolveRate;
                    for (int x = 0; x < SMR.Length; x++)
                    {
                        SMR[x].materials[0].SetFloat("_DissolveAmount", counter);
                        SMR[x].materials[1].SetFloat("_DissolveAmount", counter);
                    }

                    for (int x = 0; x < MR.Length; x++)
                    {
                        MR[x].materials[0].SetFloat("_DissolveAmount", counter);
                    }

                    yield return new WaitForSeconds(refreshRate);
                    if (Active)
                    {
                        StopCoroutine(StartDissolve());
                        HandleVisibile();
                        break;
                    }

                }
            }

            Deactivate();
        }
        #endregion

        //deactive this clone
        public void Deactivate()
        {
            dissolving = false;
            currentIdleTime = idleTimeBeforeDissolve;
        }

    }
}