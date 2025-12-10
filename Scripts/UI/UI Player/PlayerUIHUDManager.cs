using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace KS
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        [SerializeField]private PlayerManager playerManager;

        [SerializeField] CanvasGroup[] canvasGroup;

        [Header("Player Vitality")]
        [SerializeField] GameObject playerVitality;

        [SerializeField] private Slider playerHealthSlider;
        [SerializeField] private Slider playerEaseHealthSlider;
        [SerializeField] private float easeHealthLerpSpeed = 0.05f;


        [Header("Ability Slots")]
        [SerializeField] UISkillSlot skillSlotIcon_North;
        [SerializeField] UISkillSlot skillSlotIcon_South;
        [SerializeField] UISkillSlot skillSlotIcon_West;
        [SerializeField] UISkillSlot skillSlotIcon_East;
        public Sprite skillSlotIcon_Empty;
        public Sprite skillSlotIcon_NoIcon;

        [Header("Health Items")]
        [SerializeField] private UIHealingSlot healItemUI_Small;
        [SerializeField] private UIHealingSlot healItemUI_large;

        [Header("Status Effects")]
        [SerializeField] private List<StatusEffectUI_Icon> statusEffects;
        [SerializeField] GridLayoutGroup iconContainer;
        public StatusEffectUI_Icon icon;

        [Header("Animating HUD")]
        public bool isAnimated = false;
        public bool abilitiesOpen = false;
        [SerializeField] private RectTransform hudAbilities_S;
        [SerializeField] private RectTransform hudAbilities_L;
        [SerializeField] private RectTransform hudAbilities_anim;
        [SerializeField] private float animateDuration = .5f;

        [Header("HUD prompts")]
        [SerializeField] private List<UIInputPrompt> hudPrompts = new List<UIInputPrompt>();

        private void Start()
        {

            playerHealthSlider = playerVitality.transform.Find("HealthBar").gameObject.GetComponent<Slider>();

            if (playerHealthSlider == null)
            {
                Debug.LogError("player health slider not found!");
            }
        }

        //Update function for the hud.
        public void UpdateHUD()
        {
            CheckPlayMode();

            CheckPlayerVitality();

            CheckSkillCoodown();

            UpdateHealingItems();

            UpdateStatusEffects();

            UpdateHUDAbilities();

        }

        //toggle to show the hud, true = visible & false = invisible.
        //works with a UI Canvas group alpa variable.
        public void ToggleHUD(bool status)
        {
            if (status)
            {
                foreach (var canvas in canvasGroup)
                {
                    canvas.alpha = 1;
                }
            }
            else
            {
                foreach (var canvas in canvasGroup)
                {
                    canvas.alpha = 0;
                }
            }
        }

        #region player vitality
        //checks the playmode to see if there needs to be UI changes.
        private void CheckPlayMode()
        {
            if (UIManager.instance.player.modeManager.currentMode == PlayMode.FreeMode)
            {
                UIManager.instance.floatingManager.DisableSetLockOnVisual();
            }
            else if (UIManager.instance.player.modeManager.currentMode == PlayMode.LockOnMode)
            {
                UIManager.instance.floatingManager.EnableLockOnVisual(UIManager.instance.player.cameraHandler.currentLockOnTarget);
            }

        }

        //set up player health bars values
        public void SetupPlayerVitality()
        {
            playerHealthSlider.maxValue = UIManager.instance.player.playerStats.maxHealth;
            playerHealthSlider.value = UIManager.instance.player.playerStats.currentHealth;

            playerEaseHealthSlider.maxValue = UIManager.instance.player.playerStats.maxHealth;
            playerEaseHealthSlider.value = UIManager.instance.player.playerStats.currentHealth;
        }

        //Checks the player health in the stat manager and modifies the UI.
        //sets the health bar immediatly to the value, whilst easing in the easeHealthbar.
        private void CheckPlayerVitality()
        {
            playerHealthSlider.value = UIManager.instance.player.playerStats.currentHealth;
            playerEaseHealthSlider.value = Mathf.Lerp(playerEaseHealthSlider.value, UIManager.instance.player.playerStats.currentHealth, easeHealthLerpSpeed);
        }
        #endregion

        #region Skill slot

        #region Set SKill

        //sets the Icon of the skills depending on which place they are in.
        //e.g. the correct icon in the northon slot.
        // also checks if the skills SO have proper Icons.
        public void SetSkillSlotIcon()
        {
            SetSkillSlotIconN();
            SetSkillSlotIconS();
            SetSkillSlotIconW();
            SetSkillSlotIconE();
        }

        public void SetSkillSlotIconN()
        {
            //when there is a skill sloted but not a icon on the SO.
            if (playerManager.combatManager.SkillNorth != null &&
                playerManager.combatManager.SkillNorth.SkillIconHUD == null)
            {
                skillSlotIcon_North.AbilityIcon.sprite = skillSlotIcon_NoIcon;
            }
            //set the SO Icon on the HUD
            else if (playerManager.combatManager.SkillNorth != null)
            {
                skillSlotIcon_North.AbilityIcon.sprite = playerManager.combatManager.SkillNorth.SkillIconHUD;
            }
            //if there is no Skill sloted, set the empty icon on the HUD.
            else
            {
                skillSlotIcon_North.AbilityIcon.sprite = skillSlotIcon_Empty;
            }
        }

        public void SetSkillSlotIconS()
        {
            //when there is a skill sloted but not a icon on the SO.
            if (playerManager.combatManager.SkillSouth != null &&
               playerManager.combatManager.SkillSouth.SkillIconHUD == null)
            {
                skillSlotIcon_South.AbilityIcon.sprite = skillSlotIcon_NoIcon;
            }
            //set the SO Icon on the HUD
            else if (playerManager.combatManager.SkillSouth != null)
            {
                skillSlotIcon_South.AbilityIcon.sprite = playerManager.combatManager.SkillSouth.SkillIconHUD;
            }
            //if there is no Skill sloted, set the empty icon on the HUD.
            else
            {
                skillSlotIcon_South.AbilityIcon.sprite = skillSlotIcon_Empty;
            }
        }

        public void SetSkillSlotIconW()
        {
            //when there is a skill sloted but not a icon on the SO.
            if (playerManager.combatManager.SkillWest != null &&
                playerManager.combatManager.SkillWest.SkillIconHUD == null)
            {
                skillSlotIcon_West.AbilityIcon.sprite = skillSlotIcon_NoIcon;
            }
            //set the SO Icon on the HUD
            else if (playerManager.combatManager.SkillWest != null)
            {
                skillSlotIcon_West.AbilityIcon.sprite = playerManager.combatManager.SkillWest.SkillIconHUD;
            }
            //if there is no Skill sloted, set the empty icon on the HUD.
            else
            {
                skillSlotIcon_West.AbilityIcon.sprite = skillSlotIcon_Empty;
            }
        }

        public void SetSkillSlotIconE()
        {
            //when there is a skill sloted but not a icon on the SO.
            if (playerManager.combatManager.SkillEast != null &&
               playerManager.combatManager.SkillEast.SkillIconHUD == null)
            {
                skillSlotIcon_East.AbilityIcon.sprite = skillSlotIcon_NoIcon;
            }
            //set the SO Icon on the HUD
            else if (playerManager.combatManager.SkillEast != null)
            {
                skillSlotIcon_East.AbilityIcon.sprite = playerManager.combatManager.SkillEast.SkillIconHUD;
            }
            //if there is no Skill sloted, set the empty icon on the HUD.
            else
            {
                skillSlotIcon_East.AbilityIcon.sprite = skillSlotIcon_Empty;
            }
        }
        #endregion

        //checks every frame, checks if the skill is on cooldown.
        // if it is then update the grey fillamount of the ui for visual feedback
        private void CheckSkillCoodown()
        {

            if (skillSlotIcon_North != null && playerManager.combatManager.SkillNorth != null)
            {
                if (CooldownHandler.instance.isOnCooldown(playerManager.combatManager.SkillNorth.skillID))
                {
                    skillSlotIcon_North.CDMask.fillAmount = CooldownHandler.instance.GetCooldownTimer(playerManager.combatManager.SkillNorth.skillID) / playerManager.combatManager.SkillNorth.cooldown;
                }
                else
                {
                    skillSlotIcon_North.CDMask.fillAmount = 0;
                }

            }

            if (skillSlotIcon_South != null && playerManager.combatManager.SkillSouth != null)
            {
                if (CooldownHandler.instance.isOnCooldown(playerManager.combatManager.SkillSouth.skillID))
                {
                    skillSlotIcon_South.CDMask.fillAmount = CooldownHandler.instance.GetCooldownTimer(playerManager.combatManager.SkillSouth.skillID) / playerManager.combatManager.SkillSouth.cooldown;
                }
                else
                {
                    skillSlotIcon_South.CDMask.fillAmount = 0;
                }

            }

            if (skillSlotIcon_West != null && playerManager.combatManager.SkillWest != null)
            {
                if (CooldownHandler.instance.isOnCooldown(playerManager.combatManager.SkillWest.skillID))
                {
                    skillSlotIcon_West.CDMask.fillAmount = CooldownHandler.instance.GetCooldownTimer(playerManager.combatManager.SkillWest.skillID) / playerManager.combatManager.SkillWest.cooldown;
                }
                else
                {
                    skillSlotIcon_West.CDMask.fillAmount = 0;
                }

            }

            if (skillSlotIcon_East != null && playerManager.combatManager.SkillEast != null)
            {
                if (CooldownHandler.instance.isOnCooldown(playerManager.combatManager.SkillEast.skillID))
                {
                    skillSlotIcon_East.CDMask.fillAmount = CooldownHandler.instance.GetCooldownTimer(playerManager.combatManager.SkillEast.skillID) / playerManager.combatManager.SkillEast.cooldown;
                }
                else
                {
                    skillSlotIcon_East.CDMask.fillAmount = 0;
                }

            }

        }

        #endregion

        #region Healing items

        public void SetUpHealingItems()
        {
            if (healItemUI_Small != null)
            {
                healItemUI_Small.InputMask.fillAmount = 0;
                healItemUI_Small.amountText.text = UIManager.instance.player.playerStats.smallhealingAmount.ToString();
            }

            if (healItemUI_large != null)
            {
                healItemUI_large.InputMask.fillAmount = 0;
                healItemUI_large.amountText.text = UIManager.instance.player.playerStats.LargeHealingAmount.ToString();
            }

        }

        private void UpdateHealingItems()
        {
                healItemUI_large.InputMask.fillAmount = UIManager.instance.player.playerStats.largeHealCharge / UIManager.instance.player.playerStats.maxHealingCharge;
                healItemUI_large.amountText.text = UIManager.instance.player.playerStats.LargeHealingAmount.ToString();

                healItemUI_Small.InputMask.fillAmount = UIManager.instance.player.playerStats.smallHealCharge / UIManager.instance.player.playerStats.maxHealingCharge;
                healItemUI_Small.amountText.text = UIManager.instance.player.playerStats.smallhealingAmount.ToString();
        }

        #endregion

        #region Status Effects
        //Receive a new status effect to create an icon for, instantiate it and put it in the list for icons
        public void HandleStatusEffects(StatusEffectsSO so)
        {
            bool updatedSOIcon = false;
            for (int i = 0; i < statusEffects.Count; i++)
            {
                if (statusEffects[i].so.statusEffectType == so.statusEffectType
                    && statusEffects[i].so.affectedStat == so.affectedStat)
                {
                    //Debug.Log("Update Icon: " + statusEffects[i].name);
                    updatedSOIcon = true;
                    statusEffects[i].so = so;
                    statusEffects[i].name = so.name + " icon";
                    break;
                }
            }

            if (!updatedSOIcon)
            {
                
                StatusEffectUI_Icon newIcon = Instantiate(so.StatusEffectIcon, iconContainer.transform);
                newIcon.InitIcon(so);
                newIcon.gameObject.name = so.name + " icon";
                //Debug.Log("create Icon: " + newIcon.gameObject.name);
                statusEffects.Add(newIcon);
            }
        }

        //remove the specific icon related to the received status effect
        public void RemoveStatusEffectIcon(StatusEffectsSO so)
        {
            for (int i = 0; i < statusEffects.Count; i++)
            {
                if (statusEffects[i].so.statusEffectType == so.statusEffectType
                    && statusEffects[i].so.affectedStat == so.affectedStat)
                {
                    Destroy(statusEffects[i].gameObject);
                    statusEffects.RemoveAt(i);
                }
            }
        }

        //update the icons
        private void UpdateStatusEffects()
        {

            for (int i = 0; i < statusEffects.Count; i++)
            {
                statusEffects[i].UpdateIcon();
            }
        }

        #endregion

        #region Animating HUD

        //check if the button is currently pressed & check if the animation already had happend
        //start the animation is it hasn't played yet, else do nothing.
        //if it is not pressed check if the animation has been played and perform it based on that.
        private void UpdateHUDAbilities()
        {
            if (abilitiesOpen)
            {
                if (!isAnimated)
                {
                    StartCoroutine(AnimateAbilitiesOpen());
                }
            }
            else 
            {
                if (isAnimated)
                {
                    StartCoroutine(AnimateAbilitiesClosed());
                }
            }
        }

        //HUD animation, fades the small one out and the big one in.
        private IEnumerator AnimateAbilitiesOpen()
        {
            hudAbilities_S.GetComponent<CanvasGroup>().DOFade(0, animateDuration);
            hudAbilities_S.DORotate(new Vector3(0,0,-50), animateDuration, RotateMode.Fast);
            hudAbilities_S.DOScale(3f, animateDuration);
            hudAbilities_L.GetComponent<CanvasGroup>().DOFade(1, animateDuration);
            hudAbilities_L.DORotate(Vector3.zero, animateDuration, RotateMode.Fast);
            hudAbilities_L.DOScale(1.5f, animateDuration);
            isAnimated = true;
            yield return new WaitForSeconds(animateDuration);
        }

        //HUD animation, fades the small in and the big one in.
        private IEnumerator AnimateAbilitiesClosed()
        {
            hudAbilities_S.GetComponent<CanvasGroup>().DOFade(1, animateDuration);
            hudAbilities_S.DORotate(Vector3.zero, animateDuration, RotateMode.Fast);
            hudAbilities_S.DOScale(1.5f, animateDuration);
            hudAbilities_L.GetComponent<CanvasGroup>().DOFade(0, animateDuration);
            hudAbilities_L.DORotate(new Vector3(0, 0, 50), animateDuration, RotateMode.Fast);
            hudAbilities_L.DOScale(.75f, animateDuration);
            yield return new WaitForSeconds(animateDuration);
            isAnimated = false;
        }


        #endregion

        #region HUD Prompts

        public void TurnOnPrompts()
        {
            for (int i = 0; i < hudPrompts.Count; i++)
            {
                hudPrompts[i].gameObject.SetActive(true);
            }
        }

        public void TurnOffPrompts() 
        {
            for (int i = 0; i < hudPrompts.Count; i++)
            {
                hudPrompts[i].gameObject.SetActive(false);
            }
        }
        
        #endregion

    }
}