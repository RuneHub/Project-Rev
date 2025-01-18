using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace KS
{
    public class BaseProjectileMovement : MonoBehaviour
    {
        public float size;
        public bool useSizeAsStopDistance;
        public LayerMask hitLayer;
        public float rangeDistance = 25f;
        public float stopDistance;
        public bool DestroySelfOnComplete;

        public float delay;

        public VisualEffect vfx;
        public BaseDamageCollider hitbox;

        public bool currentlyMoving;
        public bool movementComplete;



        protected virtual void Start()
        {

        }


        protected virtual void Update()
        {

        }

        protected virtual void Initialize()
        {
            
        }

        protected virtual void StopMoving()
        {
            currentlyMoving = false;
        }

        protected virtual void CompletedMovement()
        {
            currentlyMoving = false;
            movementComplete = true;
        }

    }
}