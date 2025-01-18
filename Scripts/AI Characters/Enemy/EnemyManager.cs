using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

namespace KS {

    public class EnemyManager : CharacterManager
    {
        [Header("Chase")]
        public bool ChasePlayer;
        public bool UseHitbox;
        public bool Move;
        public bool moveFirst;
        public float moveSpeed = 1f;

        [Header("Shoot")]
        public bool ShootPlayer;
        public bool facePlayer;
        public float projSpeed = 20f;
        public float ShootTimer;
        public float shootingTime = 5f;

        [Space]
        public Vector3 posA;
        public Vector3 posB;

        private Vector3 addPos = Vector3.zero;

        PlayerManager player;
        public BaseDamageCollider hitbox;
        public float attackPwr = 1f;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            posA = transform.position;
            ShootTimer = shootingTime;
            player = FindObjectOfType<PlayerManager>();
            if (UseHitbox)
            {
                addPos = new Vector3(transform.position.x,
                                             transform.position.y + 1f,
                                             transform.position.z - .75f);
                if (!ShootPlayer)
                {
                    BaseDamageCollider col = Instantiate(hitbox, addPos,
                                                                 Quaternion.identity,
                                                                 transform);
                    col.GetComponent<BaseDamageCollider>().Init(DestroyTest, this, attackPwr);
                }
            }
        }

        protected override void Update()
        {
            base.Update();
            if (!isDead) 
            {
                if (!ChasePlayer)
                {
                    if (transform.position == posA)
                    {
                        moveFirst = true;
                    }
                    else if (transform.position == posB)
                    {
                        moveFirst = false;
                    }

                    if (Move)
                    {
                        if (moveFirst)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, posB, moveSpeed * Time.deltaTime);
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, posA, moveSpeed * Time.deltaTime);
                        }
                    }
                }
                else
                {
                    if (transform.position != player.transform.position)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                        transform.LookAt(target);
                    }
                }

                if (ShootPlayer)
                {

                    if (ShootTimer > 0)
                    {
                        ShootTimer -= Time.deltaTime;

                        if (ShootTimer <= 0)
                        {
                            if (facePlayer)
                            {
                                transform.LookAt(player.transform.position);
                            }
                            ShootHitBox();
                            ShootTimer = shootingTime;
                        }

                    }

                }

            }   
        }

        private void ShootHitBox()
        {
            BaseDamageCollider proj = Instantiate(hitbox, addPos, Quaternion.identity, transform);
            proj.transform.position = addPos;
            proj.GetComponent<Rigidbody>().velocity = transform.forward.normalized * projSpeed;
            proj.GetComponent<BaseDamageCollider>().Init(DestroyTest, this, attackPwr);
        }

        //temp
        private void DestroyTest(BaseDamageCollider obj)
        {
            Destroy(obj.gameObject);
        }


    }
}