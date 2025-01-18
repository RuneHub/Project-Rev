using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class AtlosSingle : MonoBehaviour
    {
        public TargetTypes targetingType;
        public LayerMask IgnoreLayers;

        public Vector3 GetSize()
        {
            return transform.localScale;
        }

        public void ScanUnit(out bool targetReport, out Vector3 myPos)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, layerMask: IgnoreLayers);

            if (colliders.Length > 0)
            {
                //debug.Log("unit " + gameObject.name + " reports: unidentified object found");

                foreach (Collider col in colliders)
                {
                    if (checkTargetsTag(col.transform.root.gameObject.tag, targetingType))
                    {
                        //debug.Log("unit " + gameObject.name + " reports: player Identified");
                        targetReport = true;
                        myPos = transform.position;
                        return;
                    }
                }
            }
           
            //debug.Log("unit " + gameObject.name + " reports: Nothing found");
            targetReport = false;
            myPos = transform.position;

        }

        //checks if the given target is a selected one,
        //if it is it returns true else its false.
        private bool checkTargetsTag(string targetTag, TargetTypes type)
        {
            if (type.HasFlag(TargetTypes.Player))
            {
                if (targetTag.Contains(TargetTypes.Player.ToString()))
                    return true;
            }
            return false;
        }

    }

}