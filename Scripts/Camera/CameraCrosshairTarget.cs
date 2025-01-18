using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class CameraCrosshairTarget : MonoBehaviour
    {
        PlayerManager player;

        private Camera mainCam;

        private  Ray ray;
        private RaycastHit hitInfo;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();

            mainCam = Camera.main;
        }

        //shoots an raycast out from the main camera to check if there is anything infront so the aim is not off.
        //sets itself infront of an object if it is infront of the camera so that the aim is more precise.
        //this is the object where the output transform for aim,
        //this means that it looks like they aim at the same place as the weapons, but htey don't
        private void Update()
        {
            /*
            if (player.modeManager.currentMode == PlayMode.OTSMode)
            {
                ray.origin = mainCam.transform.position;
                ray.direction = mainCam.transform.forward;

                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.collider.gameObject.tag != "Player")
                    {
                        transform.position = hitInfo.point;
                    }
                }
                else
                {
                    transform.position = player.cameraHandler.AimLookAt.position;
                }

            }*/
        }

    }
}