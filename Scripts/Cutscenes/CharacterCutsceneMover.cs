using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class CharacterCutsceneMover : MonoBehaviour
    {
        public AIBossManager boss;
        public PlayerManager player;
        public CameraManager gameplayCam;

        public Transform targetLocationBoss;
        public Transform targetLocationPlayer;

        private void Awake()
        {
            
        }

        private void Start()
        {
            
        }

        public void MoveToTarget()
        {
            if (boss != null && targetLocationBoss != null)
            {
                boss.transform.position = targetLocationBoss.position;
            }
            if (player != null && targetLocationPlayer != null && gameplayCam != null)
            {
                player.transform.position = targetLocationPlayer.position;
                player.transform.rotation = targetLocationPlayer.rotation;
                gameplayCam.transform.position = targetLocationPlayer.position;
                gameplayCam.transform.rotation = player.transform.rotation;
            }
        }

    }
}