using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace KS
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        private PlayerCombatManager combatManager;

        [SerializeField] CanvasGroup[] canvasGroup;

        [Header("Player Vitality")]
        [SerializeField] GameObject playerVitality;

        [SerializeField]
        private Slider playerHealthSlider;

        [Header("Ability Slots")]
        [SerializeField] UISkillSlot skillSlotIcon_North;
        [SerializeField] UISkillSlot skillSlotIcon_South;
        [SerializeField] UISkillSlot skillSlotIcon_West;
        [SerializeField] UISkillSlot skillSlotIcon_East;
        public Sprite skillSlotIcon_Empty;
        public Sprite skillSlotIcon_NoIcon;

        [Header("Status Effects")]
        [SerializeField] private List<StatusEffectUI_Icon> statusEffects;
        [SerializeField] GridLayoutGroup iconContainer;
        public StatusEffectUI_Icon icon;

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

            UpdateStatusEffects();
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
            if (PlayerUIManager.instance.player.modeManager.currentMode == PlayMode.FreeMode)
            {
                PlayerUIManager.instance.floatingManager.DisableSetLockOnVisual();
            }
            else if (PlayerUIManager.instance.player.modeManager.currentMode == PlayMode.LockOnMode)
            {
                PlayerUIManager.instance.floatingManager.EnableLockOnVisual(PlayerUIManager.instance.player.cameraHandler.currentLockOnTarget);
            }

        }

        //set up player health bar values
        public void SetupPlayerVitality()
        {
            playerHealthSlider.maxValue = PlayerUIManager.instance.player.playerStats.maxHealth;
            playerHealthSlider.value = PlayerUIManager.instance.player.playerStats.currentHealth;

        }

        //Checks the player health in the stat manager and modifies the UI. 
        private void CheckPlayerVitality()
        {
            playerHealthSlider.value = PlayerUIManager.instance.player.playerStats.currentHealth;
        }
        #endregion

        #region Skill slot

        #region Set SKill

        //sets the Icon of the skills depending on which place they are in.
        //e.g. the correct icon in the northon slot.
        // also checks if the skills SO have proper Icons.
        public void SetSkillSlotIcon()
        {
            combatManager = PlayerUIManager.instance.player.combatManager;

            SetSkillSlotIconN();
            SetSkillSlotIconS();
            SetSkillSlotIconW();
            SetSkillSlotIconE();
        }

        public void SetSkillSlotIconN()
        {
            //when there is a skill sloted but not a icon on the SO.
            if (combatManager.SkillNorth != null &&
                combatManager.SkillNorth.SkillIconHUD == null)
            {
                skillSlotIcon_North.AbilityIcon.sprite = skillSlotIcon_NoIcon;
            }
            //set the SO Icon on the HUD
            else if (combatManager.SkillNorth != null)
            {
                skillSlotIcon_North.AbilityIcon.sprite = combatManager.SkillNorth.SkillIconHUD;
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
            if (combatManager.SkillSouth != null &&
               combatManager.SkillSouth.SkillIconHUD == null)
            {
                skillSlotIcon_South.AbilityIcon.sprite = skillSlotIcon_NoIcon;
            }
            //set the SO Icon on the HUD
            else if (combatManager.SkillSouth != null)
            {
                skillSlotIcon_South.AbilityIcon.sprite = combatManager.SkillSouth.SkillIconHUD;
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
            if (combatManager.SkillWest != null &&
                combatManager.SkillWest.SkillIconHUD == null)
            {
                skillSlotIcon_West.AbilityIcon.sprite = skillSlotIcon_NoIcon;
            }
            //set the SO Icon on the HUD
            else if (combatManager.SkillWest != null)
            {
                skillSlotIcon_West.AbilityIcon.sprite = combatManager.SkillWest.SkillIconHUD;
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
            if (combatManager.SkillEast != null &&
               combatManager.SkillEast.SkillIconHUD == null)
            {
                skillSlotIcon_East.AbilityIcon.sprite = skillSlotIcon_NoIcon;
            }
            //set the SO Icon on the HUD
            else if (combatManager.SkillEast != null)
            {
                skillSlotIcon_East.AbilityIcon.sprite = combatManager.SkillEast.SkillIconHUD;
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

            if (skillSlotIcon_North != null && combatManager.SkillNorth != null)
            {
                if (CooldownHandler.instance.isOnCooldown(combatManager.SkillNorth.skillID))
                {
                    skillSlotIcon_North.CDMask.fillAmount = CooldownHandler.instance.GetCooldownTimer(combatManager.SkillNorth.skillID) / combatManager.SkillNorth.cooldown;
                }
                else
                {
                    skillSlotIcon_North.CDMask.fillAmount = 0;
                }

            }

            if (skillSlotIcon_South != null && combatManager.SkillSouth != null)
            {
                if (CooldownHandler.instance.isOnCooldown(combatManager.SkillSouth.skillID))
                {
                    skillSlotIcon_South.CDMask.fillAmount = CooldownHandler.instance.GetCooldownTimer(combatManager.SkillSouth.skillID) / combatManager.SkillSouth.cooldown;
                }
                else
                {
                    skillSlotIcon_South.CDMask.fillAmount = 0;
                }

            }

            if (skillSlotIcon_West != null && combatManager.SkillWest != null)
            {
                if (CooldownHandler.instance.isOnCooldown(combatManager.SkillWest.skillID))
                {
                    skillSlotIcon_West.CDMask.fillAmount = CooldownHandler.instance.GetCooldownTimer(combatManager.SkillWest.skillID) / combatManager.SkillWest.cooldown;
                }
                else
                {
                    skillSlotIcon_West.CDMask.fillAmount = 0;
                }

            }

            if (skillSlotIcon_East != null && combatManager.SkillEast != null)
            {
                if (CooldownHandler.instance.isOnCooldown(combatManager.SkillEast.skillID))
                {
                    skillSlotIcon_East.CDMask.fillAmount = CooldownHandler.instance.GetCooldownTimer(combatManager.SkillEast.skillID) / combatManager.SkillEast.cooldown;
                }
                else
                {
                    skillSlotIcon_East.CDMask.fillAmount = 0;
                }

            }

        }

        #endregion

        #region Status Effects,
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

    }
}