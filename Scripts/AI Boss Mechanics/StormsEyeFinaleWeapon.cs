using KS;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.VFX;

namespace KS
{
    public class StormsEyeFinaleWeapon : MonoBehaviour
    {
        [Header("Objects")]
        public GameObject Weapon;
        public GameObject finaleExplosion;
        public float explosionDestroyTimer = 2f;
        public GameObject finaleIndicator;
        public float indicatorDestroyTimer = 2f;

        [Header("Clean up")]
        public Material wepMat;
        public float dissolveDelay = 2f;
        public float dissolveTime = 1f;

        [SerializeField] private PathFollower follower;

        [SerializeField] private Vector3 endPosition;
        [SerializeField] private Quaternion endRotation;

        Vector3 targetPostion;
        private float startDelay;
        private float landingTime;

        AIBossManager boss;

        public void Initialize(AIBossManager bossManager, Vector3 targetPos, float moveDelay, float moveTime)
        {
            boss = bossManager;
            targetPostion = targetPos;
            startDelay = moveDelay;
            landingTime = moveTime;

            wepMat = GetComponentInChildren<MeshRenderer>().material;

            StartCoroutine(ExecuteSequence());
        }

        IEnumerator ExecuteSequence()
        {
            yield return new WaitForSeconds(startDelay);
            follower.enabled = true;

            GameObject indi = Instantiate(finaleIndicator, targetPostion, Quaternion.identity, null);
            if (indi.GetComponentInChildren<VisualEffect>() != null)
            {
                indi.GetComponent<VisualEffect>().SetFloat("Anticipation", landingTime);
            }
            Destroy(indi, indicatorDestroyTimer);
            StartCoroutine(InitiateExplosion(landingTime));

        }

        IEnumerator InitiateExplosion(float delay)
        {
            yield return new WaitForSeconds(delay);
            follower.enabled = false;
            setEndState();
            StartCoroutine(CleanUp(dissolveDelay));

            //explosion obj
            GameObject explosion = Instantiate(finaleExplosion, targetPostion, Quaternion.identity);
            explosion.GetComponentInChildren<BaseDamageCollider>().Init(boss.combatAnimationEvents.DestroyHitbox, boss, boss.statManager.HPTriggerAttack);
            Destroy(explosion, explosionDestroyTimer);
        }

        IEnumerator CleanUp(float delay)
        {
            yield return new WaitForSeconds(delay);

            float te = 0;
            float dissolve = 0;
            while (te < dissolveTime)
            {
                float t = te / dissolveTime;
                dissolve = Mathf.Lerp(0, 1, t);
                wepMat.SetFloat("MaterializeAmount", dissolve);
                te += Time.deltaTime;
            }
            Destroy(gameObject);
        }

        private void setEndState()
        {
            Weapon.transform.localPosition = endPosition;
            Weapon.transform.localRotation = endRotation;
        }
    }
}