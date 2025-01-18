using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace KS
{
    public class ProjectileHoming : MonoBehaviour
    {
        [SerializeField] private ProjectileCollisionDetection colDetect;

        public Transform target;

        public float homingSpeed = 50f;
        public float homingRotation = 5;

        [SerializeField] private Rigidbody rb;

        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        }

        private void FixedUpdate()
        {
            if (colDetect != null && target != null)
            {
                Vector3 dir = target.position - rb.position;
                dir.Normalize();

                float rotateAmount = Vector3.Cross(dir, transform.forward).z;

                rb.angularVelocity = -Vector3.Cross(dir, transform.forward) * homingRotation;

                rb.velocity = transform.forward * homingSpeed;
            }

        }

    }
}