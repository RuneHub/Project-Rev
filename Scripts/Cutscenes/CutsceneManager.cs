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

        [Header("References")]
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private AIBossManager bossManager;
        [SerializeField] private AirshipStatus airshipStatus;
        [SerializeField] private CanvasFading fadingCanvas;
        [SerializeField] private CameraManager cameraManager;
        [SerializeField] private AIBossHpTriggerManager TriggerManager;

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

        public void ToggleHUD(bool toggle)
        {
            playerManager.hudManager.ToggleHUD(toggle);
        }
        #endregion

        #region Props
        public void SetPropCharacterON()
        {
            csLowell.SetActive(true);
            csLowell.GetComponent<GetCharacterTheme>().UpdateCharacterTheme();
            csSeyliana.SetActive(true);
        }

        public void SetPropCharacterOFF()
        {
            csLowell.SetActive(false);
            csSeyliana.SetActive(false);
        }

        public void SetPropBossCharOn()
        {
            csSeyliana.SetActive(true);
        }
        public void SetPropBossCharOff()
        {
            csSeyliana.SetActive(false);
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
        #endregion
    }
}