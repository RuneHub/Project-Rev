using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KS
{
    public class UIOptionsResolution : BaseUIOptions, IDeselectHandler
    {
        Resolution[] resolutions;
        List<Resolution> resolutionsList = new List<Resolution>();

        private Resolution selectedRes;

        protected override void Start()
        {
            resolutions = Screen.resolutions;
            string newRes;
            foreach (Resolution res in resolutions)
            {
                newRes = res.width.ToString() + " : " + res.height.ToString();
                if (!options.Contains(newRes))
                {
                    options.Add(newRes.ToString());
                    resolutionsList.Add(res);
                    
                }
            }

            currentIndex = Screen.resolutions.ToList().IndexOf(Screen.currentResolution);
            selectedRes = resolutionsList[currentIndex];
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
        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (selectedRes.ToString() != options[currentIndex].ToString())
            { //change resolution
                Screen.SetResolution(resolutionsList[currentIndex].width,
                    resolutionsList[currentIndex].height,
                    Screen.fullScreen);
            }

        }
    }
}