using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class FallDetector : MonoBehaviour
    {
        private PlayerManager player;

        private void OnTriggerEnter(Collider col)
        {
            if (col.transform.root.gameObject.tag == "Player")
            {
                CameraManager.singleton.StopFollowingTarget = true;
                player = col.GetComponentInParent<PlayerManager>();
                Debug.Log("Fallen!");
                player.playerStats.TakeDamage(player.playerStats.maxHealth, false, Color.white, 0, DamageProperties.Normal);
            }
        }

    }
}