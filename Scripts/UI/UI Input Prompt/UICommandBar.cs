using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KS
{
    public class UICommandBar : MonoBehaviour
    {

        [SerializeField] private CanvasGroup canvasGroup;
            
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
           
        }

        public void SetCommandBar(bool state)
        {
            if (state)
            {
                canvasGroup.alpha = 1;
            }
            else
            {
                canvasGroup.alpha = 0;
            }
        }
    }
}