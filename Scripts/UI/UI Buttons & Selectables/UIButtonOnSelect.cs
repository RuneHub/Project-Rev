using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace KS
{
    public class UIButtonOnSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [Header("Base UI")]
        public BaseUIManager UIManager;
        public bool useVFX;

        public bool moveOnSelect;

        [Header("UI Animations")]
        [SerializeField] private RectTransform rectTransform;

        [SerializeField] private float animateDuration = 0.5f;

        [SerializeField] private Vector2 targetPos;

        private Vector2 initialPosition;

        private void Start()
        {
            DOTween.Init();
            initialPosition = rectTransform.localPosition;

        }

        //when button is selected moves the button to the target position and puts the VFX as its own child.
        public void OnSelect(BaseEventData eventData)
        {
            if (UIManager != null)
            {
                if (useVFX)
                {
                    UIManager.GetSelectedVFX().gameObject.transform.SetParent(transform, false);
                }

                if (moveOnSelect)
                {
                    AnimateButton(targetPos);
                }

            }
        }

        //moves the button back to the initial position when deselecting.
        public void OnDeselect(BaseEventData eventData)
        {
            if (moveOnSelect)
            {
                AnimateButton(initialPosition);
            }
        }

        //a manual function that is ussable if the button needs to animate but it is already selected.
        public void ManuallySelect(bool selected)
        {
            if (selected)
            {
                AnimateButton(targetPos);
            }
            else
            {
                AnimateButton(initialPosition);
            }
        }

        //animated the button to the target position via Tween.
        private void AnimateButton(Vector2 target)
        {
            rectTransform.DOAnchorPos(target, animateDuration, false).SetEase(Ease.InOutSine).SetUpdate(true);
        }
    }

}