using Unity.VisualScripting;
using UnityEngine;

namespace KSTween 
{
    public static class LocalScaleTween
    {
        public static Tween<Vector3, Transform> TweenLocalScale(this Component self, Vector3 to, float duration) =>
            Tween<Vector3, Transform>.Add<Driver>(self).Finalize(to, duration);

        private class Driver : Tween<Vector3, Transform>
        {
            public override void OnInit()
            {

            }

            public override void OnUpdate()
            {

                component.localScale = Vector3.Lerp(component.localScale, valueTo, getElapsedTime());

            }

        }
    }
}