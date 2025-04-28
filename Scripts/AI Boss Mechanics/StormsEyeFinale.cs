using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace KS
{
    public class StormsEyeFinale : MonoBehaviour
    {
        [Header("Weapons")]
        public PathCreator[] paths = new PathCreator[3];
        public StormsEyeFinaleWeapon[] weapons = new StormsEyeFinaleWeapon[3];

        public Transform[] startPostions = new Transform[3];
        public Transform[] EndPostions = new Transform[3];
        public float weaponMoveDelay = 2f;
        public float moveTime = 1.5f;

        [Header("Fissure")]
        public GameObject Fissure;
        public float fissureOpenTime;
        [Range(0,100)] public float fissureOpenAmount = 100;
        public GameObject Hitboxes;
        public float hitboxstayTime = 1f;
        [Space(10)]
        public float fissureCloseTime = 1f;

        [Header("Fissure VFX")]
        public GameObject fissureVFX;

        private AIBossManager boss;

        private void Awake()
        {
            FindManagers();
        }
        private void FindManagers()
        {
            boss = FindAnyObjectByType<AIBossManager>();
        }

        private void Start()
        {
            Fissure.gameObject.SetActive(false);
            Hitboxes.SetActive(false);
            fissureVFX.SetActive(false);
        }

        [ContextMenu("PlayFinale")]
        public void PlayFinale()
        {
            if (boss == null)
            {
                FindManagers();
            }

            StartCoroutine(ExecuteFinale());
        }

        IEnumerator ExecuteFinale()
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                StormsEyeFinaleWeapon wep = Instantiate(weapons[i], startPostions[i].transform.position, startPostions[i].rotation);
                Vector3 dir = EndPostions[i].position - wep.transform.position;
                wep.transform.rotation = Quaternion.LookRotation(dir);
                float wepSpeed = Vector3.Distance(startPostions[i].position, EndPostions[i].position) / moveTime;
                wep.GetComponentInChildren<PathFollower>().pathCreator = paths[i];
                wep.GetComponentInChildren<PathFollower>().speed = wepSpeed;
                wep.Initialize(boss, EndPostions[i].position, weaponMoveDelay, moveTime);
                
            }

            yield return new WaitForSeconds(moveTime + weaponMoveDelay);

            StartDeckExplode();
        }

        private void StartDeckExplode()
        {
            Fissure.gameObject.SetActive(true);

            StartCoroutine(DeckExplode());   
        }

        IEnumerator DeckExplode()
        {
            float te = 0;
            SkinnedMeshRenderer[] rend = Fissure.GetComponentsInChildren<SkinnedMeshRenderer>();

            while (te < fissureOpenTime)
            {
                float t = te / fissureOpenTime;
                fissureOpenAmount = Mathf.Lerp(100,0,t);
                te += Time.deltaTime;
                
                for (int i = 0; i < rend.Length; i++) 
                {
                    rend[i].SetBlendShapeWeight(0, fissureOpenAmount);
                }

                yield return null;

            }

            fissureOpenAmount = 0;
            for (int i = 0; i < rend.Length; i++)
            {
                rend[i].SetBlendShapeWeight(0, 0);
            }

            fissureVFX.SetActive(true);
            Hitboxes.SetActive(true);
            for (int i = 0; i < Hitboxes.transform.childCount; i++)
            {
                Hitboxes.transform.GetChild(i).GetComponent<BasicDamageCollider>().
                    Init(boss.combatAnimationEvents.DestroyHitbox, boss, boss.statManager.HPTriggerAttack);
            }
            
            yield return new WaitForSeconds(hitboxstayTime);

            StartCleanUp();
        }

        private void StartCleanUp()
        {
            StartCoroutine(CleanUp());    
        }

        IEnumerator CleanUp()
        {
            Hitboxes.SetActive(false);
            float te = 0;
            while (te < fissureCloseTime)
            {
                float t = te / fissureCloseTime;
                fissureOpenAmount = Mathf.Lerp(0, 100, t);
                te += Time.deltaTime;
                SkinnedMeshRenderer[] rend = Fissure.GetComponentsInChildren<SkinnedMeshRenderer>();

                for (int i = 0; i < rend.Length; i++)
                {
                    rend[i].SetBlendShapeWeight(0, fissureOpenAmount);
                }

                yield return null;

            }
            Fissure.gameObject.SetActive(false);
            Debug.Log("End Eye of the Storm!");
            boss.HpTriggerFlag = true;
        }

    }
}