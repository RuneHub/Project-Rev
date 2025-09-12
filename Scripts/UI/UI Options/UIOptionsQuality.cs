using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KS
{
    public class UIOptionsQuality : BaseUIOptions, ISubmitHandler
    {
        [SerializeField] private GameObject customs;
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private Selectable ToSelect;

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void NextOption()
        {
            base.NextOption();
        }

        protected override void PrevOption()
        {
            base.PrevOption();
        }

        protected override void SetOption()
        {
            base.SetOption();
            switch (currentIndex)
            {
                case 0:
                    QualitySettings.SetQualityLevel(3, false); //custom Quality
                    break;
                case 1:
                    QualitySettings.SetQualityLevel(0,false); //Lowest, Medium Quality.
                    break;
                case 2:
                    QualitySettings.SetQualityLevel(1, false); //High, High Quality.   
                    break;
                case 3:
                    QualitySettings.SetQualityLevel(2, false); //Ultra, Ultra Quality.
                    break;
                
            }

        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (currentIndex == 0)
            {
                customs.gameObject.SetActive(true);
                eventSystem.SetSelectedGameObject(ToSelect.gameObject);
                Debug.Log("opening Custom options");
            }
        }
    }
}