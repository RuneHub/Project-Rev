using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace KS
{
    
    public class PlayerUltimateAttackManager : MonoBehaviour
    {
        private PlayerManager player;
        private AnimatorOverrideController animatorOV;

        [Header("Build Up")]
        public float MaxUltimateBar = 100;
        public float currentUltimateBar = 0;
        [Range(0,100)] public float UltimateBarPercentage;
        public enum BarSource { Attack, PerfectTimedAttack, SkillAttack,
            JustDodge, UltimateStartUpUse, UltimateUse, Damage };

        [Header("The actual attack")]
        [SerializeField] private CutsceneManager csManager;
        [SerializeField] private PlayableAsset UltimateAttack;
        public bool UltimateAvailable;
        public bool UltConfirmed;
        [SerializeField] private Camera UltimateCam;
        [SerializeField] private float UltWaitTimer = 2f;
        [SerializeField] private UltimateAttackCollider UltimateAttackStarterHitBox;
        [SerializeField] private float UltInitialTime = 2f;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
            animatorOV = player.animatorOV;
        }

        private void Start()
        {
            currentUltimateBar = 0;
        }

        #region Build Up

        public void ChangeBarAmount(BarSource barSource)
        {
            switch (barSource)
            {
                case BarSource.Attack:
                    currentUltimateBar += Random.Range(.3f, .6f);
                    break;
                case BarSource.PerfectTimedAttack:
                    currentUltimateBar += 1;
                    break;
                case BarSource.SkillAttack:
                    currentUltimateBar += Random.Range(.5f, .7f);
                    break;
                case BarSource.JustDodge:
                    currentUltimateBar += 2;
                    break;
                case BarSource.UltimateStartUpUse:
                    currentUltimateBar -= 20;
                    break;
                case BarSource.UltimateUse:
                    currentUltimateBar = 0;
                    break;
                case BarSource.Damage:
                    currentUltimateBar += 1;
                    break;
            }

            if (currentUltimateBar > MaxUltimateBar)
                currentUltimateBar = MaxUltimateBar;

            if (currentUltimateBar == MaxUltimateBar)
                UltimateAvailable = true;
            else
                UltimateAvailable = false;

            if (currentUltimateBar < 0)
                currentUltimateBar = 0;

            UltimateBarPercentage = (currentUltimateBar / MaxUltimateBar) * 100;


        }

        #endregion

        #region Ultimate Attack
        //starts the animation + effect for the Ultimate Attack, also turns on the specific camera overlay.
        //The Ultimate attack will be performed based on hit which the conformation comes from the Hitbox.
        public void PerformUltimate()
        {
            Debug.Log("Perform Ultimate Attack");
            player.playerAnimations.PlayTargetAnimation("Ultimate_StartUp", true, layerNum: 1);
            ScreenManager.instance.DarkenScene();

            //turn on Ultimate Cam.
            UltimateCam.gameObject.SetActive(true);
        }

        //starts the coroutine for the attack, comes from the combat animation event script.
        public void ShootUltHitbox()
        {
            StartCoroutine(UltimateAttackPerformed());
        }

        //Creates the hitbox, vfx, screenshake & sfx.
        //waits for a given time then sends it through the "attack missed" route.
        IEnumerator UltimateAttackPerformed()
        {
            //create the hitbox + vfx + screen shake + sfx
            UltimateAttackCollider hitbox = Instantiate(UltimateAttackStarterHitBox, player.transform);
            hitbox.DestroyWithTime = true;
            hitbox.DestroyTimer = UltWaitTimer;
            hitbox.InsertManager(player);
            hitbox.Init(DestroyHitbox, player, 0);

            yield return new WaitForSeconds(UltWaitTimer);

            //if it doens't then start recovery animation + revert dark scene effect.
            if (!UltConfirmed)
            {
                UltimateAttackMissed();
            }

        }

        //if the hitbox makes contact then stop the coroutine that will go to the "attack missed" route.
        //starts the coroutine for the actual attack.
        public void ApprovedUlt()
        {
            UltConfirmed = true;
            //stops coroutine
            StopCoroutine(UltimateAttackPerformed());
            Debug.Log("start Ultimate");

            ChangeBarAmount(BarSource.UltimateUse);

            //Start coroutine UltimateAttack
            StartCoroutine(StartUltimateAttack());
        }

        //turns off the inputs & the locomotion (for gravity) & starts the cs for the attack.
        IEnumerator StartUltimateAttack()
        {
            //turn player invincible.
            player.combatAnimationEvents.InvulnON();

            //turn inputs off.
            player.inputs.DisableGameplayInput();
            player.playerLocomotion.enabled = false;

            yield return new WaitForSeconds(UltInitialTime);

            //start utlimate cs.
            csManager.PlayCutscene(UltimateAttack);

            //move the player & Boss so they aren't visible in the cs.
            player.animationEvents.HandleInvisible();
            player.playerAnimations.PlayTargetAnimation("Ultimate_Recovery", true, layerNum: 1);

            //make target invisible (specifically for boss)
            csManager.TurnBossInvisible();
        }

        //gets called based on a timer, plays a recovery animation & turns the darkened scene back to normal.
        public void UltimateAttackMissed()
        {
            if (UltConfirmed)
                return;

            Debug.Log("recover Ultimate Attack");

            player.playerAnimations.PlayTargetAnimation("Ultimate_Recovery", true, layerNum: 1);
            ScreenManager.instance.RevertDarkenScene();
            UltimateCam.gameObject.SetActive(false);
        }

        public void HandleUltimateAttackRecovery()
        {
            Debug.Log("Recpvery of Ultimate Attack");

            ScreenManager.instance.RevertDarkenScene();
            UltimateCam.gameObject.SetActive(false);
            player.inputs.EnableGameplayInput();
        }
        
        //the function that destroys the hitbox
        private void DestroyHitbox(BaseDamageCollider obj)
        {
            Destroy(obj.gameObject);
        }
        #endregion
    }
}