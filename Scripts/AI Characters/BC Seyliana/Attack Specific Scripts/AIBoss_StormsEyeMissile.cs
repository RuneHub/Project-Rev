using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace KS
{
    public class AIBoss_StormsEyeMissile : MonoBehaviour
    {

        //Indicator explosion object with own hitbox
        public GameObject stormExplosion;
        public float explosionDestroyTimer = 2f;
        public GameObject StormIndicator;
        public float indicatorDestroyTimer = 2f;

        public BasicDamageCollider hitbox;

        public float missileSpeed = 50f;
        public float missileRotation = 5f;

        private AIBossManager boss;
        private Vector3 targetLocation;
        private bool activated = false;

        [SerializeField] private Rigidbody rb;

        [SerializeField] private float TimeToTarget;
        [SerializeField] private float distanceToTarget;
        [SerializeField] private float currentDistance;
        [SerializeField] private float distanceThreshold = 2f;


        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void InitializeMissile(AIBossManager manager, Vector3 targetLoc)
        {
            targetLocation = targetLoc;
            boss = manager;

            hitbox.Init(boss.combatAnimationEvents.DestroyHitbox, boss, boss.statManager.HPTriggerAttack);

            distanceToTarget = Vector3.Distance(targetLocation, transform.position);
            TimeToTarget = distanceToTarget / missileSpeed;

            GameObject indi = Instantiate(StormIndicator, targetLocation, Quaternion.identity, null);
            if (indi.GetComponentInChildren<VisualEffect>() != null) 
            {
                indi.GetComponent<VisualEffect>().SetFloat("Anticipation", TimeToTarget);
            }
            Destroy(indi, indicatorDestroyTimer);

            activated = true;
        }

        private void FixedUpdate()
        {
            if (activated)
            {
                currentDistance = Vector3.Distance(targetLocation, transform.position);

                if (currentDistance < distanceThreshold)
                {
                    InitiateExplosion();
                }

                Vector3 dir = targetLocation - rb.position;
                dir.Normalize();

                float rotateAmount = Vector3.Cross(dir, transform.forward).z;
                rb.angularVelocity = -Vector3.Cross(dir, transform.forward) * missileRotation;
                rb.velocity = transform.forward * missileSpeed;

            }

        }

        private void InitiateExplosion()
        {
            activated = false;

            GameObject explosion = Instantiate(stormExplosion, targetLocation, Quaternion.identity);
            explosion.GetComponentInChildren<BaseDamageCollider>().Init(boss.combatAnimationEvents.DestroyHitbox, boss, boss.statManager.HPTriggerAttack);
            Destroy(explosion, explosionDestroyTimer);

            Destroy(gameObject);
        }

    }
}