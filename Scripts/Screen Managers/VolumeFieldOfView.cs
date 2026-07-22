using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace KS
{
    public class VolumeFieldOfView : MonoBehaviour
    {
        [SerializeField] private Volume GlobalVolume;
        [SerializeField] private Volume UltimateVolume;
        [SerializeField] private Vector2 FoV_FarNear;
        [SerializeField] private float FoV_Duration;
        [SerializeField] private AnimationCurve FoV_Curve;
        private DepthOfField DoF;

        private void Start()
        {
            UltimateVolume.profile.TryGet<DepthOfField>(out DoF);
        }

        public void VolumeFieldOfViewTurnOn()
        {
            DoF.active = true;
        }

        public void VolumeFieldOfViewTurnOff()
        {
            DoF.active = false;
        }

        [ContextMenu("Fovnear")]
        public void VolumeFieldOfViewFocusNear()
        {
            StartCoroutine(ChangeFoV(FoV_FarNear, FoV_Duration));
        }

        [ContextMenu("FovFar")]
        public void VolumeFieldOfViewFocusFar()
        {
            Vector2 reverse = new Vector2(FoV_FarNear.y, FoV_FarNear.x);
            StartCoroutine(ChangeFoV(reverse, FoV_Duration));
        }

        IEnumerator ChangeFoV(Vector2 baseToTarget, float duration)
        {
            float timeStamp = Time.time;
            while (Time.time < timeStamp + duration)
            {
                float t = (Time.time - timeStamp) / duration;
                t = FoV_Curve.Evaluate(t);
                DoF.focusDistance.value = Mathf.LerpUnclamped(baseToTarget.x, baseToTarget.y, t);
                yield return null;
            }
        }

    }
}