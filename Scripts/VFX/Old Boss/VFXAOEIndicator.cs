using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace KS
{
    public class VFXAOEIndicator : VFXBase
    {

        [ColorUsage(true, true)] public Color color;

        public void SetColor(Color _color)
        {
            color = _color;
        }

        

        protected override void Start()
        {
            base.Start();

            if (applyChanges)
            {
                vfx.SetVector4("Color", color);
            }
        }

    }

}