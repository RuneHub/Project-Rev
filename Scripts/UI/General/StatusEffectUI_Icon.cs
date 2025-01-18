using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KS
{
    public class StatusEffectUI_Icon : MonoBehaviour
    {
        public Sprite SE;

        public StatusEffectsSO so;

        private Image Icon;
        private CanvasGroup canvas;

        public void InitIcon(StatusEffectsSO owner)
        {
            so = owner;
            Icon = GetComponent<Image>();
            canvas = GetComponent<CanvasGroup>();

            Icon.sprite = SE;

        }

        public void UpdateIcon()
        {
            if (so.IconBlink)
            {
                canvas.alpha = Mathf.Lerp(0, 1, Mathf.PingPong(Time.time * so.blinkSpeed, 1));
            }

            if (!so.Active)
            {
                canvas.alpha = 0;
                
            }

        }

    }
}