using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class FootstepSwapper : MonoBehaviour
    {
        PlayerManager manager;

        private string currentlayer;

        FootstepCollection[] FootstepCollections;

        private void Awake()
        {
            manager = GetComponent<PlayerManager>();
        }

        //sends a raycast down tot he current walkable platform for source type
        //sends the collection of sounds to the soundmanager.
        public void CheckLayers()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 3))
            {
                if (hit.transform.GetComponent<SurfaceType>() != null)
                {
                    FootstepCollection collection = hit.transform.GetComponent<SurfaceType>().footstepCollection;
                    currentlayer = collection.name;
                    //swapFootsteps on player
                    manager.soundManager.SwapFootsteps(collection);
                }
            }

        }
    }
}