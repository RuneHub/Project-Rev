using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    [RequireComponent(typeof(DamageColliderProperties))]
    public class MeleeDamageCollider : BaseDamageCollider
    {
        public override void Init(Action<BaseDamageCollider> killAction, CharacterManager owner, float atkPwr)
        {
            _owner = owner;
            _killAction = killAction;
            colliderAtkPwr = atkPwr;
            properties = GetComponent<DamageColliderProperties>();
            ResetBeforeUse();
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

                        var colPoint = col.ClosestPoint(transform.position);
                        CreateImpactVFX(colPoint);
                    }
                    else
                    {
                        //log error
                        Debug.LogError(col.transform.root.gameObject.name + " does not have a statManager attached");
                    }

                    collided = true;
                }
            }
        }

        //resets variables for multiple use
        public void ResetBeforeUse()
        {
            collided = false;
        }

        //calculate the damage and send it over
        private void HitTarget(CharacterManager target, float hitAngle)
        {
            var (damage, isCrit) = StatCalculator.CalculateDamage(_owner, colliderAtkPwr, target);
            target.charStatManager.TakeDamage(damage, isCrit, _owner.charStatManager.HUDDisplayColor, hitAngle, properties.getProperty(_owner));
        }

        private IEnumerator HitStopDelay(float delay, Collider col, Vector3 dir)
        {
            yield return new WaitForSeconds(delay);
            EngageProperty(col, dir, properties.damageProperties, false);
        }

    }
}