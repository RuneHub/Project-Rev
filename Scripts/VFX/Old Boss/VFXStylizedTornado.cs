using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace KS
{
    public class VFXStylizedTornado : VFXBase
    {
        [SerializeField] private float coreDissolve;
        [SerializeField] private float layer1Dissolve;

        [SerializeField, ColorUsage(true, true)] private Color coreColor;
        [SerializeField, ColorUsage(true, true)] private Color layer1Color;


        public void SetCoreDissolve(float _dissolve)
        {
            coreDissolve = _dissolve;
        }

        public void SetLayer1Dissolve(float _dissolve)
        {
            layer1Dissolve = _dissolve;
        }

        public void SetCoreColor(Color color)
        {
            coreColor = color;
        }

        public void SetLayer1Color(Color color)
        {
            layer1Color = color;
        }

        protected override void Start()
        {
            base.Start();

            if (applyChanges)
            {
                vfx.SetFloat("TornadoCoreDissolve", coreDissolve);
                vfx.SetFloat("TornadoLayer1Dissolve", layer1Dissolve);
                vfx.SetVector4("TornadoCoreColor", coreColor);
                vfx.SetVector4("TornadoLayer1Color", layer1Color);
            }
        }

    }
}