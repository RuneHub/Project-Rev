using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSTween
{

    public class UITweenMove : MonoBehaviour
    {
        [Header("Status")]
        [SerializeField] private bool OnAwake;
        [SerializeField] private bool ResetOnDisable;

        [Header("Variables")]
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private float duration;
        [SerializeField] private AnimationCurve easeCurve;

        [Header("Position")]
        [SerializeField] private Vector2 startPos;
        [SerializeField] private Vector2 targetPos;
        [SerializeField] private DefaultPosition position;

        [SerializeField]
        private enum DefaultPosition
        {
            TargetPosition,
            StartingPosition
        }

        private void Awake  ()
        {
            switch (position)
            {
                case DefaultPosition.TargetPosition:
                    targetPos = rectTransform.localPosition;
                    rectTransform.localPosition = startPos;
                    break;
                case DefaultPosition.StartingPosition:
                    startPos = rectTransform.localPosition;
                    break;
                default:
                    startPos = rectTransform.localPosition;
                    break;
            }

           
        }

        private void OnDisable()
        {
            Debug.Log("disabled");
            if(ResetOnDisable)
            {
                rectTransform.localPosition = startPos;
            }
        }

        private void OnEnable()
        {
            if (OnAwake)
            {
                HandleMove();
            }
        }

        [ContextMenu("move")]
        public void MoveUI()
        {
            HandleMove();
        }

        private void HandleMove()
        {
            rectTransform.DOAnchorPos(targetPos, duration, false).SetEase(easeCurve).SetUpdate(true);
        }

    }
}