using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public abstract class BaseDamageCollider : MonoBehaviour
    {

        public TargetTypes Owners;
        public TargetTypes Targets;

        public bool DestroyAfterImpact = false;
        public bool DestroyWithTime = true;
        public float DestroyTimer = 15f;

      //  [SerializeField] protected 
        public float colliderAtkPwr;

        [SerializeField] protected bool collided;
        protected Action<BaseDamageCollider> _killAction;
        [SerializeField] protected DamageColliderProperties properties;
        protected CharacterManager _owner;

        public abstract void Init(Action<BaseDamageCollider> killAction, CharacterManager owner, float atkPwr);

        protected abstract void OnTriggerEnter(Collider col);

        //check the tag type and return true or false;
        protected bool TargetTag(string tag, TargetTypes type)
        {
            if (type.HasFlag(TargetTypes.Player))
            {
                if (tag.Contains(TargetTypes.Player.ToString()))
                    return true;
            }
            else if (type.HasFlag(TargetTypes.Enemy))
            {
                if (tag.Contains(TargetTypes.Enemy.ToString()))
                    return true;
            }
            else if (type.HasFlag(TargetTypes.Projectile))
            {
                if (tag.Contains(TargetTypes.Projectile.ToString()))
                    return true;
            }
            return false;
        }

        //Activate the given property
        public void EngageProperty(Collider col, Vector3 dir, DamageProperties property, bool center)
        {
            switch (property)
            {
                case DamageProperties.Knockback:
                    properties.ActiveKnockBack(col, dir, properties.power);
                    break;
                case DamageProperties.Launcher:
                    properties.ActiveLaunch(col, properties.power);
                    break;
                case DamageProperties.PullIn:
                    properties.ActivePullIn(col, dir, properties.threshold, properties.power, center);
                    break;
                case DamageProperties.PushOut:
                    properties.ActivePushOut(col, dir, properties.power, center);
                    break;
            }
        }

        //initializes destroy method after given seconds
        public IEnumerator KillAfterTime(float sec)
        {
            yield return new WaitForSeconds(sec);
            _killAction(this);
        }

        //sets attack power of collider
        public void SetAttackPower(float atkPwr)
        {
            colliderAtkPwr = atkPwr;
        }

    }
}