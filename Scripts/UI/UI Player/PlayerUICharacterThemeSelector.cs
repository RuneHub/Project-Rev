using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KS
{
    public class PlayerUICharacterThemeSelector : MonoBehaviour, IMoveHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private EventSystem eventSystem;

        [SerializeField] private PlayerUICharacterWindowManager UICharacterWindow;

        private void Start()
        {

        }

        private void Next()
        {
            UICharacterWindow.CycleRight();
        }

        private void Prev()
        {
            UICharacterWindow.CycleLeft();
        }

        public void OnMove(AxisEventData eventData)
        {
            MoveDirection moveDir = eventData.moveDir;
            if (moveDir.Equals(MoveDirection.Left))
            {
                Prev();
            }
            if (moveDir.Equals(MoveDirection.Right))
            {
                Next();
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            
        }

        public void OnDeselect(BaseEventData eventData)
        {
        }
    }
}