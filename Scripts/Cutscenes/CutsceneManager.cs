    using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace KS
{
    public class CutsceneManager : MonoBehaviour
    {
        [Header("Cutscene")]
        [SerializeField] private PlayableDirector pd;
        [SerializeField] private float SkipToTimestamp;
        public bool skipableCS = false;

        [Header("Props")]
        public GameObject csLowell;
        public GameObject csSeyliana;
        public GameObject csClone;

        [Header("References")]
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private AIBossManager bossManager;
        [SerializeField] private AirshipStatus airshipStatus;
        [SerializeField] private CanvasFading fadingCanvas;
        [SerializeField] private CameraManager cameraManager;
        [SerializeField] private AIBossHpTriggerManager TriggerManager;
        [SerializeField] private UIManager uiManager;

        [Header("others")]
        [SerializeField] private GameObject UILabelFocus;

        private void Awake()
        {
            pd = GetComponent<PlayableDirector>();
        }

        private void Start()
        {
            pd.Stop();

        }

        public void PlayCutscene(PlayableAsset PA)
        {
            pd.playableAsset = PA;
            pd.Play();
            playerManager.currentCSManager = this;
            playerManager.InCutscene = true;
        }

        public void CutsceneEnd()
        {
            playerManager.currentCSManager = null;
            playerManager.InCutscene = false;
        }

        public void SkipToPoint()
        {
            if (skipableCS)
            {
                pd.time = SkipToTimestamp;
            }
        }

        #region  turn on/off
        public void TurnOffPlayerInput()
        {
            playerManager.inputs.DisableGameplayInput();
            playerManager.playerLocomotion.enabled = false;
        }

        public void TurnOnPlayerInputs()
        {
            playerManager.inputs.EnableGameplayInput();
            playerManager.playerLocomotion.enabled = true;
        }

        public void TurnOffBossBehaviour()
        {
            bossManager.behaviourRunner.enabled = false;
            bossManager.bossLocomotion.enabled = false;
        }

        public void TurnOnBossBehaviour() 
        {
            bossManager.behaviourRunner.enabled = true;
            bossManager.bossLocomotion.enabled = true;
        }

        public void TurnBossInvisible()
        {
            bossManager.animationEvents.CharInvisible();
        }

        public void TurnBossVisible()
        {
            bossManager.animationEvents.CharVisible();
        }
        #endregion

        #region environmental
        public void AirshipSwapParts()
        {
            airshipStatus.SwapParts();
        }
        #endregion

        #region UI
        public void BlackScreenFadeIn()
        {
            fadingCanvas.FadeIn();
        }

        public void BlackScreenFadeOut()
        {
            fadingCanvas.FadeOut();
        }

        public float GetCanvasFadingDuration()
        {
            return fadingCanvas.GetDuration();
        }

        public void HandleQuestIntro()
        {
            UIManager.instance.popupManager.SendQuestIntro();
        }

        public void HandleQuestClear()
        {
            UIManager.instance.popupManager.SendQuestClearedPopup();
        }

        public void HandleQuestClearMenu()
        {
            UIManager.instance.popupManager.SendQuestClearMenu();
        }

        public void ToggleHUD(bool toggle)
        {
           playerManager.hudManager.ToggleHUD(toggle);
        }

        public void ToggleHUDBossVitality(bool toggle)
        {
            playerManager.hudManager.ToggleBossHUD(true, 1f);
        }

        public void ToggleDamageLabel(bool toggle)
        {
            if (toggle)
            {
                FloatingUIManager.instance.GetComponent<CanvasGroup>().alpha = 1;
                FloatingUIManager.instance.GetComponentInChildren<UIWorldLookAt>().gameObject.SetActive(false);
            }
            else
            {
                FloatingUIManager.instance.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
        #endregion

        #region Props
        //player
        public void SetPropCharacterON()
        {
            csLowell.SetActive(true);
            csLowell.GetComponent<GetCharacterTheme>().UpdateCharacterTheme();
            csSeyliana.SetActive(true);
        }

        public void SetCharacterTheme()
        {
            csLowell.GetComponent<GetCharacterTheme>().UpdateCharacterTheme();
        }

        public void SetPropCharacterOFF()
        {
            csLowell.SetActive(false);
            csClone.SetActive(false);
        }

        //boss
        public void SetPropBossCharOn()
        {
            csSeyliana.SetActive(true);
        }
        public void SetPropBossCharOff()
        {
            csSeyliana.SetActive(false);
        }

        //clone
        public void startCloneDissolve()
        {
            csClone.GetComponent<CSLowellAnimationEvents>().HandleDissolve();
        }
        #endregion

        #region Camera

        public void CameraShake()
        {
            cameraManager.EffectShake(.3f, .7f);
        }

        public void RotateCameraToZero()
        {
            cameraManager.ResetCamera(0);
        }

        public void RotateCameraToNinety()
        {
            cameraManager.ResetCamera(90);
        }

        #endregion

        #region Mechanics

        public void turnBossOffMech()
        {
            TriggerManager.TurnOffBoss();
        }

        public void startHPTrigger()
        {
            TriggerManager.StartHpTrigger();
        }

        public void StartUltimateAttackRecovery()
        {
            playerManager.UltManager.HandleUltimateAttackRecovery();
            bossManager.statManager.RecoverUltimateHit();
            csClone.GetComponent<CSLowellAnimationEvents>().DestroyUltVFXParent();
        }

        public void DoUltimateAttackDamage()
        {
            Debug.Log("Do Ultimate Attack Damage");
            var (damage, isCrit) = StatCalculator.CalculateDamage(playerManager, playerManager.UltManager.LaserUltDamage, bossManager);
            playerManager.UltManager.AddUltDamage(damage);
            bossManager.statManager.TakeDamage(damage, false, playerManager.playerStats.HUDDisplayColor);
        }

        public void DisplayUltDamage()
        {
            playerManager.UltManager.DisplayTotalUltDamage();
        }

        #endregion

    }
}