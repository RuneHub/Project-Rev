using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public enum DestroyAtEnd {  none, projectile, path, Both }
    public class ProjectilePathFollower : MonoBehaviour
    {

        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 50;
        public int loopAmount;

        public bool destroyAtEnd = false;
        public float destroyTimer;
        public DestroyAtEnd destroyWho = DestroyAtEnd.none;

        private float distanceTavelled;
        
        
        void Start()
        {
            if (pathCreator != null)
            {
                pathCreator.pathUpdated += OnPathChanged;
            }
        }


        void Update()
        {
            if (pathCreator != null)
            {
                distanceTavelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTavelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTavelled, endOfPathInstruction);

                if(distanceTavelled >= (pathCreator.path.length * loopAmount))
                {
                    endOfPathInstruction = EndOfPathInstruction.Stop;
                    if (destroyAtEnd)
                    {
                        switch (destroyWho)
                        {
                            case DestroyAtEnd.none:
                                break;
                            case DestroyAtEnd.projectile:
                                Destroy(gameObject, destroyTimer);
                                break;
                            case DestroyAtEnd.path:
                                Destroy(pathCreator.gameObject, destroyTimer);
                                break;
                            case DestroyAtEnd.Both:
                                Destroy(gameObject, destroyTimer);
                                Destroy(pathCreator.gameObject, destroyTimer);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

        }

        void OnPathChanged()
        {
            distanceTavelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

    }
}
