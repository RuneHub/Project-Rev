using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KS
{
    public class CameraEffectManager : MonoBehaviour
    {

        CameraManager manager;

        private Vector3 _originalPos = Vector3.zero;
        private Transform cameraTransform;

        private void Awake()
        {
            manager = GetComponent<CameraManager>();
            cameraTransform = manager.cameraTransform;
        }

        void Start()
        {

        }


        void Update()
        {

        }

        //shake the camera for the given amount of time(duration) with the given amount of power(Magnitude)
        public IEnumerator Shake(float duration, float magnitude)
        {
            Vector3 OriginalPos = cameraTransform.transform.localPosition;

            float elapsed = 0f;

            while (elapsed < duration)
            {
                //Debug.Log("magnitude: " + magnitude);
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                cameraTransform.transform.localPosition = new Vector3(x, y, OriginalPos.z);
                elapsed += Time.deltaTime;

                yield return null;

            }

            cameraTransform.transform.localPosition = OriginalPos;

        }

    }
}