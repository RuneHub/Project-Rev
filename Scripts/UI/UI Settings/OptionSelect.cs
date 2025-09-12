using KSTween;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KS
{
    public class OptionSelect : MonoBehaviour
    {
        public List<Image> Options = new List<Image>();

        private int selectedOption;
        Vector3 targetPos;

        [SerializeField] Vector3 optionStep;
        [SerializeField] RectTransform optionRect;

        [SerializeField] float tweenTime;

        private void Awake()
        {
            selectedOption = 0;
            targetPos = optionRect.localPosition;
        }

        public void SetOption(int selectedIndex)
        {
            Debug.Log("Set index: " + selectedIndex);
        }

        public void NextOption()
        {
            if (selectedOption < Options.Count)
            {
                selectedOption++;
                targetPos += optionStep;
                MoveOption();
            }
        }

        public void PreviousOption()
        {
            if (selectedOption > 0)
            {
                selectedOption--;
                targetPos -= optionStep;
                MoveOption();
            }
        }

        private void MoveOption()
        {
            optionRect.TweenPosition(targetPos, tweenTime);
            SetOption(selectedOption);
        }

    }
}