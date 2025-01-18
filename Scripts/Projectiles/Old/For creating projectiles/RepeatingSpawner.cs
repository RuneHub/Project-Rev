using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KS
{
    public class RepeatingSpawner : MonoBehaviour
    {
        public float repeatTimer = 4f;
        public GameObject spawnObject;
        
        public bool spawnOnRepeat;

        public List<Transform> spawnPoints = new List<Transform>();

        private float waitTime;

        private void Start()
        {
        }

        public void Restart()
        {
            if (repeatTimer != 0)
            {
                waitTime = (repeatTimer / spawnPoints.Count);
            }

            spawnOnRepeat = true;
            StartCoroutine(RepeatSpawn());
        }


        IEnumerator RepeatSpawn()
        {
            while (spawnOnRepeat)
            {
                for (int i = 0; i < spawnPoints.Count; i++)
                {
                    GameObject spawn = Instantiate(spawnObject, spawnPoints[i].position, Quaternion.identity);
                    yield return new WaitForSeconds(waitTime);
                }
            }

        }

    }
}