using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    [RequireComponent(typeof(DamageColliderProperties))]
    public class UltimateAttackCollider : BaseDamageCollider
    {
        private PlayerManager player;

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

        public void InsertManager(PlayerManager _manager)
        {
            player = _manager;
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
                    if (col.transform.root.gameObject.GetComponent<CharacterManager>() != null)
                    {
                        if (!col.transform.root.gameObject.GetComponent<CharacterManager>().isInvulnerable ||
                            !col.transform.root.gameObject.GetComponent<CharacterManager>().isDead)
                        {

                            collided = true;
                            col.transform.root.gameObject.GetComponent<CharacterManager>().charStatManager.TakeUltimateHit();

                            var colPoint = col.ClosestPoint(transform.position);
                            CreateImpactVFX(colPoint);

                            if (player != null)
                            {
                                player.combatManager.UltimateAttackHitConfirmed();
                            }
                        }
                    }
                }
            }

        }

    }
}