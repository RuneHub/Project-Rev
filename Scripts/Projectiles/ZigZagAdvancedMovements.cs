using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

namespace KS
{
    public class ZigZagAdvancedMovements : BaseProjectileMovement
    {
        
        [Header("Raycast")]
        [SerializeField] private float forwardWallDistance;
        [SerializeField] private float rightWallDistance;
        [SerializeField] private float LeftWallDistance;

        [SerializeField] private float shootHeight;
        [SerializeField] private Vector3 rayOutput;

        [Header("Sine Wave variables")]
        [Tooltip("speed of the zigzag movement")]
        [SerializeField] private float frequencey = 1f; //can also be seen as zigzag speed.
        [Tooltip("Width of the movement")]
        [SerializeField] private float amplitude;  //can also be seen as how wide the movement is.
        [Tooltip("speed of each cycle")]
        [SerializeField] private float cycleSpeed = 1f; // how fast it goes through a cycle.

        private Vector3 pos;
        private Vector3 axis;

        protected override void Start()
        {
            base.Start();

            pos = transform.position;
            axis = transform.right;


            Initialize();
        }

        protected override void Update()
        {
            rayOutput = new Vector3(transform.position.x,
                                  transform.position.y + shootHeight,
                                  transform.position.z);

            RaycastHit hit;
            if (Physics.Raycast(rayOutput, transform.forward, out hit, rangeDistance, hitLayer))
            {
                forwardWallDistance = Vector3.Distance(rayOutput, hit.point);
                //Debug.DrawLine(rayOutput, hit.point, Color.cyan, 3);
            }

            base.Update();
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

                float vfxTime = forwardWallDistance / ((cycleSpeed + frequencey) / 2);
                //Debug.Log("time, distance, speed: " + vfxTime + ", " + forwardWallDistance + ", " + ((cycleSpeed + frequencey) / 2));

                vfx.SetFloat("Duration", vfxTime);
                hitbox.DestroyTimer = vfxTime;
            }
            
            if (Physics.Raycast(rayOutput, transform.right, out hit, rangeDistance, hitLayer))
            {
                rightWallDistance = Vector3.Distance(rayOutput, hit.point);
                //Debug.DrawLine(rayOutput, hit.point, Color.cyan, 3);
            }

            if (Physics.Raycast(rayOutput, -transform.right, out hit, rangeDistance, hitLayer))
            {
                LeftWallDistance = Vector3.Distance(rayOutput, hit.point);
                //Debug.DrawLine(rayOutput, hit.point, Color.cyan, 3);
            }

            amplitude = ((LeftWallDistance+rightWallDistance)/2) - size;

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

        private void ZigZagMovement()
        {
            pos += transform.forward * Time.deltaTime * cycleSpeed;
            transform.position = pos + axis * Mathf.Sin(Time.time * frequencey) * amplitude;
            
        }

        IEnumerator ExecuteMovement()
        {
            yield return new WaitForSeconds(delay);
            currentlyMoving = true;

            while (forwardWallDistance > stopDistance)
            {
                ZigZagMovement();

                yield return null;
            }

            CompletedMovement();

        }

    }
}