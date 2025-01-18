using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public abstract class ProjectileCollisionDetection : MonoBehaviour
    {
        public TargetTypes ownerTypes;
        public GameObject impactFX;

        public TargetTypes attackingTargets;

        public float lastingTime = 15f;

        [SerializeField] protected bool collided;
        protected Action<ProjectileCollisionDetection> _killAction;
        [SerializeField] protected ProjectileProperties properties;

        public abstract void Init(Action<ProjectileCollisionDetection> killAction);

        public void SetupProjectile(GameObject _impactFX)
        {
            impactFX = _impactFX;
        }

        //checks if the given target is a selected one,
        //if it is it returns true else its false.
        protected bool CheckTargetsTag(string targetTag, TargetTypes type)
        {
            if (type.HasFlag(TargetTypes.Player))
            {
                if (targetTag.Contains(TargetTypes.Player.ToString()))
                    return true;
            }
            else if (type.HasFlag(TargetTypes.Enemy))
            {
                if (targetTag.Contains(TargetTypes.Enemy.ToString()))
                    return true;
            }
            else if (type.HasFlag(TargetTypes.Projectile))
            {
                if (targetTag.Contains(TargetTypes.Projectile.ToString()))
                    return true;
            }
            return false;
        }

        public void EngageProperty(Collider col, Vector3 dir, DamageProperties _sendProperty)
        {
            switch (_sendProperty)
            {
                case DamageProperties.Knockback:
                    properties.ActiveProperty(col,  dir, properties.power);
                    break;
                case DamageProperties.Launcher:
                    properties.ActiveProperty(col, properties.power);
                    break;
                case DamageProperties.PullIn:
                    properties.ActiveProperty(col, dir, properties.threshold, properties.power);
                    break;
            }
        }

        public void ActivateImpactVFX(Vector3 pos, Quaternion rotation, float destroyTimer)
        {
            GameObject impactV = Instantiate(impactFX, pos, rotation);
            Destroy(impactV, destroyTimer);
        }
    }
}