using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class BossWaypointHQ : MonoBehaviour
    {
        [SerializeField]
        private BossWaypoint waypoint;

        private void Start()
        {
           
        }

        public bool CheckExistingWaypoint()
        {
            if (waypoint == null)
                return false;
            else
                return true;
        }

        public void FindWaypoint()
        {
            waypoint = FindObjectOfType<BossWaypoint>();
        }

        public bool WaypointOverlappingReport
        {

            get
            {
                return waypoint.isOverlapping;
            }

        }

        public void MoveWaypoint(Vector3 newPos, bool addtoCurrent = false)
        {
            if (addtoCurrent)
            {
                waypoint.transform.position += newPos;
            }
            else
            {
                waypoint.transform.position = newPos;
            }
        }

        public BossWaypoint GetWaypoint()
        {
            return waypoint;
        }

        public Transform GetWaypointTransform()
        {
            return waypoint.transform;
        }

        public void DestroyWaypoint(BossWaypoint point = null)
        {
            if (point != null)
            {
                Destroy(point.gameObject);
            }

            if (waypoint != null)
            {
                Destroy(waypoint.gameObject);
            }

        }

    }
}