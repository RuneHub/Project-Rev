using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace KS
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {
        [Header("Message Popup")]
        [SerializeField] GameObject popupMessageGameObject;
        [SerializeField] TextMeshProUGUI popUpMessageText;

        [Header("Quest Failed popup")]
        [SerializeField] GameObject questFailedGameObject;
        [SerializeField] TextMeshProUGUI questFailedPopupBackgroundText;
        [SerializeField] TextMeshProUGUI questFailedPopupText;
        [SerializeField] CanvasGroup questFailedPopUpCanvasGroup;

        public void CloseAllPopUpWindows()
        {
            popupMessageGameObject.SetActive(false);
            PlayerUIManager.instance.popUpWindowisOpen = false;
        }

        public void SendPlayerMessagePopUp(string messageText)
        {
            PlayerUIManager.instance.popUpWindowisOpen = true;
            popUpMessageText.text = messageText;
            popupMessageGameObject.SetActive(true);
        }

        public void SendQuestFailedPopup()
        {
            //activate Post Processing effects

            questFailedGameObject.SetActive(true);
            questFailedPopupBackgroundText.characterSpacing = 0;
            StartCoroutine(StretchPopUpTextOverTime(questFailedPopupBackgroundText, 8, 20f));
            StartCoroutine(FadeIn(questFailedPopUpCanvasGroup, 5));
            StartCoroutine(WaitThenFadeOutPopupOverTimer(questFailedPopUpCanvasGroup, 2, 5));
        }

        private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
        {
            if (duration > 0)
            {
                text.characterSpacing = 0; //resets spacing
                float timer = 0;
                yield return null;

                while (timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20)); //might remove the divide depending on the look.
                    yield return null;
                }

            }
        }

        private IEnumerator FadeIn(CanvasGroup canvas, float duration)
        {
            if (duration > 0)
            {
                canvas.alpha = 0;
                float timer = 0;
                yield return null;

                while (timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                    yield return null;
                }

            }

            canvas.alpha = 1;
            yield return null;

        }

        private IEnumerator WaitThenFadeOutPopupOverTimer(CanvasGroup canvas, float duration, float delay)
        {
            if (duration > 0)
            {
                while (delay > 0)
                {
                    delay = delay - Time.deltaTime;
                    yield return null;
                }

                canvas.alpha = 1;
                float timer = 0;
                yield return null;

                while (timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime); 
                    yield return null;
                }

            }

            canvas.alpha = 0;
            yield return null;

        }

    }
}