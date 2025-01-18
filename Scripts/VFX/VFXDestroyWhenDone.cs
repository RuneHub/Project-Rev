using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace KS
{
    public class VFXDestroyWhenDone : MonoBehaviour
    {
        public float DestroyTimer;
        public VisualEffect vfx;

        private void Start()
        {
                Destroy(this.gameObject, DestroyTimer);
        }

    }
}