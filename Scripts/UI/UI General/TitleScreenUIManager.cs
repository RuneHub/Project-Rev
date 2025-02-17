using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace KS
{
    public class TitleScreenUIManager : BaseUIManager
    {
        [SerializeField] SceneManagement sceneManagement;

        [SerializeField] UIManager UIManager;
        [SerializeField] PlayerManager player;
        [SerializeField] CinemachineVirtualCamera introVCam;
        [SerializeField] CutsceneManager CutsceneManager;
        [SerializeField] CanvasFading canvasFading;

        [SerializeField] PlayableAsset introCutscene;

        private void Awake()
        {
            sceneManagement = SceneManagement.instance;
            if (sceneManagement.reloadState == SceneManagement.ReloadState.intoGameplay)
            {
                StartCoroutine(WaitThenPlay());
            }
            else
            {
                UIManager.instance.menuWindowIsOpen = true;
                OpenMenu();
            }
        }

        public void PlayGame()
        {
            Debug.Log("Play Game");
            player.animator.SetBool("OnGameStart", false);
            CloseMenu();
            UIManager.instance.menuWindowIsOpen = false;
            introVCam.Priority = 9;
            UIManager.LockCursor();
            CutsceneManager.PlayCutscene(introCutscene);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public override void OpenMenu()
        {
            Debug.Log("Opening titlescreen");
            base.OpenMenu();
        }

        public override void CloseMenu()
        {
            base.CloseMenu();
        }

        public override void CloseMenuAfterFixedUpdate()
        {
            base.CloseMenuAfterFixedUpdate();
        }

        IEnumerator WaitThenPlay()
        {
            UIManager.instance.menuWindowIsOpen = false;
            CloseMenu();
            yield return new WaitForSeconds(canvasFading.GetDuration()/5);
            PlayGame();
            sceneManagement.reloadState = SceneManagement.ReloadState.None;
        }
    }
}