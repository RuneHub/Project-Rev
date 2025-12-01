using KSTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace KS { 
    public class AirshipStatus : MonoBehaviour
    {
        public List<GameObject> CompleteParts = new List<GameObject>();
        public List<GameObject> RemainingParts = new List<GameObject>();
        public List<GameObject> BrokenMast = new List<GameObject>();
        public List<GameObject> BrokenWing = new List<GameObject>();

        public Transform ForcePositionMast;
        public Transform ForcePositionWing;

        public float forceStrength;
        public float forceRadius;
        public float shrinkFactor;

        private void Start()
        {
            for (int i = 0; i < RemainingParts.Count; i++)
            {
                RemainingParts[i].SetActive(false);
            }

            for (int i = 0; i < BrokenMast.Count; i++)
            {
                BrokenMast[i].SetActive(false);
            }
            
            for (int i = 0; i < BrokenWing.Count; i++)
            {
                BrokenWing[i].SetActive(false);
            }
        }

        [ContextMenu("SwapParts")]
        public void SwapParts()
        {
            for (int i = 0; i < CompleteParts.Count; i++)
            {
                CompleteParts[i].SetActive(false);
            }

            for (int i = 0; i < RemainingParts.Count; i++)
            {
                RemainingParts[i].SetActive(true);
            }

            StartFractureProcess();
            
        }

        private void StartFractureProcess()
        {
            for (int i = 0; i < BrokenMast.Count; i++)
            {
                BrokenMast[i].SetActive(true);

                float distance = Vector3.Distance(ForcePositionMast.position, BrokenMast[i].transform.position);
                Vector3 direction = BrokenMast[i].transform.position - ForcePositionMast.position;
                float force = (forceStrength / distance);

                var rb = BrokenMast[i].GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(force, ForcePositionMast.position, forceRadius);
                }

                StartCoroutine(Shrink(BrokenMast[i].transform, 5));
                Destroy(BrokenMast[i], 10);
            }

            for (int i = 0; i < BrokenWing.Count; i++)
            {
                BrokenWing[i].SetActive(true);

                float distance = Vector3.Distance(ForcePositionMast.position, BrokenWing[i].transform.position);
                Vector3 direction = BrokenWing[i].transform.position - ForcePositionMast.position;
                float force = (forceStrength / distance);

                var rb = BrokenWing[i].GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(force, ForcePositionMast.position, forceRadius);
                }

                StartCoroutine(Shrink(BrokenWing[i].transform, 5));

                Destroy(BrokenWing[i], 10);
            }



            Debug.Log("Fracture explosion");

        }


        IEnumerator Shrink(Transform t, float delay)
        {
            yield return new WaitForSeconds(delay);

            Vector3 newScale = t.localScale;

            while (newScale.x > 0.1f)
            {
                newScale -= new Vector3(shrinkFactor, shrinkFactor, shrinkFactor);

                t.localScale = newScale;
                yield return new WaitForSeconds(0.05f);
            }

        }

    }

}