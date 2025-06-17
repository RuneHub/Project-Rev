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

        [Header("Props")]
        public GameObject csLowell;
        public GameObject csSeyliana;

        [Header("References")]
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private AIBossManager bossManager;
        [SerializeField] private AirshipStatus airshipStatus;
        [SerializeField] private CanvasFading fadingCanvas;

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
        #endregion

        #region Props
        public void SetPropCharacterON()
        {
            csLowell.SetActive(true);
            csSeyliana.SetActive(true);
        }

        public void SetPropCharacterOFF()
        {
            csLowell.SetActive(false);
            csSeyliana.SetActive(false);
        }
        #endregion

    }
}