using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace KS
{
    public class PlayerEffectManager : CharacterEffectManager
    {
        private PlayerManager player;

        public Transform FeetTransform;

        [Header("Combat")]
        public BaseEffectSO perfectTiming;
        public BaseEffectSO UniqueChargeLvlUp;
        public GameObject UniqueChargeConstant;

        [Header("Sprinting")]
        public bool SprintingEffect = false;
        public BaseEffectSO sprintVFX;

        [Header("Jumping")]
        public BaseEffectSO jumpVFX;

        #region defaults
        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void DeployEffect(BaseEffectSO _effect)
        {
            base.DeployEffect(_effect);
        }

        //uses the setup function in the base effect script.
        protected override void Setup()
        {
            base.Setup();
        }
        #endregion

        #region Combat
        public void PerfectTimingEffect()
        {
            DeployEffect(perfectTiming);
        }

        //set & deploy effect used for unique skill
        public void UniqueSkillEffect(PlayerCombatManager.ChargeLevel lvl, float shakeAmount, float shakeDuration)
        {
            //switch case for changes per charge lvl
            switch (lvl)
            {
                case PlayerCombatManager.ChargeLevel.lvl1:
                    player.cameraHandler.EffectShake(shakeDuration, shakeAmount);
                    break;
                case PlayerCombatManager.ChargeLevel.lvl2:
                    player.cameraHandler.EffectShake(shakeDuration, shakeAmount);
                    DeployEffect(UniqueChargeLvlUp);
                    break;
                case PlayerCombatManager.ChargeLevel.lvl3:
                    player.cameraHandler.EffectShake(shakeDuration, shakeAmount);
                    DeployEffect(UniqueChargeLvlUp);
                    break;
                case PlayerCombatManager.ChargeLevel.lvl4:
                    player.cameraHandler.EffectShake(shakeDuration, shakeAmount);
                    DeployEffect(UniqueChargeLvlUp);
                    break;
            }
            
            player.combatManager.ExecutedEffect = true;
        }

        public void UniqueSKillEffect(bool Active)
        {
            if (UniqueChargeConstant != null)
            {
                if (Active)
                {
                    UniqueChargeConstant.SetActive(true);
                }
                else 
                {
                    UniqueChargeConstant.SetActive(false);
                }
            }

        }
        #endregion

        #region Sprinting
        //sets up & deploy's all effects related to Dashing
        public void DashEffect()
        {
            SprintingEffect = true;
            DeployEffect(sprintVFX);
        }
        #endregion

        #region Jumping
        //sets up & deploy's all effects related to Jumping
        public void JumpEffect()
        {
            DeployEffect(jumpVFX);
        }
        #endregion

        #region Shake
        //starts the shake corutine
        public void CharacterShake(float duration, float magnitude)
        {
            StartCoroutine(Shake(duration, magnitude));
        }

        //saves the orignal local position, then shakes the character for its duration.
        //then puts it back to the original position.
        IEnumerator Shake(float duration, float magnitude)
        {
            Vector3 OriginalPos = player.playerAnimations.gameObject.transform.localPosition;

            float elapsed = 0f;

            while(elapsed < duration) 
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                player.playerAnimations.gameObject.transform.localPosition = new Vector3(x, y, OriginalPos.z);
                elapsed += Time.deltaTime;

                yield return null;
            }

            player.playerAnimations.gameObject.transform.localPosition = OriginalPos;

        }
        #endregion

    }
}