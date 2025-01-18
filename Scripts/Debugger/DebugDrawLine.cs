using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class DebugDrawLine : MonoBehaviour
    {
        Ray ray;
        RaycastHit hitInfo;

        public Transform outputTransform;

        // Update is called once per frame
        void Update()
        {
            ray.origin = outputTransform.position;
            ray.direction = outputTransform.forward;

            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);
            }

        }

    }
}
