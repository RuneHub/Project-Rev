using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace KS
{
    public class AIBoss_StormsEyeLaser : MonoBehaviour
    {
        private AIBossManager boss;

        public bool activated = false;
        public bool completed = false;
        public GameObject laser;
        public GameObject laserHitBox;
        public float laserDistanceMultiplier = 2;
        public float laserHitboxScaleMultiplier = 170;

        [SerializeField] private float laserXYScale = .1f;

        [SerializeField] private PathFollower pathFollower;
        [SerializeField] private Vector3 laserEndLocation;

        private void Awake()
        {
            boss = FindAnyObjectByType<AIBossManager>();
        }

        public void InitializeLaser(Vector3 laserEnd)
        {
            laserEndLocation = laserEnd;

            pathFollower.gameObject.SetActive(true);
            activated = true;

            if (laserHitBox.GetComponent<BaseDamageCollider>() != null) 
            {
                laserHitBox.GetComponent<BaseDamageCollider>().Init(boss.combatAnimationEvents.DestroyHitbox,
                    boss, boss.statManager.HPTriggerAttack);
            }

        }

        private void Update()
        {
            if (activated)
            {
                stickLaserEnd();
            }
        }

        private void stickLaserEnd()
        {
            if (laserEndLocation != Vector3.zero)
            {
                if (laser != null)
                {
                    Vector3 dir = laserEndLocation - laser.transform.position;
                    laser.transform.rotation = Quaternion.LookRotation(dir);
                    laserHitBox.transform.rotation = Quaternion.LookRotation(dir);

                    float dist = Vector3.Distance(laserEndLocation, laser.transform.position);
                    dist = dist * laserDistanceMultiplier;

                    laser.GetComponentInChildren<VisualEffect>().SetFloat("Size", laserXYScale);
                    laser.GetComponentInChildren<VisualEffect>().SetFloat("ScaleZ", dist);

                    float hitboxScaleXY = laserXYScale * laserHitboxScaleMultiplier;
                    laserHitBox.transform.localScale = new Vector3(hitboxScaleXY, hitboxScaleXY, dist);
                }
            }

        }

    }
}