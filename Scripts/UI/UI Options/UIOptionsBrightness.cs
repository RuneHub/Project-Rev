using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KS
{
    public class UIOptionsBrightness : BaseUIOptions
    {
        [Header("UI")]
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI sliderText;

        [Header("Light")]
        [SerializeField] private Light directionalLight;
        [SerializeField] private Light contrastLight;

        [Header("values")]
        [SerializeField] private float sliderMultiplier = .1f;
        [SerializeField] private float min, max, baseAmount;
        [SerializeField] private float shownValue;

        protected override void Start()
        {
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
            directionalLight.intensity = slider.value;
            contrastLight.intensity = slider.value;
            CalculateAmount();
        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }

        private void CalculateAmount()
        {
            float value = (((slider.value - min) / (max - min)) * shownValue);
            value = Mathf.Round(value);
            sliderText.text = value.ToString();
        }

    }
}