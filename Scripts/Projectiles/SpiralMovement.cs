using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class SpiralMovement : BaseProjectileMovement
    {
        [Header("Movement")]
        public Vector3 Radius = new Vector3(.5f, 0.5f);
        public Vector3 LinearVelocity = new Vector3(.5f, 0f, 0.5f);
        public float AngularVelocity = 5f;
        public float TimeScale = 2f;

        public float tsDecrementer = 0.3f;

        private float timer;

        [Header("Raycast")]
        [SerializeField] private float forwardWallDistance;
        [SerializeField] private float shootHeight;
        [SerializeField] private Vector3 rayOutput;

        protected override void Start()
        {
            base.Start();

            Initialize();
        }
       
        protected override void Update()
        {
            base.Update();

            rayOutput = new Vector3(transform.position.x,
                                  transform.position.y + shootHeight,
                                  transform.position.z);

            RaycastHit hit;
            if (Physics.Raycast(rayOutput, transform.forward, out hit, rangeDistance, hitLayer))
            {
                forwardWallDistance = Vector3.Distance(rayOutput, hit.point);
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            if (useSizeAsStopDistance)
            {
                stopDistance = size;
            }

            rayOutput = new Vector3(transform.position.x,
                                  transform.position.y + shootHeight,
                                  transform.position.z);

            RaycastHit hit;
            if (Physics.Raycast(rayOutput, transform.forward, out hit, Mathf.Infinity, hitLayer))
            {
                forwardWallDistance = Vector3.Distance(rayOutput, hit.point);
                float vfxTime = forwardWallDistance / ((AngularVelocity + TimeScale) / 2);

                // vfx.SetFloat("Duration", vfxTime);
                // hitbox.DestroyTimer = vfxTime;
            }

            StartCoroutine(ExecuteMovement());
        }

        protected override void StopMoving()
        {
            base.StopMoving();
        }

        protected override void CompletedMovement()
        {
            base.CompletedMovement();
        }

        IEnumerator ExecuteMovement()
        {
            yield return new WaitForSeconds(delay);
            currentlyMoving = true;

            while (forwardWallDistance > stopDistance)
            {
                var angle = AngularVelocity * timer;
                var position = LinearVelocity * timer;

                position += new Vector3(Mathf.Cos(angle) * Radius.x, 0, Mathf.Sin(angle) * Radius.z);
                transform.position += position;

                timer += TimeScale * Time.deltaTime;

                yield return null;
            }
        }

    }
}