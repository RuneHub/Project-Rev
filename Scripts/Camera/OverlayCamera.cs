using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS {

    public class OverlayCamera : MonoBehaviour
    {
        [SerializeField] private Camera MainCamera;
        [SerializeField] private Camera OverlayCam;
        public bool OnUpdate = false;

        private void Awake()
        {
            OverlayCam = GetComponent<Camera>();

            if (MainCamera != null)
            {
                OverlayCam.orthographic = MainCamera.orthographic;
                OverlayCam.fieldOfView = MainCamera.fieldOfView;
            }

        }

        public void SetOverLayFoV(Camera cam)
        {
            OverlayCam.orthographic = cam.orthographic;
            OverlayCam.fieldOfView = cam.fieldOfView;
        }

        private void Update()
        {
            if (MainCamera != null && OnUpdate)
            {
                OverlayCam.orthographic = MainCamera.orthographic;
                OverlayCam.fieldOfView = MainCamera.fieldOfView;
            }
        }

    }
}