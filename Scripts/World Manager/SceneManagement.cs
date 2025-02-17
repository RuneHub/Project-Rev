using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KS
{
    public class SceneManagement : MonoBehaviour
    {
        public static SceneManagement instance;

        public enum ReloadState
        {
            None,
            intoWholeScene,
            intoGameplay
        }

        public TitleScreenUIManager titleScreenUIManager;
        public CanvasFading fadingCanvas;
        public ReloadState reloadState = ReloadState.None;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            titleScreenUIManager = FindObjectOfType<TitleScreenUIManager>();
            fadingCanvas = FindObjectOfType<CanvasFading>();
        }

        public void ReloadWholeGameScene()
        {
            reloadState = ReloadState.intoWholeScene;
            StartCoroutine(ReloadGameScene());
        }

        public void ReloadGameplayScene()
        {
            reloadState = ReloadState.intoGameplay;
            StartCoroutine(ReloadGameScene());
        }

        public void ReloadScene(ReloadState state)
        {
            switch (state)
            {
                case ReloadState.None:
                    break;
                case ReloadState.intoWholeScene:
                    StartCoroutine(ReloadWholeScene());
                    break;
                case ReloadState.intoGameplay:
                    StartCoroutine(ReloadGameScene());
                    break;
            }
        }

        private IEnumerator ReloadWholeScene()
        {
            fadingCanvas.FadeOut();
            yield return new WaitForSeconds(fadingCanvas.GetDuration());
            Debug.Log("whoe scene");
            string currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene);
        }

        private IEnumerator ReloadGameScene()
        {
            
            fadingCanvas.FadeOut();
            yield return new WaitForSeconds(fadingCanvas.GetDuration());

            string currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene);

        }

    }
}