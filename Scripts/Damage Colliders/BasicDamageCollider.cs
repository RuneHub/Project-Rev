using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KS
{
    [RequireComponent(typeof(DamageColliderProperties))]
    public class BasicDamageCollider : BaseDamageCollider
    {
        //the basic Damage Collider is... basic, it just checks if there was a collision
        // and do damage if it is the correct type and destroy's itself afterwards
        //or it get destroyed after the timer runs out.

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
            if (col.tag.Contains("Hurtbox") & !collided)
            {
                //check if the collided object is the same as a target type
                if (TargetTag(col.transform.root.gameObject.tag, Targets))
                {
                    //check if collided object has a stat manager
                    if (col.transform.root.gameObject.GetComponent<CharacterStatsManager>() != null)
                    {
                        collided = true;
                        //calculate hit angle
                        float hitAngle = Vector3.SignedAngle(col.transform.root.forward, transform.forward, Vector3.up);

                        // send data to function for damage
                        HitTarget(col.transform.root.gameObject.GetComponent<CharacterManager>(), hitAngle);
                        
                        if (properties.damageProperties != DamageProperties.Normal)
                        {
                            Vector3 dir = col.transform.position - transform.position;
                            //EngageProperty(col, dir, properties.damageProperties, false);
                            StartCoroutine(HitStopDelay(.3f, col, dir));
                        }

                    }
                    else
                    {
                        //log error
                        Debug.LogError(col.transform.root.gameObject.name + " does not have a statManager attached");
                    }

                    //Debug.Log("hit: " + col.gameObject.name);

                    collided = true;
                    if (DestroyAfterImpact)
                    {
                        DestroyOnImpact();
                    }

                }
            }
            else if (!TargetTag(col.transform.root.gameObject.tag, Owners) &&
                !TargetTag(col.transform.root.gameObject.tag, Targets))
            {
                if (DestroyAfterImpact)
                {
                    DestroyOnImpact();
                }
            }
        }

        //calculate the damage and send it over
        private void HitTarget(CharacterManager target, float hitAngle)
        {
            var (damage, isCrit) = StatCalculator.CalculateDamage(_owner, colliderAtkPwr, target);
            target.charStatManager.TakeDamage(damage, isCrit, _owner.charStatManager.HUDDisplayColor, hitAngle, properties.getProperty(_owner));
        }

        //Destroy's this collider
        private void DestroyOnImpact()
        {
            _killAction(this);
        }

        private IEnumerator HitStopDelay(float delay, Collider col, Vector3 dir)
        {
            yield return new WaitForSeconds(delay);
            EngageProperty(col, dir, properties.damageProperties, false);
        }

    }
}