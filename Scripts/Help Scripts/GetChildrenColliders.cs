using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class GetChildrenColliders : MonoBehaviour
    { 
        //returns a list of all children from the given parent object with a collider attached to them.
        public static List<GameObject> GetAllChildrenColliders(GameObject Parent)
        {
            List<GameObject> ChildrenColliders = new List<GameObject>();

            foreach (var c in Parent.GetComponentsInChildren<Collider>())
            {
                ChildrenColliders.Add(c.gameObject);
            }

            return ChildrenColliders;
        }
    }
}