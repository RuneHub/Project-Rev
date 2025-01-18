using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    [RequireComponent(typeof(DamageColliderProperties))]
    public class ForceDamageCollider : BaseDamageCollider
    {

        public bool useCenter;
        public Vector3 forceDirection;

        public override void Init(Action<BaseDamageCollider> killAction, CharacterManager owner, float atkPwr)
        {
            _owner = owner;
            _killAction = killAction;
            colliderAtkPwr = atkPwr;
            properties = GetComponent<DamageColliderProperties>();
            if (DestroyWithTime)
            {
                StartCoroutine(KillAfterTime(DestroyTimer));
            }
        }

        protected override void OnTriggerEnter(Collider col)
        {
            //if it is a hurtbox & this collider hasn't collided yet.
            if (col.tag.Contains("Hurtbox"))
            {
                //check if the collided object is the same as a target type
                if (TargetTag(col.transform.root.gameObject.tag, Targets))
                {
                    //check if collided object has a stat manager
                    if (col.transform.root.gameObject.GetComponent<CharacterStatsManager>() != null)
                    {

                        if (useCenter)
                        {
                            Vector3 dir = col.transform.position - transform.position;
                            EngageProperty(col, dir, properties.damageProperties, useCenter);
                        }
                        else
                        {
                            EngageProperty(col, forceDirection, properties.damageProperties, useCenter);
                        }

                        
                    }
                    else
                    {
                        //log error
                        Debug.LogError(col.transform.root.gameObject.name + " does not have a statManager attached");
                    }

                    Debug.Log("hit: " + col.gameObject.name);


                }
            }
           
        }


        private void OnTriggerExit(Collider col)
        {
            if (col.tag.Contains("Hurtbox"))
            {
                //check if the collided object is the same as a target type
                if (TargetTag(col.transform.root.gameObject.tag, Targets))
                {
                    //check if collided object has a stat manager
                    if (col.transform.root.gameObject.GetComponent<CharacterStatsManager>() != null)
                    {
                        properties.DeactiveForce();
                    }
                }
            }

        }

    }
}