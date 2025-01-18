using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

namespace KS
{
    public class BossFlight : MonoBehaviour
    {

        BossManager manager;

       [SerializeField] private EndOfPathInstruction pathEndInstruction = EndOfPathInstruction.Loop;
       [SerializeField] private float distanceTravelled;

        public PathCreator pathCreator;
        public float speed = 100;

        public int loopAmount = 3;

        private void Awake()
        {
            manager = GetComponent<BossManager>();
        }

        private void Start()
        {
            if (pathCreator != null)
            {
                pathCreator.pathUpdated += OnPathChanged;
                manager.repeatingSpawner.enabled = true;
                manager.repeatingSpawner.Restart();
            }
        }

        private void Update()
        {
            if (pathCreator != null)
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, pathEndInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, pathEndInstruction);

                if (distanceTravelled >= (pathCreator.path.length * loopAmount))
                {
                    pathEndInstruction = EndOfPathInstruction.Stop;
                    manager.galeFlutterActive = false;
                    manager.repeatingSpawner.spawnOnRepeat = false;
                    Destroy(this);
                }

            }
        }

        void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

        public void Reset()
        {
            pathEndInstruction = EndOfPathInstruction.Loop;
            distanceTravelled = 0;

            if (pathCreator != null)
            {
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

    }

}