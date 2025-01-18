using PathCreation;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KS
{
    [RequireComponent(typeof(PathCreator))]
    public class GenerateProjectilePath : MonoBehaviour
    {
        private BezierPath path;

        public float normalsRotation = 83;
        public bool closedLoop = false;

        public List<Transform> waypoints = new List<Transform>();

        // Start is called before the first frame update
        void Start()
        {
            //temp
           // GeneratePath();
            
        }


        public void InsertWaypoints(Transform insertWaypoint)
        {
            waypoints.Add(insertWaypoint);
        }

        public void RemoveWaypoint(Transform removal)
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                if (waypoints[i] == removal)
                {
                    waypoints.Remove(waypoints[i]);
                }
            }
        }

        public void ClearWaypoints()
        {
            waypoints.Clear();
        }

        public void DestroyAndClearWaypoints()
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                Destroy(waypoints[i]);
            }
            waypoints.Clear();
        }

        public void GeneratePath()
        {
            if (waypoints.Count > 0)
            {
                path = new BezierPath(waypoints, closedLoop, PathSpace.xyz);
                path.GlobalNormalsAngle = normalsRotation;
                GetComponent<PathCreator>().bezierPath = path;
            }
            else
            {
                Debug.LogError("no added waypoints");
            }
        }

    }
}