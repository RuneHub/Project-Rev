using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KS
{
    public class UIHorizontalSelector : MonoBehaviour, IMoveHandler
    {
        [SerializeField] private EventSystem eventSystem;

        [Header("options")]
        public List<GameObject> options = new List<GameObject>();

        [SerializeField] private int currentIndex = 0;

        void Start()
        {

        }

        void Update()
        {

        }

        private void NextOption()
        {
            if (currentIndex < options.Count)
            {
                currentIndex++;
            }
        }

        private void PrevOption()
        {
            if (currentIndex > 0)
            {
                currentIndex--;
            }
        }

        public void OnMove(AxisEventData eventData)
        {
            MoveDirection moveDir = eventData.moveDir;
            if (moveDir.Equals(MoveDirection.Left))
            {
                PrevOption();
            }
            if (moveDir.Equals(MoveDirection.Right))
            {
                NextOption();
            }
        }

    }
}