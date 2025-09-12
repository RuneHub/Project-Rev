using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KS
{
    public class BaseUIOptions : MonoBehaviour, IMoveHandler
    {
        [SerializeField] private bool useBase = true;
        [SerializeField] protected int currentIndex = 0;

        [Header("Options")]
        public List<string> options = new List<string>();

        [Header("visuals")]
        [SerializeField] private TextMeshProUGUI UIText;
        [SerializeField] private Image RightArrow;
        [SerializeField] private Image LeftArrow;

        protected virtual void Start()
        {
            if (useBase)
            {
                UIText.text = options[currentIndex];
                CheckIndex();
            }
        }

        protected virtual void Update()
        {

        }

        protected virtual void NextOption()
        {
            if (currentIndex < options.Count)
            {
                currentIndex++;
                CheckIndex();
                SetOption();
            }
        }

        protected virtual void PrevOption()
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                CheckIndex();
                SetOption();
            }
        }

        protected virtual void SetOption()
        {
            if (useBase)
            {
                UIText.text = options[currentIndex].ToString();
            }
        }

        private void CheckIndex()
        {
            if (currentIndex >= options.Count - 1)
            {
                currentIndex = options.Count - 1;
                RightArrow.gameObject.SetActive(false);
                LeftArrow.gameObject.SetActive(true);
            }
            else if (currentIndex <= 0)
            {
                currentIndex = 0;
                LeftArrow.gameObject.SetActive(false);
                RightArrow.gameObject.SetActive(true);
            }
            else if (currentIndex == 0 && options.Count == 1)
            {
                RightArrow.gameObject.SetActive(false);
                LeftArrow.gameObject.SetActive(false);
            }
            else
            {
                RightArrow.gameObject.SetActive(true);
                LeftArrow.gameObject.SetActive(true);
            }
        }

        public virtual void OnMove(AxisEventData eventData)
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
           
            if (useBase)
            {
                CheckIndex();
            }
        }
    }
}
