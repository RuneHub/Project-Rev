using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace KS
{
    public class ZigZagMovements : BaseProjectileMovement
    {
        [Header("movement variables")]
        public float totalMoveTime;

        [Header("Sine Wave variables")]
        [Tooltip("speed of the zigzag movement")]
        [SerializeField] private float frequencey = 1f; //can also be seen as zigzag speed.
        [Tooltip("Width of the movement")]
        [SerializeField] private float amplitude = 5f;  //can also be seen as how wide the movement is.
        [Tooltip("speed of each cycle")]
        [SerializeField] private float cycleSpeed = 1f; // how fast it goes through a cycle.

        private Vector3 pos;
        private Vector3 axis;

        //moving in a zigzag pattern through using Sine Wave.
        //used this as base: https://medium.com/nerd-for-tech/zig-zag-movement-in-unity-3c2762b1be61

        protected override void Start()
        {
            base.Start();

            pos = transform.position;
            axis = transform.right;

            Initialize();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void Initialize()
        {
            base.Initialize();

            StartCoroutine(performMovement());
        }

        protected override void StopMoving()
        {
            base.StopMoving();
            StopCoroutine(performMovement());
        }

        protected override void CompletedMovement()
        {
            base.CompletedMovement();

        }

        //the Sine Wave movement calculation that makes it zig zag.
        private void ZigZagMovement()
        {
            pos += transform.forward * Time.deltaTime * cycleSpeed;
            transform.position = pos + axis * Mathf.Sin(Time.time * frequencey) * amplitude;
            Debug.Log(pos + axis * Mathf.Sin(Time.time * frequencey) * amplitude);
        }


        //IEnumerator that calls the movement whilst keeping the time
        IEnumerator performMovement()
        {
            float movementTime = 0f;
            currentlyMoving = true;


            while (movementTime < totalMoveTime)
            {
                ZigZagMovement();
                movementTime += Time.deltaTime;

                yield return null;
            }

            CompletedMovement();

        }

    }
}