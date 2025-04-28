using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KS
{
    public class BMech_TornadoMechanic : BMech_Base
    {

        public MechanicType type;

        public bool fastMechanic;
        public float spawnTimer;

        public GameObject Tornado;

        public List<AtlosSingle> spawnPoints;

        private AtlosMain atlos;

        private void Awake()
        {
            FindManagers();
        }

        private void FindManagers()
        {
            boss = FindAnyObjectByType<AIBossManager>();
            atlos = AtlosMain.atlos;
        }

        private void Start()
        {
        }

        //starts the mechanic coroutine.
        [ContextMenu("PlayMechanic")]
        public override void PlayMechanic()
        {
            if (boss == null)
            {
                FindManagers();
            }

            switch (type)
            {
                case MechanicType.Sequencer:
                    StartCoroutine(ExecuteMechanicSequencer());
                    break;
                case MechanicType.Instant:
                    StartCoroutine(ExecuteMechanicInstant());
                    break;
                case MechanicType.Random:
                    StartCoroutine(ExecuteMechanicRandom());
                    break;
                default:
                    break;
            }
        }

        //stops the mechanic coroutine.
        public override void StopMechanic()
        {
            switch (type)
            {
                case MechanicType.Sequencer:
                    StopCoroutine(ExecuteMechanicSequencer());
                    break;
                case MechanicType.Instant:
                    StopCoroutine(ExecuteMechanicInstant());
                    break;
                case MechanicType.Random:
                    StopCoroutine(ExecuteMechanicRandom());
                    break;
                default:
                    break;
            }
        }

        //starts playing the mechanic which actually means spawning given objects,
        //in order of the list locations after waiting on the spawntimers.
        IEnumerator ExecuteMechanicSequencer()
        {
            MechanicPlaying = true;
           
            Instantiate(Tornado, spawnPoints[0].transform.position, spawnPoints[0].transform.rotation);
            
            if (fastMechanic)
            {
                boss.ActiveMechanic = false;
            }

            for (int i = 1; i < spawnPoints.Count; i++)
            {
                yield return new WaitForSeconds(spawnTimer);

                Instantiate(Tornado, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);

            }

            MechanicPlaying = false;
            boss.ActiveMechanic = false;
        }

        //starts playing the mechanic which actually means spawning given objects,
        //in order of the list locations.
        IEnumerator ExecuteMechanicInstant()
        {
            MechanicPlaying = true;
            if (fastMechanic)
            {
                boss.ActiveMechanic = false;
            }
            yield return new WaitForSeconds(spawnTimer);
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                Instantiate(Tornado, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
            }

            MechanicPlaying = false;
            boss.ActiveMechanic = false;
        }

        //starts playing the mechanic which actually means spawning given objects,
        //Shuffles the list and spawns them after waiting on the timer
        IEnumerator ExecuteMechanicRandom()
        {
            MechanicPlaying = true;

            List<AtlosSingle> randomSpawns = ShuffleList(spawnPoints);

            if (fastMechanic)
            {
                boss.ActiveMechanic = false;
            }

            Instantiate(Tornado, randomSpawns[0].transform.position, spawnPoints[0].transform.rotation);

            for (int i = 1; i < randomSpawns.Count; i++)
            {
                yield return new WaitForSeconds(spawnTimer);

                Instantiate(Tornado, randomSpawns[i].transform.position, spawnPoints[i].transform.rotation);

            }

            MechanicPlaying = false;
            boss.ActiveMechanic = false;
        }

        //shuffles a list in random order
        private List<AtlosSingle> ShuffleList(List<AtlosSingle> ogList)
        {
            System.Random random = new System.Random();
            return ogList.OrderBy(x => random.Next()).ToList();
        }

    }
}