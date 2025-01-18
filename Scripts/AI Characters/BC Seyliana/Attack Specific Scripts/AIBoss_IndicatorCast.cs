using KS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace kS
{
    public class AIBoss_IndicatorCast : MonoBehaviour
    {
        public float castTime;
        public GameObject Indicator;
        public GameObject Cast;
        private AIBossManager boss;

        private void Awake()
        {
            boss = GameObject.FindFirstObjectByType<AIBossManager>();
        }

        private void Start()
        {

            if (Indicator.GetComponentInChildren<VisualEffect>() != null &&
                Cast.GetComponentInChildren<VisualEffect>() != null)
            {
                StartCoroutine(ExecuteCast());
            }

        }

        public float GetVFXFloat(GameObject obj, string floatName)
        {
            if (obj == Indicator)
            {
                return Indicator.GetComponent<VisualEffect>().GetFloat(floatName);
            }
            else if (obj == Cast)
            {
                return Cast.GetComponent<VisualEffect>().GetFloat(floatName);
            }
            else
            {
                return 0;
            }
        }

        public bool SetVFXFloat(GameObject obj, string floatName, float floatNum)
        {
            if (obj == Indicator)
            {
                Indicator.GetComponent<VisualEffect>().SetFloat(floatName, floatNum);   
                return true;
            }
            else if (obj == Cast)
            {
                Cast.GetComponent<VisualEffect>().SetFloat(floatName, floatNum);
                return true;
            }
            else
            {
                return false;
            }
        }

        //instantiate the indicator, when castime is finished and the vfx is done,
        // instantiate the cast && destroy the indicator, then do the same for the cast
        IEnumerator ExecuteCast()
        {
            float duration = 0;
            GameObject aoeIndi = Instantiate(Indicator, transform.position, transform.rotation, transform);
            yield return new WaitForSeconds(castTime);

            if (Indicator.GetComponentInChildren<VisualEffect>().aliveParticleCount == 0)
            {
                GameObject cast = Instantiate(Cast, transform.position, transform.rotation, transform);
                yield return new WaitForSeconds(1);

                duration = cast.GetComponentInChildren<VisualEffect>().GetFloat("Duration");
                Destroy(aoeIndi.gameObject);
            }

            yield return new WaitForSeconds(duration);

            if (Cast.GetComponentInChildren<VisualEffect>().aliveParticleCount == 0)
            {
                Destroy(gameObject);
            }
        }

    }
}