using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace KS
{
    public class BossAnimationEvents : MonoBehaviour
    {
        [HideInInspector]
        public BossManager manager;

        public event EventHandler OnShootEventTriggered;

        private void Awake()
        {
            manager = GetComponentInParent<BossManager>();
        }

        public void TriggerShootEvent()
        {
            OnShootEventTriggered?.Invoke(this, EventArgs.Empty);
        }
    }
}