using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class ProjectileLasting : ProjectileCollisionDetection
    {
        public override void Init(Action<ProjectileCollisionDetection> killAction)
        {
            _killAction = killAction;
            properties = GetComponent<ProjectileProperties>();
            StartCoroutine(KillAfterTime(lastingTime));
        }


        private void OnTriggerEnter(Collider col)
        {
            if (col.tag.Contains("Hurtbox") && !collided)
            {
                if (CheckTargetsTag(col.transform.root.gameObject.tag, attackingTargets))
                {
                    //Debug.Log("direct hit: " + col.transform.name + " of " + col.transform.root.gameObject.name);
                    if (col.transform.root.gameObject.GetComponent<CharacterStatsManager>() != null && !collided)
                    {
                        float colDefense = col.transform.root.gameObject.GetComponent<CharacterStatsManager>().baseDefense;
                        //Debug.Log("coldef: " + colDefense);
                        //float damage = StatCalculator.CalculateDamage(properties.attackValue, properties.CriticalHitRate, properties.CriticalHitBuff, colDefense);
                        //float damage = StatCalculator.CalculateDamage(properties.attackValue, properties.CriticalHitRate, properties.CriticalHitBuff, colDefense);
                        //float damage = 10;
                        if (properties.damageProperties != DamageProperties.Normal)
                        {
                            EngageProperty(col, GetComponent<Rigidbody>().velocity, properties.damageProperties);
                        }

                        //Debug.Log("Damage: " + damage);
                        float hitAngle = Vector3.SignedAngle(transform.forward, col.transform.forward, Vector3.up);
                       // col.transform.root.gameObject.GetComponent<CharacterStatsManager>().TakeDamage(damage, hitAngle, GetComponent<ProjectileProperties>().getProperty());
                    }

                    collided = true;
                    ActivateImpactVFX(col.ClosestPointOnBounds(transform.position), Quaternion.identity, 2);

                }
            }
        }

        IEnumerator KillAfterTime(float sec)
        {
            yield return new WaitForSeconds(sec);
            _killAction(this);
        }
    }
}