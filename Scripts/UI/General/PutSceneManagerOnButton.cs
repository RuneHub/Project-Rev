using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KS 
{
    public class PutSceneManagerOnButton : MonoBehaviour
    {
        Button button;
        [SerializeField] SceneManagement sceneManagement;
        [SerializeField] bool resetWholeScene = true;

        private void Awake()
        {
            button  = GetComponent<Button>();
            sceneManagement = SceneManagement.instance;

            if (resetWholeScene)
            {
                button.onClick.AddListener(sceneManagement.ReloadWholeGameScene);
            }
            else
            {
                button.onClick.AddListener(sceneManagement.ReloadGameplayScene);
            }

        }
    }
}