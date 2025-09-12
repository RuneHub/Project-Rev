using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KS
{
    public enum Invertinput { Vertical, Horizontal }
    public class UIOptionsInvertedControls : BaseUIOptions
    {
        public Invertinput input = Invertinput.Vertical;

        private PlayerManager playerManager;
        
        private void Awake()
        {
            playerManager = FindObjectOfType<PlayerManager>();
        }

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
            
            bool invert = false;
            if (currentIndex == 0)
                invert = false;
            else if (currentIndex == 1)
                invert = true;

            if (input == Invertinput.Vertical)
            {
                playerManager.inputs.invertedYCamera = invert;
            }
            else if (input == Invertinput.Horizontal)
            {
                playerManager.inputs.invertedXCamera = invert;
            }

        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }

    }
}