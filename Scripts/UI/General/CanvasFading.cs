using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KS
{
    public class CanvasFading : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeDuration = 5.0f;
        [SerializeField] private bool StartFadeIn = false;

        private void Start()
        {
            if (StartFadeIn)
            {
                canvasGroup.alpha = 1;
                FadeIn();
            }
            else
            {
                FadeOut();
            }
        }

        //fading in by making the image transparent
        public void FadeIn()
        {
            StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0, fadeDuration));
        }

        //fading out by showing the image.
        public void FadeOut()
        {
            StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1, fadeDuration));
        }

        public float GetDuration()
        {
            return fadeDuration;
        }

        //fade in/out the screen by using an image with a black color,
        //and making it gradually transparent depending on if you faidn in or out.
        // it uses unscaled deltatime so it can be used when game is paused.
        private IEnumerator FadeCanvasGroup(CanvasGroup group, float start, float end, float duration)
        {
            float elapsedTime = 0;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                group.alpha = Mathf.Lerp(start, end, elapsedTime /duration);
                yield return null;
            }

            group.alpha = end;

        }

    }
}