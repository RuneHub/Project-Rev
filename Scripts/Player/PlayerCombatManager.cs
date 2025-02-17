using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace KS
{
    public class PlayerCombatManager : MonoBehaviour
    {
        private PlayerManager player;
        private AnimatorOverrideController animatorOV;

        [Header("combat")]
        public int comboCounter;
        public int perfectComboCounter;
        public bool perfectCombo;
        public bool perfectFinish;
        public AttackStandardSO currentAttack;
        public float MaxHitDistance;
        public bool SkillsSealed;
        public float targetDistance;

        [Header("Free Combat")]
        public List<AttackStandardSO> baseAttacks;

        public List<AttackStandardSO> groundAttacks;
        public List<AttackStandardSO> aerialAttacks;

        [Space] public AttackStandardSO PerfectFinishAttack;

        [Header("Projectile Pooling")]
        public bool usePool = true;
        public int defaultCapacity = 25;
        public int MaxCapacity = 250;

        private GameObject poolParent;
        private ObjectPool<GameObject> _pool; //needs to be changed to projectile script when Projectile scripts get redone.

        [Header("Raycast shooting")]
        public float FakeBulletSpeed = 500f;

        [Header("Skills")]
        public PlayerSkillsSO SkillNorth;
        public PlayerSkillsSO SkillSouth;
        public PlayerSkillsSO SkillWest;
        public PlayerSkillsSO SkillEast;

        [Space(10)]
        public bool GapClosing;
        public PlayerGapCloserSkillSO currentGPSkill;
        public float curGPTime;

        [Header("Celestial Clone")]
        public float clonePlacementOffset = 1f;

        [Header("Unique Attack")]
        public float maxChargeTime = 9f;
        public float chargeTime;
        public float chargeSpeed;
        public float ChargeEffectShakeAmount = .4f;
        public float ChargeEffectShakeDuration = .3f;
        public enum ChargeLevel { lvl1, lvl2, lvl3, lvl4 };
        public ChargeLevel chargeLevel = ChargeLevel.lvl1;
        public bool ChargeRelease;
        public bool ExecutedEffect;

        [SerializeField] AnimationClip ChargeLevel1Anim;
        [SerializeField] AttackStandardSO chargelevel1Attack;
        [SerializeField] AnimationClip ChargeLevel2Anim;
        [SerializeField] AttackStandardSO chargelevel2Attack;
        [SerializeField] AnimationClip ChargeLevel3Anim;
        [SerializeField] AttackStandardSO chargelevel3Attack;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
            animatorOV = player.animatorOV;

            if (usePool)
            {
                //SetupPooling(currentAttack.Projectile);
                SetupHitscanPooling(currentAttack.FX_FakeProjectile);
            }
        }

        private void Start()
        {
            MaxHitDistance = player.CombatRange;
            baseAttacks = groundAttacks;
        }

        public void CombatUpdate()
        {

            #region Unique Action Related
            if (player.isCharging)
            {
                CheckChargeTime();

                CheckChargeLevel();

                if (ChargeRelease)
                {
                    ChargeRelease = false;
                    //reset animations
                    animatorOV[ChargeLevel1Anim.name] = ChargeLevel1Anim;
                    //reset timer
                    chargeTime = 0;
                }
            }
            else 
            {
                player.effectManager.UniqueSKillEffect(false);
                if (ScreenManager.instance.celestialON)
                {
                    ScreenManager.instance.FullscreenCelestialOFF();
                }
            }
            #endregion

            if (player.modeManager.currentMode == PlayMode.LockOnMode)
            {
                targetDistance = Vector3.Distance(player.cameraHandler.currentLockOnTarget.position, transform.position);
            }

            HandleGapCloserMovement();

        }

        #region Free & LockOn mode

        //Handles the combo attack, this goes from the second attack, checks the combo counter and insert the correct attack SO.
        //also checks if the combo count might exceed the current count and resets it.
        public void HandleAttackCombo()
        {

            if (comboCounter >= baseAttacks.Count && !perfectFinish)
            {
                ResetCombo();
            }
            else if (comboCounter >= baseAttacks.Count && perfectFinish)
            {
                groundAttacks.Add(PerfectFinishAttack);
                player.inputs.comboFlag = true;
            }

            if (player.inputs.comboFlag)
            {
                player.animator.SetBool("canDoCombo", false);

                animatorOV[baseAttacks[0].attackAnim.name] = baseAttacks[comboCounter].attackAnim; 
                player.animator.runtimeAnimatorController = animatorOV;

                currentAttack = baseAttacks[comboCounter];

                if (player.isGrounded)
                {
                    player.playerAnimations.PlayTargetAnimation("G_Attack", true, CrossFadeSpeed: 0, layerNum: 1, normalizedTime: 0);
                }
                else
                {
                    player.playerAnimations.PlayTargetAnimation("A_Attack", true, true, CrossFadeSpeed: 0, layerNum: 1, normalizedTime: 0);
                }

                comboCounter++;
            }
        }

        //Handles the first attack of the combo, it also increases the counter.
        //checks if it is the aerial or ground combo.
        public void HandleBasicAttack()
        {
            if (player.isGrounded)
            {
                baseAttacks = groundAttacks;
                player.playerAnimations.PlayTargetAnimation("G_Attack", true, layerNum: 1);
            }
            else
            {
                player.isAerial = true;
                baseAttacks = aerialAttacks;
                player.playerAnimations.PlayTargetAnimation("A_Attack", true, layerNum: 1);
            }

            currentAttack = baseAttacks[comboCounter];

            comboCounter++;
        }

        //resets the combo counter
        public void ResetCombo()
        {
            comboCounter = 0;
            perfectComboCounter = 0;

            perfectCombo = false;

            if (perfectFinish)
            {
                perfectFinish = false;
                groundAttacks.Remove(PerfectFinishAttack);
            }
        }

        //resets the inserted animation to the first animation.
        public void ResetCombatAnimations()
        {
            animatorOV[baseAttacks[0].attackAnim.name] = baseAttacks[0].attackAnim;
        }
        #endregion

        #region OTS mode
        //Handles the aiming attack, checks if the is left or right and activates the correct animation.
        /*public void HandleAimingAttack()
        {
            if (LeftFire)
            {
                LeftFire = false;
                //play shoot left animaition
                currentAttack = LeftAimedAttack;
            }
            else 
            {
                LeftFire = true;
                //play shoot right animation
                currentAttack = RightAimedAttack;
            }
            player.playerAnimations.PlayTargetAnimation(currentAttack.attackAnim.name, false, layerNum: 1);
        }*/
        #endregion

        #region projectile based shooting
        //Object pooling the player bullets.
        //it grabs or creates a bullet depending on if pooling is enabled.
        //gives it the information it needs and alligns it with the parameter transform.
        /*
        public void shootProjectile(Transform outputTransform)
        {
            if (poolParent == null)
            {
                poolParent = new GameObject();
                poolParent.name = currentAttack.Projectile.name + "_poolParent";
            }

            var projectile = usePool ? _pool.Get() : Instantiate(currentAttack.Projectile);
            projectile.transform.position = outputTransform.position;
            projectile.transform.rotation = outputTransform.rotation;

            projectile.GetComponent<ProjectileCollisionDetection>().impactFX = currentAttack.FX_Impact;
            projectile.GetComponent<ProjectileCollisionDetection>().Init(DestroyProjectile);
            projectile.GetComponent<ProjectileProperties>().attackValue = StatCalculator.CalculateAttackValue(player.playerStats.baseAttack);
            projectile.GetComponent<ProjectileProperties>().CriticalHitRate = player.playerStats.CriticalHitRate;
            projectile.GetComponent<ProjectileProperties>().CriticalHitBuff = player.playerStats.CriticalHitBuff;
            projectile.transform.parent = poolParent.transform;

            if (player.modeManager.currentMode == PlayMode.FreeMode)
            {
                projectile.GetComponent<Rigidbody>().velocity = outputTransform.forward.normalized * currentAttack.projectileSpeed;
            }
            else if (player.modeManager.currentMode == PlayMode.LockOnMode)
            {
                Vector3 targetDir = (player.cameraHandler.currentLockOnTarget.transform.position - projectile.transform.position).normalized * currentAttack.projectileSpeed;
                projectile.GetComponent<Rigidbody>().velocity = targetDir;    
            }
            currentAttack = null;
        }
        */

        //returns the pool of projectiles
        /*
        public ProjectileCollisionDetection GetPool()
        {
            return _pool.Get();
        }
        */

        ////sets the given projectile as a child of the pooling.

        /*
        public void SetPoolParent(ProjectileCollisionDetection child)
        {
            if (poolParent == null)
            {
                poolParent = new GameObject();
                poolParent.name = child.name + "_poolParent";
            }

            child.transform.parent = poolParent.transform;
        }
        */

        //funtion to setup the pooling, gets called in start.
        /*  public void SetupPooling(ProjectileCollisionDetection proj)
          {
              _pool = new ObjectPool<ProjectileCollisionDetection>(
              () =>
              {
                  return Instantiate(proj);
              },
              prefab =>
              {
                  prefab.gameObject.SetActive(true);
              },
              prefab =>
              {
                  prefab.gameObject.SetActive(false);
              },
              prefab =>
              {
                  Destroy(prefab.gameObject);
              },
              false,
              defaultCapacity,
              MaxCapacity
              );
          }
          */

        //a function that is used to either turn a bullet of or destroy it if its an excessive one for the pool.
        /*
        public void DestroyProjectile(ProjectileCollisionDetection proj)
        {
            if (usePool)
            {
                _pool.Release(proj);
            }
            else
            {
                Destroy(proj.gameObject);
            }
        }
        */
        #endregion

        #region Raycast based shooting
        //The function to shoot raycast based projectiles.
        //it also manages the VFX effects.
        //changes depending on modes, lockon mode is kind of guranteed to hit.
        public void ShootRaycastHitscan(Transform outputTransform, AttackStandardSO attackSO)
        {
            //set up pool parent.
            if (poolParent == null)
            {
                poolParent = new GameObject();
                poolParent.name = attackSO.FX_FakeProjectile.name + "_poolParent";
            }

            // 1. create muzzleflash
            if (attackSO.FX_Muzzleflash != null)
            {
                var ReleaseFX = Instantiate(attackSO.FX_Muzzleflash);
                Destroy(ReleaseFX, 2);
                ReleaseFX.transform.position = outputTransform.transform.position;
                ReleaseFX.transform.rotation = outputTransform.transform.rotation;
            }

            // 2. set sfx up
            if (attackSO.ReleaseSFX != null)
            {
                player.soundManager.PlayActionSound(attackSO.ReleaseSFX);
            }

            // 3. set screen shake
            if (attackSO.useScreenShake)
            {
                player.cameraHandler.EffectShake(attackSO.shakeDuration, attackSO.shakeMagnitude);
            }

            // 4. set up hitscan direction/distance
            Vector3 Direction = Vector3.zero;
            float distanceFromTarget = 0;

            //check what playemode is active and change the direction based on that
            if (player.modeManager.currentMode == PlayMode.LockOnMode
                && player.cameraHandler.currentLockOnTarget != null)
            {
                Direction = player.cameraHandler.currentLockOnTarget.transform.position - outputTransform.transform.position;
            }
            else if (player.modeManager.currentMode == PlayMode.FreeMode)
            {
                Direction = player.transform.forward;
            }

            // 5. the hitscan
            RaycastHit hit;
            if (Physics.Raycast(outputTransform.transform.position, Direction, out hit))
            {
                //get distance from target.
                distanceFromTarget = Vector3.Distance(outputTransform.transform.position, hit.point);

                //Create trailrenderer vfx
                var projectile = usePool ? _pool.Get() : Instantiate(attackSO.FX_FakeProjectile);
                projectile.transform.position = outputTransform.position;
                projectile.transform.rotation = outputTransform.rotation;
                projectile.transform.parent = poolParent.transform;
               
                //projectile.transform.position = hit.point;
                StartCoroutine(SpawnBullet(projectile, hit.point, distanceFromTarget));

                //return if it is too far away.
                if (distanceFromTarget >= MaxHitDistance)
                {
                    return;
                }

                // 6. create the impact vfx
                if (attackSO.FX_Impact != null) //temp, can remove the if statements later
                {
                    var impactVFX = Instantiate(attackSO.FX_Impact);
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
                        float atkDmg = attackSO.rawDamage;
                        var (damage, isCrit) = StatCalculator.CalculateDamage(player, atkDmg, hit.transform.root.GetComponent<CharacterManager>());
                        if (perfectCombo)
                        {
                            //Debug.Log("perfect combo atk");
                            damage = Mathf.Round(((damage / 100) * player.playerStats.PerfectTimingBuff) + damage);
                        }

                        float hitAngle = Vector3.SignedAngle(transform.forward, hit.transform.forward, Vector3.up);

                        hit.transform.root.gameObject.GetComponent<CharacterStatsManager>().TakeDamage(damage, isCrit, player.playerStats.HUDDisplayColor, hitAngle);

                        if (perfectCombo)
                        {
                            player.uniqueMechManager.IncreaseLoadedGauge();
                        }

                        //temp
                        //if(!isCrit)
                        //    Debug.Log("hit: " + hit.transform.root.gameObject.name + " for " + damage + " damage!");
                        //else
                        //    Debug.Log("Crit hit: " + hit.transform.root.gameObject.name + " for " + damage + " damage!");
                        //temp end
                    }
                }

                //temp, the line when hitting something
                Debug.DrawLine(outputTransform.position, hit.point, Color.blue, 3);

            }

            IEnumerator SpawnBullet(GameObject fakeProjectile, Vector3 hitpoint, float distance)
            {
                var startPos = fakeProjectile.transform.position;
                var remainingDistance = distance;

                while (remainingDistance > 0)
                {
                    fakeProjectile.transform.position = Vector3.Lerp(startPos, hitpoint, 1 - (remainingDistance / distance));
                    remainingDistance -= FakeBulletSpeed * Time.deltaTime;
                    yield return null;
                }

                fakeProjectile.transform.position = hitpoint;
                yield return new WaitForSeconds(2);
                _pool.Release(fakeProjectile);
            }

        }

        #endregion

        #region Character Skills
        public void HandleSkillAction(char skillDir)
        {
            if (!player.isCancellable)
            {
                if (player.isInteracting)
                    return;

                if (!player.isGrounded)
                    return;

                if (SkillsSealed)
                    return;
            }
            else
            {
                player.animator.SetBool("Cancelled", true);
                player.animationEvents.HandleCancelAnim();
            }

            //Debug.Log(skillDir);
            if (skillDir == 'N' && SkillNorth != null)
            {
                if (CooldownHandler.instance.isOnCooldown(SkillNorth.skillID)) { return; }

                //Debug.Log("skill North");
                SkillNorth.HandleSkill(player, "Skill_North");


                CooldownHandler.instance.PutOnCooldown(SkillNorth);

            }
            else if (skillDir == 'S' && SkillSouth != null)
            {
                if (CooldownHandler.instance.isOnCooldown(SkillSouth.skillID)) { return; }

                //Debug.Log("skill South");
                SkillSouth.HandleSkill(player, "Skill_South");

                CooldownHandler.instance.PutOnCooldown(SkillSouth);
            }
            else if (skillDir == 'W' && SkillWest != null)
            {
                if (CooldownHandler.instance.isOnCooldown(SkillWest.skillID)) { return; }

                //Debug.Log("skill West");
                SkillWest.HandleSkill(player, "Skill_West");

                CooldownHandler.instance.PutOnCooldown(SkillWest);
            }
            else if (skillDir == 'E' && SkillEast != null)
            {
                if (CooldownHandler.instance.isOnCooldown(SkillEast.skillID)) { return; }

               // Debug.Log("skill East");
                SkillEast.HandleSkill(player, "Skill_East");

                CooldownHandler.instance.PutOnCooldown(SkillEast);
            }

        }

        private void HandleGapCloserMovement()
        {
            if (player.playerLocomotion.useAdditionalMovement && GapClosing)
            {
                if (player.modeManager.currentMode == PlayMode.LockOnMode)
                {
                    //curGPDistance = Vector3.Distance(player.lockOnTransform.position, player.transform.position);
                    //Debug.Log(curGPDistance);

                    Vector3 rotDir = player.cameraHandler.currentLockOnTarget.position - transform.position;
                    rotDir.y = 0;
                    rotDir.Normalize();

                    Quaternion tr = Quaternion.LookRotation(rotDir);
                    Quaternion targetRot = Quaternion.Slerp(transform.rotation, tr, 15f * Time.deltaTime);

                    transform.rotation = targetRot;

                }
                
                player.playerLocomotion.adMoveDirection = player.transform.forward;
                player.playerLocomotion.adMoveDirection.y = 0;

                curGPTime -= Time.deltaTime;

                if (curGPTime < 0 ||
                    (player.modeManager.currentMode == PlayMode.LockOnMode 
                    && targetDistance < currentGPSkill.stopDistance))
                {
                    currentGPSkill.GapClosed();
                }
            }
        }

        #endregion

        #region Character Unique Action
        public void HandleUniqueAttack()
        {
            if (!player.isCancellable)
            {
                if (player.isInteracting)
                    return;

                if (player.isJumping || player.isAerial)
                    return;

            }
            else
            {
                player.animator.SetBool("Cancelled", true);
                player.animationEvents.HandleCancelAnim();
            }

            if (player.isCancellable)
            {
                player.cameraHandler.StopEffectShake();
                player.animationEvents.HandleAllAnimCancels();
            }

            if (!player.isCharging)
            {
                player.playerAnimations.PlayTargetAnimation("U_Skill", true, UseRootmotion: true, layerNum: 1);
                player.isCharging = true;

                //turn on constant effect
                player.effectManager.UniqueSKillEffect(true);
            }
        
        }

        private void CheckChargeTime()
        {
            chargeTime += Time.deltaTime * chargeSpeed;

            if (chargeTime < (maxChargeTime / 3))
            {
                chargeLevel = ChargeLevel.lvl1;
            }
            else if (chargeTime > (maxChargeTime / 3)
                && chargeTime < (maxChargeTime / 3) * 2)
            {
                if (chargeLevel == ChargeLevel.lvl1)
                {
                    ExecutedEffect = false;
                }

                chargeLevel = ChargeLevel.lvl2;
            }
            else if ((chargeTime > (maxChargeTime / 3) * 2)
                && chargeTime < maxChargeTime)
            {
                if (chargeLevel == ChargeLevel.lvl2)
                {
                    ExecutedEffect = false;
                }
                chargeLevel = ChargeLevel.lvl3;
            }
            else if (chargeTime >= maxChargeTime)
            {
                if (chargeLevel == ChargeLevel.lvl3)
                {
                    ExecutedEffect = false;
                }
                chargeLevel = ChargeLevel.lvl4;
            }
        }

        private void CheckChargeLevel()
        {
            float CelestialCharge = 0.6f;
            if (chargeLevel == ChargeLevel.lvl1)
            {
                animatorOV[ChargeLevel1Anim.name] = ChargeLevel1Anim;
                player.animator.runtimeAnimatorController = animatorOV;
                CelestialCharge = 0.6f;
                currentAttack = chargelevel1Attack;
            }
            else if (chargeLevel == ChargeLevel.lvl2)
            {
                animatorOV[ChargeLevel1Anim.name] = ChargeLevel2Anim;
                player.animator.runtimeAnimatorController = animatorOV;
                CelestialCharge = 0.48f;
                currentAttack = chargelevel2Attack;
            }
            else if (chargeLevel == ChargeLevel.lvl3)
            {
                animatorOV[ChargeLevel1Anim.name] = ChargeLevel3Anim;
                player.animator.runtimeAnimatorController = animatorOV;
                CelestialCharge = 0.37f;
                currentAttack = chargelevel3Attack;
            }
            else if (chargeLevel == ChargeLevel.lvl4)
            {
                animatorOV[ChargeLevel1Anim.name] = ChargeLevel3Anim;
                player.animator.runtimeAnimatorController = animatorOV;
                chargeTime = maxChargeTime;
                CelestialCharge = 0.25f;
                currentAttack = PerfectFinishAttack;
            }

            if (!ExecutedEffect)
            {
                player.effectManager.UniqueSkillEffect(chargeLevel, ChargeEffectShakeAmount, ChargeEffectShakeDuration);
                ScreenManager.instance.FullscreenCelestial(CelestialCharge, ChargeEffectShakeDuration);
            }

        }
        #endregion

        #region Celestial Clone
        public void CelestialCloneAddition()
        {
            Vector3 addition = new Vector3();
            if (player.cameraHandler.GetCameraCross(player.transform))
            {
                //right
                addition = new Vector3(clonePlacementOffset, 0, -(clonePlacementOffset / 2));
            }
            else
            {
                //left
                addition = new Vector3(-clonePlacementOffset, 0, -(clonePlacementOffset / 2));
            }
            //Debug.Log("Combo counter: " + comboCounter);
            player.Clone.ActivateClone(addition, comboCounter);
            perfectCombo = false;
        }

        public void CelestialClonePerfectFinish()
        {
            Vector3 addition = new Vector3(clonePlacementOffset, 0, 0);
            //Debug.Log("Clone Finsiher");
            player.Clone.ActiveCloneFinisher(addition);
            perfectCombo = false;
        }

            #endregion

        #region Pooling

        public void SetupHitscanPooling(GameObject proj)
        {
            _pool = new ObjectPool<GameObject>(
            () =>
            {
                return Instantiate(proj);
            },
            prefab =>
            {
                prefab.gameObject.SetActive(true);
            },
            prefab =>
            {
                prefab.gameObject.SetActive(false);
            },
            prefab =>
            {
                Destroy(prefab.gameObject);
            },
            false,
            defaultCapacity,
            MaxCapacity
            );
        }

        #endregion

    }
}