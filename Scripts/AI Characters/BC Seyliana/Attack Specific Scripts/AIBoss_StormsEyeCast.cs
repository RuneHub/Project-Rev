using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace KS
{
    public class AIBoss_StormsEyeCast : MonoBehaviour
    {
        public float castTime = 3f;
        public float extraTime = 2f;
        public GameObject castStar;
        public AIBoss_StormsEyeMissile StormsEyeMissile;

        private Vector3 location;
        private AIBossManager boss;

        public void StartCast(AIBossManager manager, Vector3 loc)
        {
            Debug.Log("Cast");
            boss = manager;
            location = loc;
            if (castStar.GetComponentInChildren<VisualEffect>() != null)
            {
                StartCoroutine(ExecuteCast());
            }
        }

        IEnumerator ExecuteCast()
        {
            GameObject cast = Instantiate(castStar, transform); 
            Destroy(cast, castTime + extraTime);
            yield return new WaitForSeconds(castTime);

            AIBoss_StormsEyeMissile missile = Instantiate(StormsEyeMissile, transform);
            missile.transform.parent = null;
            //missille stuff
            missile.InitializeMissile(boss, location);

            yield return new WaitForSeconds(extraTime);
            Destroy(gameObject);
        }

    }
}