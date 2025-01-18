using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class ForwardMovement : BaseProjectileMovement
    {
        [Header("Movement")]
        public float movementSpeed = 7f;

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
                //Debug.Log("Hit: " + hit.transform.name);
                //Debug.DrawLine(rayOutput, hit.point, Color.cyan, 3);

                float vfxTime = forwardWallDistance / (movementSpeed / 2);
                //Debug.Log("time, distance, speed: " + vfxTime + ", " + forwardWallDistance + ", " + ((cycleSpeed + frequencey) / 2));

                vfx.SetFloat("Duration", vfxTime);
                hitbox.DestroyTimer = vfxTime;
                StartCoroutine(CompleteTimer(vfxTime));
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

            if (DestroySelfOnComplete)
            {
                Destroy(gameObject);
            }

        }

        IEnumerator ExecuteMovement()
        {
            yield return new WaitForSeconds(delay);
            currentlyMoving = true;

            while (forwardWallDistance > stopDistance)
            {
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

                yield return null;
            }

        }

        IEnumerator CompleteTimer(float timer)
        {
            yield return new WaitForSeconds(timer);
            CompletedMovement();
        }

    }
}