using System;
using System.Collections;
using UnityEngine;

namespace KS
{
    public class CelestialClone : MonoBehaviour
    {
        public PlayerManager player;
        Animator animator;

        private bool move;
        private bool finisher;
        private Vector3 direction;

        public bool active;
        public Transform Output;

        [Header("Basic movement")]
        public float moveSpeed = 2f;
        public float currMoveSpeed;
        public float speedReducer = 0.2f;
        public float finishedSpeedReducer;

        [Header("Attacks")]
        public AttackStandardSO AttackSO;

        [Header("Effects")]
        public float dissolveRate = 0.0125f;
        public float refreshRate = 0.025f;
        public bool DoneDissolving = false;
        [SerializeField] private SkinnedMeshRenderer[] SMR;
        [SerializeField] private MeshRenderer[] MR;
        private float animTime;

        public Action<Transform, AttackStandardSO> Shoot;

        void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
            animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            ResetCC();
        }

        void Update()
        {
            if (active)
            {
                animTime -= Time.deltaTime;
            }

            if (move)
            {
                transform.Translate(direction * currMoveSpeed * Time.deltaTime);
                if (!finisher)
                {
                    currMoveSpeed -= Time.deltaTime * speedReducer;
                }
                else
                {
                    currMoveSpeed -= Time.deltaTime * finishedSpeedReducer;
                }
            }

            if (currMoveSpeed < 0)
            {
                currMoveSpeed = 0;
            }

            if (animTime < 0)
            {
                animTime = 0;
                move = false;
            }

        }

        //Handle the main effect of the additional effect,
        // it adds the movement direction & plays the animation,
        // gets the animation time & sets the movement bool to true.
        public void PerformPefectAddition(Vector3 dir)
        {
            direction = dir;

            PlayTargetAnimation("PerfectAttack", false);
            animTime = animator.GetCurrentAnimatorClipInfo(0).Length;

            active = true;
            move = true;
        }

        //Handles the main effect of the perfect finisher.
        //checks if it right/left and sets it active.
        public void PeformPerfectFinishAddition(Vector3 dir, bool rightSide)
        {
            finisher = true;
            if (rightSide)
            {
                PlayTargetAnimation("PerfectFinish_R", false);
            }
            else
            {
                PlayTargetAnimation("PerfectFinish_L", false);
            }

            animTime = animator.GetCurrentAnimatorClipInfo(0).Length;
            active = true;
            move = true;
        }

        //uses the delegated Raycast shoot function from Combat manager
        public void HandleAttack()
        {
            Shoot(Output, AttackSO);
        }

        //start the coroutine to for the dissove effect.
        public void HandleTurningTransparenting()
        {
            StartCoroutine(StartDissolve());
        }

        //deactive this object via the pooling function in the player combat manager.
        public void Deactivate()
        {
            finisher = false;
            active = false;
            //player.combatManager.DeactiveCC(this); 
        }

        //resets values that are needed for the next a clone gets used.
        public void ResetCC()
        {
            currMoveSpeed = moveSpeed;
            finishedSpeedReducer = speedReducer * 2;

            for (int x = 0; x < SMR.Length; x++)
            {
                SMR[x].materials[0].SetFloat("_DissolveAmount", 0);
            }

        }

        //the function that will override the animation to play a targeted animation,
        //has parameters that have to be filled and ones that have a default value if left empty.
        private void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool UseRootmotion = false, float CrossFadeSpeed = 0.2f, int layerNum = 0, float normalizedTime = 0f)
        {
            animator.SetBool("isInteracting", isInteracting);
            animator.SetBool("isUsingRootmotion", UseRootmotion);
            animator.CrossFade(targetAnimation, CrossFadeSpeed, layerNum, normalizedTime);
        }

        //the dissove effect,
        //uses all of the SkinnedMeshRenderers & MeshRenderers components,
        //starts adding to the "DissolveAmount" for the effect to happen.
        //the speed it dissoves depends on the "dissolveRate" variable.
        private IEnumerator StartDissolve()
        {
            if (SMR.Length > 0)
            {
                float counter = 0;
                while (SMR[0].material.GetFloat("_DissolveAmount") < 1)
                {
                    counter += dissolveRate;
                    for (int x = 0; x < SMR.Length; x++)
                    {
                        SMR[x].materials[0].SetFloat("_DissolveAmount", counter);
                    }

                    for (int x = 0; x < MR.Length; x++)
                    {
                        MR[x].materials[0].SetFloat("_DissolveAmount", counter);
                    }

                    yield return new WaitForSeconds(refreshRate);
                }
            }

            Deactivate();
        }

    }
}
