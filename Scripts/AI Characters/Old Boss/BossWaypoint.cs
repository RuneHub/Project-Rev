using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class BossWaypoint : MonoBehaviour
    {
        private int Overlappings = 0;

        public bool isOverlapping
        {
            get
            {
                return Overlappings > 0;
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            Overlappings++;
        }

        private void OnTriggerExit(Collider col)
        {
            Overlappings--;
        }



    }
}