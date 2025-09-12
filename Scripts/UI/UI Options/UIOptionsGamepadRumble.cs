using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KS
{
    public class UIOptionsGamepadRumble : BaseUIOptions
    {
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

            if (currentIndex == 0)
            {
                playerManager.GamePadRumble = false;
            }
            else if (currentIndex == 1)
            {

                playerManager.GamePadRumble = true;
                //let it rumble for a bit when set to true.
                playerManager.inputs.GamepadRumble();
            }

        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }

    }
}