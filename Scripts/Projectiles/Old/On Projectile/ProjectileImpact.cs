using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class ProjectileImpact : ProjectileCollisionDetection
    {

        //Init, gets an kill action from the script it creates to call that function when its job is done.
        //also starts the coroutine to turn it off or kill it, if the projectile hasn't collided with anything
        //after said time.
        public override void Init(Action<ProjectileCollisionDetection> killAction)
        {
            _killAction = killAction;
            properties = GetComponent<ProjectileProperties>();
            StartCoroutine(KillAfterTime(lastingTime));
        }

        //checks if the projectile collides with anything,
        //if it does, it checks if it is a hurtbox and if its a target that it should apply damage to.
        //else it just calls the function for impacting.
        private void OnTriggerEnter(Collider col)
        {
            if (col.tag.Contains("Hurtbox") & !collided)
            {
                if (CheckTargetsTag(col.transform.root.gameObject.tag, attackingTargets))
                {
                    //Debug.Log("direct hit: " + col.transform.name + " of " + col.transform.root.gameObject.name);
                    if (col.transform.root.gameObject.GetComponent<CharacterStatsManager>() != null)
                    {
                        float colDefense = col.transform.root.gameObject.GetComponent<CharacterStatsManager>().baseDefense;
                        //Debug.Log("coldef: " + colDefense);
                        //float damage = StatCalculator.CalculateDamage(properties.attackValue, properties.CriticalHitRate, properties.CriticalHitBuff, colDefense);
                        //float damage = 10;
                        //Debug.Log("Damage: " + damage);
                        float hitAngle = Vector3.SignedAngle(transform.forward, col.transform.forward, Vector3.up);
                       // col.transform.root.gameObject.GetComponent<CharacterStatsManager>().TakeDamage(damage, hitAngle, GetComponent<ProjectileProperties>().getProperty());
                    }
                    else
                    {
                        Debug.LogError(col.transform.root.gameObject.name + " does not have a statManager attached");
                    }

                    collided = true;
                    DestroyOnImpact(col.ClosestPointOnBounds(transform.position));
                }
            }
            else if (!CheckTargetsTag(col.transform.root.gameObject.tag, ownerTypes))
            {
                DestroyOnImpact(transform.position);
            }

        }

        //creates a impact VFX effect and then calls the killaction function.
        private void DestroyOnImpact(Vector3 posImpactVFX)
        {
            if (impactFX != null)
            {
                ActivateImpactVFX(posImpactVFX, Quaternion.identity, 2);
            }
            collided = false;

            _killAction(this);
        }

        //the coroutine that after given time calls the kill action.
        IEnumerator KillAfterTime(float sec)
        {
            yield return new WaitForSeconds(sec);
            _killAction(this);
        }

    }
}