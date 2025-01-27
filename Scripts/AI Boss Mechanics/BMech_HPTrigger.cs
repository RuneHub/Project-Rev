using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class BMech_HPTrigger : MonoBehaviour
    {
        [Header("Missiles")]
        public int castAmount = 50;
        public int accuracyThreshold = 60;
        public float castTime = 3f;
        public float inBetweenCastTime = 2f;

        [Header("Lasers")]
        public Vector3 AppearCount = Vector3.zero;
        public float LaserDuration = 5f;
        public float laserDestroyTime = 2f;

        [Space(15)]
        [Header("Missiles")]
        [SerializeField] private LayerMask enviromentLayer;
        [SerializeField] private GameObject stormsEyeVisual;

        [SerializeField] private List<Transform> outputs = new List<Transform>();
        [SerializeField] private BoxCollider field;

        [SerializeField] private AIBoss_StormsEyeCast StormCast;

        [Header("Lasers")]
        [SerializeField] private Transform laserSource;
        [SerializeField] private List<AIBoss_StormsEyeLaser> lasers = new List<AIBoss_StormsEyeLaser>();

        [Header("Finale")]
        public StormsEyeFinale Finale;

        private AIBossManager boss;
        private PlayerManager player;
        private List<Transform> outputsInUse = new List<Transform>();


        private void Awake()
        {
            boss = FindAnyObjectByType<AIBossManager>();
            player = FindAnyObjectByType<PlayerManager>();
            field = boss.field;
            stormsEyeVisual.SetActive(false);
        }

        //loop x amount of times, start
        //get accuracy check
        //if its true
        //get player location
        //if false
        //get random location in field
        //get a random output
        //instantiate projectile with data
        //loop end

        //get random spear pattern
        //instantiate spear projectile

        [ContextMenu("PlayMechanic")]
        public void PlayMechanic()
        {
            StartCoroutine(ExecuteMechanic());
            Debug.Log("start Eye of the Storm!");
        }

        IEnumerator ExecuteMechanic()
        {
            stormsEyeVisual.SetActive(true);
            for (int i = 0; i < castAmount; i++)
            {
                Transform output = GetOutput();
                StartCoroutine(UseOutput(output));
                AIBoss_StormsEyeCast cast = Instantiate(StormCast, output.position, output.rotation);
                cast.StartCast(boss, GetLocation());

                if (i == AppearCount.x ||
                    i == AppearCount.y ||
                    i == AppearCount.z) 
                {
                    ExecuteLaser();
                }

                yield return new WaitForSeconds(inBetweenCastTime);
            }
            Finale.gameObject.SetActive(true);
            Finale.PlayFinale();

        }

        #region Missiles

        private Vector3 GetLocation()
        {
            Vector3 location = Vector3.zero;
            float random = Random.Range(1, 100);

            if (random > accuracyThreshold)
            {
                //get player location
                //location = player.transform.position;
                RaycastHit hit;
                if (Physics.Raycast(player.transform.position, -Vector3.up, out hit, Mathf.Infinity, enviromentLayer))
                {
                    Debug.DrawRay(player.transform.position, -Vector3.up * hit.distance, Color.yellow);
                    location = hit.point;
                }
                else
                {
                    location = new Vector3(player.transform.position.x, 0, player.transform.position.z);
                }
            }
            else
            {
                //get random field location
                Bounds b = field.bounds;

                location = new Vector3(
                    Random.Range(b.min.x, b.max.x),
                    Random.Range(b.min.y, b.max.y),
                    Random.Range(b.min.z, b.max.z)
                    );

                location = field.ClosestPoint(location);
            }

            location.y = 0;
            return location;
        }

        private Transform GetOutput()
        {
            int index = Random.Range(0, outputs.Count);

            return outputs[index];
        }

        IEnumerator UseOutput(Transform output)
        {
            outputsInUse.Add(output);
            outputs.Remove(output);

            yield return new WaitForSeconds(castTime);

            outputs.Add(output);
            outputsInUse.Remove(output);

        }
        #endregion

        #region Lasers

        private void ExecuteLaser()
        {
            //send it to te ienumerator
            StartCoroutine(HandleLaser(lasers[0]));
        }

        IEnumerator HandleLaser(AIBoss_StormsEyeLaser laser)
        {
            AIBoss_StormsEyeLaser inLaser = Instantiate(laser, null);
            AIBoss_StormsEyeLaser[] sel = inLaser.gameObject.GetComponents<AIBoss_StormsEyeLaser>();

            foreach (AIBoss_StormsEyeLaser il in sel)
            {
                il.InitializeLaser(laserSource.position);
            }
            Destroy(inLaser.gameObject, LaserDuration + laserDestroyTime);

            lasers.Remove(laser);

            yield return new WaitForSeconds(LaserDuration);

        }

        #endregion

    }
}