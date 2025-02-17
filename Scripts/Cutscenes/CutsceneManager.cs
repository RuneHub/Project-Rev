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

        [Header("References")]
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private AIBossManager bossManager;
        [SerializeField] private AirshipStatus airshipStatus;

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

        public void TurnOffPlayerInput()
        {
            playerManager.inputs.DisableGameplayInput();
        }

        public void TurnOnPlayerInputs()
        {
            playerManager.inputs.EnableGameplayInput();
        }

        public void TurnOffBossBehaviour()
        {
            bossManager.behaviourRunner.enabled = false;
        }

        public void TurnOnBossBehaviour() 
        {
            bossManager.behaviourRunner.enabled = true;
        }

        public void AirshipSwapParts()
        {
            airshipStatus.SwapParts();
        }

    }
}