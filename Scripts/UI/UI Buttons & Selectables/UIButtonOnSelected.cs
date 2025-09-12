using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KS
{
    public class UIButtonOnSelected : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public BaseUIManager UIManager;
        public bool useVFX;

        public bool moveOnSelect;
        public bool useScaledTime = false;

        [SerializeField] private RectTransform recTransform;

        public enum AnimateToDirection
        {
            Top,
            Bottom,
            Left,
            Right
        }

        [Header("Animation Setup")]
        [SerializeField] private AnimateToDirection SelectedDirection = AnimateToDirection.Right;
        [SerializeField] private AnimateToDirection DeslectDirection = AnimateToDirection.Left;
        [Space]
        [SerializeField] private Vector2 animationDistance = new Vector2(100, 100);
        [SerializeField] private AnimationCurve easingCurve = AnimationCurve.EaseInOut(0,0,1,1);
        [Range(0, 1)][SerializeField] private float animationDuration = 0.5f;

        private Vector2 initialPostion;
        private Vector2 currentPostion;

        private Vector2 upOffset, downOffset, leftOffset, rightOffset;

        private Coroutine animateButtonRoutine;

        [Header("Visualization")]
        [SerializeField] private bool displayGizmos = true;

        private enum DisplayGizmosAtLocation
        {
            Selected,
            Deslected,
            Both
        }

        [SerializeField, DrawIf("displayGizmos", true)] private DisplayGizmosAtLocation gizmoHandler;
        [SerializeField, DrawIf("displayGizmos", true)] private Color gizmoSelectedColor = Color.green;
        [SerializeField, DrawIf("displayGizmos", true)] private Color gizmoDeselectedColor = Color.red;
        [SerializeField, DrawIf("displayGizmos", true)] private Color gizmoInitiallocationColor = Color.grey;

        private Vector2 gizmoSelectedPosition;
        private Vector2 gizmoDeselectedPosition;
        private Vector2 gizmoInitiallocationPosition;

        private void OnValidate()
        {
            recTransform = GetComponent<RectTransform>();

            animationDistance.x = Mathf.Max(0, animationDistance.x);
            animationDistance.y = Mathf.Max(0, animationDistance.y);

            initialPostion = transform.position;

            RecalculateGizmos();
        }

        private void Start()
        {
            initialPostion = transform.position;

            InitialOffsetPosition();


        }

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
                    if (animateButtonRoutine != null)
                        StopCoroutine(animateButtonRoutine);

                    animateButtonRoutine = StartCoroutine(AnimateButton(true));

                }

            }

        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (moveOnSelect)
            {
                if (animateButtonRoutine != null)
                {
                    StopCoroutine(animateButtonRoutine);
                }

                animateButtonRoutine = StartCoroutine(AnimateButton(false));
            }
        }

        public void ManuallySelectAnimate(bool selected)
        {
            if (animateButtonRoutine != null)
            {
                StopCoroutine(animateButtonRoutine);
            }

            if (selected)
            {
                animateButtonRoutine = StartCoroutine(AnimateButton(true));
            }
            else
            {
                animateButtonRoutine = StartCoroutine(AnimateButton(false));
            }
        }

        private void InitialOffsetPosition()
        {
            upOffset = new Vector2(0, +animationDistance.y);
            downOffset = new Vector2(0, -animationDistance.y);

            leftOffset = new Vector2(-animationDistance.x, 0);
            rightOffset = new Vector2(+animationDistance.x, 0);
        }

        private Vector2 GetOffset(AnimateToDirection dir)
        {
            switch (dir)
            {
                case AnimateToDirection.Top:
                    return upOffset;
                case AnimateToDirection.Bottom:
                    return downOffset;
                case AnimateToDirection.Left:
                    return leftOffset;
                case AnimateToDirection.Right:
                    return rightOffset;
                default:
                    return Vector2.zero;
            }
        }

        private IEnumerator AnimateButton(bool selected)
        {

            currentPostion = transform.position;
            float elapsedTime = 0;
            Vector2 targetPos = currentPostion;

            if (selected)
            {
                targetPos = currentPostion + GetOffset(SelectedDirection);
            }
            else
            {
                targetPos = currentPostion + GetOffset(DeslectDirection);
            }


            while (elapsedTime < animationDuration)
            {
                float evuationAtTime = easingCurve.Evaluate(elapsedTime / animationDuration);
                transform.position = Vector2.Lerp(currentPostion, targetPos, evuationAtTime);
                if (useScaledTime)
                {
                    elapsedTime += Time.deltaTime;
                }
                else
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }

                yield return null;

            }

            transform.position = targetPos;

            if (!selected)
            {
                transform.position = initialPostion;
            }

        }

        #region Visualization
        [ContextMenu("Refresh")]
        private void Refres()
        {
            OnValidate();
        }

        private void RecalculateGizmos()
        {
            InitialOffsetPosition();

            gizmoInitiallocationPosition = new Vector2(transform.position.x, transform.position.y) + recTransform.rect.center;
            gizmoSelectedPosition = gizmoInitiallocationPosition + GetOffset(SelectedDirection);
            gizmoDeselectedPosition = gizmoSelectedPosition + GetOffset(DeslectDirection);
        }

        private void OnDrawGizmos()
        {
            if (!displayGizmos)
                return;

            if (recTransform == null)
                return;

            Gizmos.color = gizmoInitiallocationColor;
            Gizmos.DrawWireCube(gizmoInitiallocationPosition, recTransform.sizeDelta);

            switch (gizmoHandler)
            {
                case DisplayGizmosAtLocation.Selected:
                    DrawCube(gizmoSelectedPosition, true);
                    break;
                case DisplayGizmosAtLocation.Deslected:
                    DrawCube(gizmoDeselectedPosition, false);
                    break;
                case DisplayGizmosAtLocation.Both:
                    DrawCube(gizmoSelectedPosition, true);
                    DrawCube(gizmoDeselectedPosition, false);
                    break;
            }

            DrawIndicators();
        }

        private void DrawCube(Vector2 pos, bool selected)
        {
            Gizmos.color = selected ? gizmoSelectedColor : gizmoDeselectedColor;
            Gizmos.DrawWireCube(pos, recTransform.sizeDelta);
        }

        private void DrawIndicators()
        {
            Gizmos.color = gizmoSelectedColor;
            Gizmos.DrawLine(gizmoInitiallocationPosition, gizmoSelectedPosition);


            Gizmos.color = gizmoDeselectedColor;
            Gizmos.DrawLine(gizmoInitiallocationPosition, gizmoDeselectedPosition);
        }

        #endregion

    }
}