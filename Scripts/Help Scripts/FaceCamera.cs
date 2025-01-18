using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KS
{
    public class FaceCamera : MonoBehaviour
    {
        public bool alwaysFaceCam = true;
        [SerializeField] CameraManager cam;

        void Start()
        {
            cam = CameraManager.singleton;
        }

        // Update is called once per frame
        void Update()
        {
            if (alwaysFaceCam)
            {
                Vector3 targetPos = new Vector3(cam.transform.position.x,
                                                transform.position.y, 
                                                cam.transform.position.z);
                transform.LookAt(targetPos);
            }
        }
    }
}