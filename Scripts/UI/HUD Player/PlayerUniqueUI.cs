using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KS
{
    public class PlayerUniqueUI : MonoBehaviour
    {
        PlayerManager manager;

        [SerializeField] CanvasGroup[] canvasGroup;

        [SerializeField] Image loadedGauge;

        [SerializeField, Range(0, 0.5f)] float animationTime = 0.25f;

        [SerializeField]  private float gaugeAmount;
        private float maxGaugeAmount = 100;
        private float minGaugeAmount = 0;
        private bool animating; 

        [SerializeField] private float visualGaugeAmount;

        private void Awake()
        {
            manager = FindObjectOfType<PlayerManager>();
        }

        private void Start()
        {
            if (loadedGauge == null)
            {
                Debug.LogError("Loaded Gauge not Found");
            }
        }

        //set's the maxGauge for the bar.
        public void SetHud(int maxGauge)
        {
            maxGaugeAmount = (float)maxGauge;
        }

        //update the visuals every frame
        public void UpdateGauge()
        {
            loadedGauge.fillAmount = visualGaugeAmount;
        }

        //resets the gauge and starts the coroutine to visualize it.
        public void ResetGauge()
        {
            gaugeAmount = 0;
            StartCoroutine(SmoothlyAnimateGauge(gaugeAmount));
        }

        //Updates the gauge to the correct amount, keeps it inbounds.
        //starts visualizing it through a courtine if it isn't already.
        public void UpdateGaugeAmount()
        {
            float amount = manager.uniqueMechManager.LoadedGaugeLevel;
            gaugeAmount = amount / maxGaugeAmount;

            if (gaugeAmount >= maxGaugeAmount)
            {
                gaugeAmount = maxGaugeAmount;
            }
            if (gaugeAmount <= minGaugeAmount)
            {
                gaugeAmount = minGaugeAmount;
            }

            if (!animating)
            {
                StartCoroutine(SmoothlyAnimateGauge(gaugeAmount));
            }

        }

        //corooutine to smoothly animate the gauge to the given amount
        private IEnumerator SmoothlyAnimateGauge(float targetFill)
        {
            float originalAmount = visualGaugeAmount;
            float elapsedTime = 0f;

            while (elapsedTime < animationTime)
            {
                animating = true;
                elapsedTime += Time.deltaTime;
                float time = elapsedTime / animationTime;
                visualGaugeAmount = Mathf.Lerp(visualGaugeAmount, targetFill, time);

                yield return null;
            }

            //this is temp, need to make it smootly alpha in & out depending on gauge amount
            if (visualGaugeAmount == 0)
            {
                foreach (var canvas in canvasGroup)
                {
                    canvas.alpha = 0f;
                }
            }
            else
            {
                foreach (var canvas in canvasGroup)
                {
                    canvas.alpha = 1f;
                }
            }

            animating = false;
            visualGaugeAmount = gaugeAmount;
        }

    }
}