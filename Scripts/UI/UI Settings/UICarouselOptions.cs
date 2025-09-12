using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace KS
{
    public class UICarouselOptions : MonoBehaviour, IMoveHandler
    {
        [SerializeField] private EventSystem eventSystem;

        [Header("options")]
        [SerializeField] private GameObject optionParent;
        public List<GameObject> options = new List<GameObject>();

        [SerializeField] private int currentIndex = 0;

        private void Start()
        {
            for (int c = 0; c < optionParent.transform.childCount; c++) 
            {
                options.Add(transform.GetChild(c).gameObject);
            }

        }

        private void Update()
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