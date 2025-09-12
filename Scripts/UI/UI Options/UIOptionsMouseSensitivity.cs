using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KS
{
    public class UIOptionsMouseSensitivity : BaseUIOptions
    {

        private CameraManager camManager;

        [Header("UI")]
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI sliderText;

        [Header("Sensitivity")]
        [SerializeField] private float sliderMultiplier = .1f;
        [SerializeField] private float min, max, baseAmount;

        protected override void Start()
        {
            camManager = CameraManager.singleton;

            baseAmount = camManager.cameraLookSpeed;

            slider.maxValue = max;
            slider.minValue = min;
            slider.value = baseAmount;
            CalculateAmount();

            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void NextOption()
        {
            slider.value += sliderMultiplier;
            SetOption();
        }

        protected override void PrevOption()
        {
            slider.value -= sliderMultiplier;
            SetOption();
        }

        protected override void SetOption()
        {
            camManager.cameraLookSpeed = slider.value;
            CalculateAmount();
        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }

        private void CalculateAmount()
        {
            float value = (((slider.value - min) / (max - min)) * 10);
            value = Mathf.Round(value);
            sliderText.text = value.ToString();
        }
    }
}