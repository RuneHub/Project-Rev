using Coffee.UIEffects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KS
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {
        [Header("Message Popup")]
        [SerializeField] GameObject popupMessageGameObject;
        [SerializeField] TextMeshProUGUI popUpMessageText;

        [Header("Stage Intro")]
        [SerializeField] float IntroAppearTime;
        [SerializeField] float IntroStayTime;
        [SerializeField] float IntroDisappearTime;

        [SerializeField] List<GameObject> introObjects = new List<GameObject>();

        [Header("Quest Window")]
        [SerializeField] GameObject questWindow;

        [Header("Failed Window")]
        [SerializeField] GameObject failedWindow;
        [SerializeField] GameObject questFailedObject;
        [SerializeField] float qfoEntranceTime = 0.8f;

        [Header("Cleared Window")]
        [SerializeField] GameObject clearedWindow;

        [Header("Middle Clear Object")]
        [SerializeField] float MCOintroTime;
        [SerializeField] float MCOWaitForEffects;
        [SerializeField] float MCOShutDownTimer;
        [SerializeField] GameObject MiddleClearObject;
        [SerializeField] GameObject MCOBackParticels;
        [SerializeField] GameObject MCOQuest;
        [SerializeField] GameObject MCOCleared;
        [SerializeField] GameObject MCOFrontParticels;

        [Header("Clear Menu")]
        [SerializeField] GameObject CMBgLeft;
        [SerializeField] GameObject CMBgRight;
        [SerializeField] GameObject CMClearObject;
        [SerializeField] GameObject CMClearObjectQuest;
        [SerializeField] float CMBgFadeInTime;
        [SerializeField] float CMClearSlideInTime;

        [Header("Result Menu")]
        [SerializeField] GameObject resultOptions;
        [SerializeField] GameObject retryOption;

        [Header("Event system")]
        [SerializeField] EventSystem eventSystem;
        [SerializeField] GameObject selectOnOpen;

        public void CloseAllPopUpWindows()
        {
            popupMessageGameObject.SetActive(false);
            UIManager.instance.popUpWindowIsOpen = false;
        }

        public void SendPlayerMessagePopUp(string messageText)
        {
            UIManager.instance.popUpWindowIsOpen = true;
            popUpMessageText.text = messageText;
            popupMessageGameObject.SetActive(true);
        }

        [ContextMenu("Intro")]
        public void SendQuestIntro()
        {
            StartCoroutine(ActivateQuestIntro());
        }

        IEnumerator ActivateQuestIntro()
        {
            for (int i = 0; i < introObjects.Count; i++)
            {
                introObjects[i].SetActive(true);
                introObjects[i].GetComponent<UIAutoAnimation>().EntranceAnimation();

                yield return new WaitForSeconds(IntroAppearTime);
            }

            yield return new WaitForSeconds(IntroStayTime);

            for (int i = introObjects.Count -1; i >= 0; i--)
            {
                introObjects[i].GetComponent<UIAutoAnimation>().ExitAnimation();

                yield return new WaitForSeconds(IntroDisappearTime);

                introObjects[i].SetActive(false);
            }
        }

        [ContextMenu("Failed")]
        public void SendQuestFailedPopup()
        {
            UIManager.instance.player.inputs.DisableGameplayInput();

            StartCoroutine(QuestFailedEvent());

            eventSystem.SetSelectedGameObject(selectOnOpen);
        }

        [ContextMenu("Cleared")]
        public void SendQuestClearedPopup()
        {
            UIManager.instance.player.inputs.DisableGameplayInput();

            clearedWindow.SetActive(true);
            MiddleClearObject.SetActive(true);
            StartCoroutine(QuestClearedEvent());
        }

        [ContextMenu("Clear Menu")]
        public void SendQuestClearMenu()
        {
            UIManager.instance.player.inputs.DisableGameplayInput();

            clearedWindow.SetActive(true);

            StartCoroutine(QuestClearedMenu());

            eventSystem.SetSelectedGameObject(selectOnOpen);
        }

        private IEnumerator QuestFailedEvent()
        {
            if (failedWindow != null)
            {
                failedWindow.SetActive(true);
                failedWindow.GetComponent<UIAutoAnimation>().EntranceAnimation();
            }

            yield return new WaitForSeconds(qfoEntranceTime);

            if(questFailedObject != null)
            {
                questFailedObject.SetActive(true);
                questFailedObject.GetComponent<UIAutoAnimation>().EntranceAnimation();
                questFailedObject.GetComponent<UIEffectTweener>().PlayForward();
            }

        }

        private IEnumerator QuestClearedEvent()
        {
            MiddleClearObject.GetComponent<UIAutoAnimation>().EntranceAnimation();
            yield return new WaitForSeconds(MCOintroTime);

            MCOBackParticels.SetActive(true);
            MCOQuest.GetComponent<UIEffectTweener>().Play();
            MCOFrontParticels.SetActive(true);

            yield return new WaitForSeconds(MCOWaitForEffects);

            MiddleClearObject.GetComponent<UIAutoAnimation>().ExitAnimation();

            yield return new WaitForSeconds(MCOShutDownTimer);

            MiddleClearObject.SetActive(false);

        }

        private IEnumerator QuestClearedMenu()
        {
            CMBgLeft.SetActive(true);
            CMBgRight.SetActive(true);

            CMBgRight.GetComponent<UIAutoAnimation>().EntranceAnimation();
            CMBgLeft.GetComponent<UIAutoAnimation>().EntranceAnimation();
            yield return new WaitForSeconds(CMBgFadeInTime);

            CMClearObject.SetActive(true);
            CMClearObject.GetComponent<UIAutoAnimation>().EntranceAnimation();
            yield return new WaitForSeconds(CMClearSlideInTime);

            CMClearObjectQuest.GetComponent<UIEffectTweener>().Play();

        }

    }
}