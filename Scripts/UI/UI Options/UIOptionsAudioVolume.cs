using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using TMPro;

namespace KS
{
    public class UIOptionsAudioVolume : BaseUIOptions
    {
        [Header("mixer")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private string targetAudio;

        [Header("UI")]
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI sliderText;

        [Header("Volume")]
        [SerializeField] private float sliderMultiplier = .01f;
        [SerializeField] private float minValue = 0.0001f;
        [SerializeField] private float maxValue = 1f;
        [SerializeField] private float baseValue = 1f;


        protected override void Start()
        {
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.value = baseValue;

            SetOption();

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
            audioMixer.SetFloat(targetAudio, GetVolumeValue());
            CalculateAmount();

        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }

        public float GetVolumeValue()
        {
            float value = Mathf.Log10(slider.value) * 20f;
            return value;
        }

        private void CalculateAmount()
        {
            float value = (((slider.value - minValue) / (maxValue - minValue)) * 100);
            value = Mathf.Round(value);
            sliderText.text = value.ToString();
        }

    }
}