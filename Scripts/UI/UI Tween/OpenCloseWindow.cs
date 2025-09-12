using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace KS
{
    public class OpenCloseWindow : MonoBehaviour
    {
        //https://www.youtube.com/watch?v=AaudFyM3KV0&t=301s
        //tutorial vid

        [Header("Window Setup")]
        [SerializeField] private GameObject window;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private CanvasGroup canvasGroup;

        public enum AnimateToDirection
        {
            Top,
            Bottom,
            Left,
            Right
        }

        [Header("Animaton Setup")]
        [SerializeField] private AnimateToDirection openDirection = AnimateToDirection.Top;
        [SerializeField] private AnimateToDirection CloseDirection = AnimateToDirection.Bottom;
        [Space]
        [SerializeField] private Vector2 distanceToAnimate = new Vector2(100, 100);
        [SerializeField] private AnimationCurve easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [Range(0, 1)] [SerializeField] private float animationDuration = 0.5f;

        private bool isOpen;
        private Vector2 initialPostion;
        private Vector2 currentPosition;

        private Vector2 upOffset;
        private Vector2 downOffset;
        private Vector2 leftOffset;
        private Vector2 rightOffset;

        private Coroutine animateWindowCoroutine;

        [Header("Helpers")]
        [SerializeField] private bool displayGizmos = true;

        public static event Action OnOpenWindow;
        public static event Action OnCloseWindow;

        private enum DisplayGizmosAtLocation
        {
            Open,
            Close,
            Both,
            Situational
        }

        [SerializeField, DrawIf("displayGizmos", true)] private DisplayGizmosAtLocation gizmoHandler;
        [SerializeField, DrawIf("displayGizmos", true)] private Color gizmoOpenColor = Color.green;
        [SerializeField, DrawIf("displayGizmos", true)] private Color gizmoCloseColor = Color.red;
        [SerializeField, DrawIf("displayGizmos", true)] private Color gizmoInitialLocationColor = Color.grey;
        private Vector2 GizmoOpenPosition;
        private Vector2 GizmoClosePosition;
        private Vector2 GizmoInitialPosition;
        
        private void OnValidate()
        {
            if (window != null)
            {
                rectTransform = window.GetComponent<RectTransform>();
                canvasGroup = window.GetComponent<CanvasGroup>();
            }

            distanceToAnimate.x = Mathf.Max(0, distanceToAnimate.x);
            distanceToAnimate.y = Mathf.Max(0, distanceToAnimate.y);

            initialPostion = window.transform.position;

            RecalculateGizmoPositions();

        }

        #region Animation Functionality
        private void Start()
        {
            initialPostion = window.transform.position;

            InitializeOffsetPosition();
        }

        private void InitializeOffsetPosition()
        {
            upOffset = new Vector2(0, distanceToAnimate.y);
            downOffset = new Vector2(0, -distanceToAnimate.y);

            leftOffset = new Vector2(-distanceToAnimate.x, 0);
            rightOffset = new Vector2(+distanceToAnimate.x, 0);
        }

        [ContextMenu("Toggle Open Close")]
        public void ToggleOpenClose()
        {
            if (isOpen)
                CloseWindow();
            else
                OpenWindow();
        }

        [ContextMenu("Open Window")]
        public void OpenWindow()
        {
            if (isOpen)
                return;

            isOpen = true;
            OnOpenWindow?.Invoke();

            if (animateWindowCoroutine != null)
                StopCoroutine(animateWindowCoroutine);

            animateWindowCoroutine = StartCoroutine(AnimateWindow(true));
            
        }

        [ContextMenu("Close Window")]
        public void CloseWindow()
        {
            if(!isOpen) 
                return;

            isOpen = false;
            OnCloseWindow?.Invoke();

            if(animateWindowCoroutine != null)
                StopCoroutine(animateWindowCoroutine);

            animateWindowCoroutine = StartCoroutine(AnimateWindow(false));

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

        private IEnumerator AnimateWindow(bool open)
        {
            if (open)
                window.gameObject.SetActive(true);

            currentPosition = window.transform.position;
            float elapsedTime = 0;
            Vector2 targetPos = currentPosition;

            if (open)
                targetPos = currentPosition + GetOffset(openDirection);
            else
                targetPos = currentPosition + GetOffset(CloseDirection);

            while (elapsedTime < animationDuration)
            {
                float evaluationAtTime = easingCurve.Evaluate(elapsedTime / animationDuration);
                window.transform.position = Vector2.Lerp(currentPosition, targetPos, evaluationAtTime);
                canvasGroup.alpha = open 
                    ? Mathf.Lerp(0, 1, evaluationAtTime)
                    : Mathf.Lerp(1, 0, evaluationAtTime);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            window.transform.position = targetPos;

            canvasGroup.alpha = open ? 1 : 0;
            canvasGroup.interactable = open;
            canvasGroup.blocksRaycasts = open;

            if (!open)
            {
                window.gameObject.SetActive(false);
                window.transform.position = initialPostion;
            }

        }

        #endregion

        #region Visualization
        [ContextMenu("Refresh")]
        private void Refres()
        {
            OnValidate();
        }

        private void RecalculateGizmoPositions()
        {
            InitializeOffsetPosition();

            GizmoInitialPosition = new Vector2(window.transform.position.x, window.transform.position.y) + rectTransform.rect.center;
            GizmoOpenPosition = GizmoInitialPosition + GetOffset(openDirection);
            GizmoClosePosition = GizmoOpenPosition + GetOffset(CloseDirection);
        }

        private void OnDrawGizmos()
        {
            if (!displayGizmos)
                return;

            if (window == null)
                return;

            if (rectTransform == null)
                return;

            Gizmos.color = gizmoInitialLocationColor;
            Gizmos.DrawWireCube(GizmoInitialPosition, rectTransform.sizeDelta);

            switch (gizmoHandler)
            {
                case DisplayGizmosAtLocation.Open:
                    DrawCube(GizmoOpenPosition, true);
                    break;
                case DisplayGizmosAtLocation.Close:
                    DrawCube(GizmoClosePosition, false);
                    break;
                case DisplayGizmosAtLocation.Both:
                    DrawCube(GizmoOpenPosition, true);
                    DrawCube(GizmoClosePosition, false);
                    break;
                case DisplayGizmosAtLocation.Situational:
                    if (isOpen)
                        DrawCube(GizmoClosePosition, true);
                    else
                        DrawCube(GizmoOpenPosition, false);
                        break;
                default:
                    break;
            }

            DrawIndicators();
        }

        private void DrawCube(Vector2 pos, bool open)
        {
            Gizmos.color = open ? gizmoOpenColor : gizmoCloseColor;
            Gizmos.DrawWireCube(pos, rectTransform.sizeDelta);
        }

        private void DrawIndicators()
        {
            Gizmos.color = gizmoOpenColor;
            Gizmos.DrawLine(GizmoInitialPosition, GizmoOpenPosition);

            Gizmos.color = gizmoCloseColor;
            Gizmos.DrawLine(GizmoOpenPosition, GizmoClosePosition);
        }

        #endregion

    }
}