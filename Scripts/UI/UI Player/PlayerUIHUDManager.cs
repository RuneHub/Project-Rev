using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace KS
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        [SerializeField]private PlayerManager playerManager;
        [SerializeField] private AIBossManager bossManager;

        [SerializeField] CanvasGroup[] canvasGroup;

        [Header("Player Vitality")]
        [SerializeField] GameObject playerVitality;
        [SerializeField] private Slider playerHealthSlider;
        [SerializeField] private Slider playerEaseHealthSlider;
        [SerializeField] private float easeHealthLerpSpeed = 0.05f;
        [SerializeField] private TextMeshProUGUI playerHealthText;

        [Header("Boss Vitality")]
        [SerializeField] GameObject bossVitality;
        [SerializeField] private Slider bossHealthSlider;
        [SerializeField] private Slider bossEaseHealthSlider;
        [SerializeField] private TextMeshProUGUI bossHealthText;

        [Header("Ability Slots")]
        public Sprite skillSlotIcon_Empty;
        public Sprite skillSlotIcon_NoIcon;

        [SerializeField] List<UISkillSlot> iconSlot_North = new List<UISkillSlot>(2);
        [SerializeField] List<UISkillSlot> iconSlot_South = new List<UISkillSlot>(2);
        [SerializeField] List<UISkillSlot> iconSlot_West = new List<UISkillSlot>(2);
        [SerializeField] List<UISkillSlot> iconSlot_East = new List<UISkillSlot>(2);

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
            CheckBossVitality();

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

        //changes the UI text to the current player health.
        //get called with the "OnChanged" events on the slider
        public void EditPlayerHealthText()
        {
            playerHealthText.text = playerManager.playerStats.currentHealth.ToString() + " / " + playerManager.playerStats.maxHealth.ToString();
        }
        #endregion

        #region boss vitality

        //turns the boss healthbar visible with a fade. or turns it invisible instantly.
        public void toggleBossHUD(bool status, float time)
        {
            if (status)
            {
                //bossVitality.GetComponent<CanvasGroup>().alpha
                Mathf.Lerp(bossVitality.GetComponent<CanvasGroup>().alpha, 1, time * Time.deltaTime);
            }
            else
            {
                bossVitality.GetComponent<CanvasGroup>().alpha = 0;
            }
        }

        //Sets up the boss healthbar values.
        public void SetupBossVitality()
        {
            bossHealthSlider.maxValue = bossManager.statManager.maxHealth;
            bossHealthSlider.value = bossManager.statManager.currentHealth;

            bossEaseHealthSlider.maxValue = bossManager.statManager.maxHealth;
            bossEaseHealthSlider.value = bossManager.statManager.currentHealth;
        }

        //checks the boss health and modifies the UI.
        //uses the same easing method as the players healthbar.
        private void CheckBossVitality()
        {
            bossHealthSlider.value = bossManager.statManager.currentHealth;
            bossEaseHealthSlider.value = Mathf.Lerp(bossEaseHealthSlider.value, bossManager.statManager.currentHealth, easeHealthLerpSpeed);
        }

        //calculates the boss health into percentage and modifies the UI.
        //get called with the "OnChanged" events on the slider
        public void EditBossHealthText ()
        {
            //make the percentage.
            float bossHealthPercentage = Mathf.Floor((bossManager.statManager.currentHealth / bossManager.statManager.maxHealth) * 100);

            //set to UI text
            bossHealthText.text = bossHealthPercentage.ToString() + "%";
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
            for (int i = 0; i < iconSlot_North.Count; i++)
            {
                //when there is a skill sloted but not a icon on the SO.
                if (playerManager.combatManager.SkillNorth != null &&
                    playerManager.combatManager.SkillNorth.SkillIconHUD == null)
                {
                    iconSlot_North[i].AbilityIcon.sprite = skillSlotIcon_NoIcon;
                    Debug.Log("skill exists but no icon!");
                }
                //set the SO Icon on the HUD
                else if (playerManager.combatManager.SkillNorth != null)
                {
                    if (!iconSlot_North[i].Small)
                    {
                        iconSlot_North[i].AbilityIcon.sprite = playerManager.combatManager.SkillNorth.SkillIconHUD;
                    }
                    else
                    {
                        // add the small icon from the correct list.
                        iconSlot_North[i].AbilityIcon.sprite = playerManager.combatManager.SkillNorth.SkillIconHUDSmall;
                    }
                }
                //if there is no Skill sloted, set the empty icon on the HUD.
                else
                {
                    iconSlot_North[i].AbilityIcon.sprite = skillSlotIcon_Empty;
                }
            }
        }

        public void SetSkillSlotIconS()
        {
            for (int i = 0; i < iconSlot_South.Count; i++)
            {
                //when there is a skill sloted but not a icon on the SO.
                if (playerManager.combatManager.SkillSouth != null &&
                playerManager.combatManager.SkillSouth.SkillIconHUD == null)
                {
                    iconSlot_South[i].AbilityIcon.sprite = skillSlotIcon_NoIcon;
                }
                //set the SO Icon on the HUD
                else if (playerManager.combatManager.SkillSouth != null)
                {
                    if (!iconSlot_South[i].Small)
                    {
                        iconSlot_South[i].AbilityIcon.sprite = playerManager.combatManager.SkillSouth.SkillIconHUD;
                    }
                    else 
                    {
                        // add the small icon from the correct list.
                        iconSlot_South[i].AbilityIcon.sprite = playerManager.combatManager.SkillSouth.SkillIconHUDSmall;
                    }
                }
                //if there is no Skill sloted, set the empty icon on the HUD.
                else
                {
                    iconSlot_South[i].AbilityIcon.sprite = skillSlotIcon_Empty;
                }
            }
        }

        public void SetSkillSlotIconW()
        {
            for (int i = 0; i < iconSlot_West.Count; i++)
            {
                //when there is a skill sloted but not a icon on the SO.
                if (playerManager.combatManager.SkillWest != null &&
                    playerManager.combatManager.SkillWest.SkillIconHUD == null)
                {
                    iconSlot_West[i].AbilityIcon.sprite = skillSlotIcon_NoIcon;
                }
                //set the SO Icon on the HUD
                else if (playerManager.combatManager.SkillWest != null)
                {
                    if (!iconSlot_West[i].Small)
                    {
                        iconSlot_West[i].AbilityIcon.sprite = playerManager.combatManager.SkillWest.SkillIconHUD;
                    }
                    else
                    {
                        // add the small icon from the correct list. 
                        iconSlot_West[i].AbilityIcon.sprite = playerManager.combatManager.SkillWest.SkillIconHUDSmall;
                    }

                }
                //if there is no Skill sloted, set the empty icon on the HUD.
                else
                {
                    iconSlot_West[i].AbilityIcon.sprite = skillSlotIcon_Empty;
                }
            }
        }

        public void SetSkillSlotIconE()
        {
            for (int i = 0; i < iconSlot_East.Count; i++)
            {
                //when there is a skill sloted but not a icon on the SO.
                if (playerManager.combatManager.SkillEast != null &&
                   playerManager.combatManager.SkillEast.SkillIconHUD == null)
                {
                    iconSlot_East[i].AbilityIcon.sprite = skillSlotIcon_NoIcon;
                }
                //set the SO Icon on the HUD
                else if (playerManager.combatManager.SkillEast != null)
                {
                    if (!iconSlot_East[i].Small)
                    {
                        iconSlot_East[i].AbilityIcon.sprite = playerManager.combatManager.SkillEast.SkillIconHUD;
                    }
                    else
                    {
                        // add the small icon from the correct list.
                        iconSlot_East[i].AbilityIcon.sprite = playerManager.combatManager.SkillEast.SkillIconHUDSmall;
                    }
                }
                //if there is no Skill sloted, set the empty icon on the HUD.
                else
                {
                    iconSlot_East[i].AbilityIcon.sprite = skillSlotIcon_Empty;
                }
            }
        }
        #endregion

        //checks every frame, checks if the skill is on cooldown.
        // if it is then update the grey fillamount of the ui for visual feedback
        private void CheckSkillCoodown()
        {

            if (iconSlot_North.Count > 0 && playerManager.combatManager.SkillNorth != null)
            {
                if (CooldownHandler.instance.isOnCooldown(playerManager.combatManager.SkillNorth.skillID))
                {
                    for (int i = 0; i < iconSlot_North.Count; i++)
                    {
                        iconSlot_North[i].CDMask.fillAmount = CooldownHandler.instance.GetCooldownTimer(playerManager.combatManager.SkillNorth.skillID) / playerManager.combatManager.SkillNorth.cooldown;
                    }

                }
                else
                {
                    for (int i = 0; i < iconSlot_North.Count; i++)
                    {
                        iconSlot_North[i].CDMask.fillAmount = 0;
                    }
                }

            }

            if (iconSlot_South.Count > 0 && playerManager.combatManager.SkillSouth != null)
            {
                if (CooldownHandler.instance.isOnCooldown(playerManager.combatManager.SkillSouth.skillID))
                {
                    for (int i = 0; i < iconSlot_South.Count; i++)
                    {
                        iconSlot_South[i].CDMask.fillAmount = CooldownHandler.instance.GetCooldownTimer(playerManager.combatManager.SkillSouth.skillID) / playerManager.combatManager.SkillSouth.cooldown;
                    }

                }
                else
                {
                    for (int i = 0; i < iconSlot_South.Count; i++)
                    {
                        iconSlot_South[i].CDMask.fillAmount = 0;
                    }
                }

            }

            if (iconSlot_West.Count > 0 && playerManager.combatManager.SkillWest != null)
            {
                if (CooldownHandler.instance.isOnCooldown(playerManager.combatManager.SkillWest.skillID))
                {
                    for (int i = 0; i < iconSlot_West.Count; i++)
                    {
                        iconSlot_West[i].CDMask.fillAmount = CooldownHandler.instance.GetCooldownTimer(playerManager.combatManager.SkillWest.skillID) / playerManager.combatManager.SkillWest.cooldown;
                    }

                }
                else
                {
                    for (int i = 0; i < iconSlot_West.Count; i++)
                    {
                        iconSlot_West[i].CDMask.fillAmount = 0;
                    }
                }

            }

            if (iconSlot_East.Count > 0 && playerManager.combatManager.SkillEast != null)
            {
                if (CooldownHandler.instance.isOnCooldown(playerManager.combatManager.SkillEast.skillID))
                {
                    for (int i = 0; i < iconSlot_East.Count; i++)
                    {
                        iconSlot_East[i].CDMask.fillAmount = CooldownHandler.instance.GetCooldownTimer(playerManager.combatManager.SkillEast.skillID) / playerManager.combatManager.SkillEast.cooldown;
                    }
                }
                else
                {
                    for (int i = 0; i < iconSlot_East.Count; i++)
                    {
                        iconSlot_East[i].CDMask.fillAmount = 0;
                    }
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