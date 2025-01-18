using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS 
{ 
    public class DamageColliderProperties : MonoBehaviour
    {
        [Header("Properties")]
        public DamageProperties damageProperties;

        public float power; //property power, E.g. how much power it takes to launch a character.
        public float threshold; //property threshold, the size of the property, like the the pullin area
        public Vector3 propertyDirection; //property direction, for when sending collider into a certain direciton.
        public bool usePropertyCenter; // for when using the center of the hitbox a point.
        public bool usePropertyDirection; // for when you want to force a certain direction.

        [Header("Automatic values")]
        public float attackValue;   //the attack value.
        public float criticalHitRate; //the % chance to get a ciritical hit.
        public float criticalHitBuff; //the amount of % that increases.

        private bool isKnockedBack;
        private bool isLaunched;
        private bool isPulledIn;
        private bool isPushedOut;

        [SerializeField] private CharacterManager owner;
        [SerializeField] private CharacterManager CollManager;

        public DamageProperties getProperty(CharacterManager _owner)
        {
            owner = _owner;
            criticalHitBuff = owner.charStatManager.CriticalHitBuff;
            criticalHitRate = owner.charStatManager.CriticalHitRate;
            return damageProperties;
        }

        //ActiveProperty for knock back, where the collided object gets launched in the same direction as the KnockBackDir.
        public void ActiveKnockBack(Collider col, Vector3 knockBackDir, float knockBackPower)
        {
            CollManager = col.transform.root.GetComponent<CharacterManager>();
            if (!usePropertyDirection)
            {
                propertyDirection = knockBackDir;
            }
            isKnockedBack = true;
        }

        //ActiveProperty for launcher, where the collided object get launched straight into the air, with a power float number.
        public void ActiveLaunch(Collider col, float launchPower)
        {
            CollManager = col.transform.root.GetComponent<CharacterManager>();
            isLaunched = true;
        }

        //ActiveProperty for Pull in, where the collided object gets pulled in into the given direction.
        public void ActivePullIn(Collider col, Vector3 pullInDir, float positionDistanceThreshold, float pullInPower, bool useCenter)
        {
            //this link might help: https://pastebin.com/UvrLm2Qz

            CollManager = col.transform.root.GetComponent<CharacterManager>();
            propertyDirection = pullInDir;
            usePropertyCenter = useCenter;
            isPulledIn = true;
        }

        //ActiveProperty for push out, where the collided object gets pushed into a given direction
        public void ActivePushOut(Collider col, Vector3 PushDirection, float pushPower, bool useCenter)
        {

            CollManager = col.transform.root.GetComponent<CharacterManager>();
            usePropertyCenter = useCenter;
            isPushedOut = true;
        }

        public void DeactiveForce()
        {
            isPulledIn = false;
            isPushedOut = false;
        }

        private void Update()
        {
            if (isKnockedBack)
            {
                isKnockedBack = false;
                AttemptKnockbackCol();
            }

            if (isLaunched)
            {
                isLaunched = false;
                AttemptLaunceCol();
            }

            if (isPulledIn)
            {
                AttemptPullInCol();
            }

            if (isPushedOut)
            {
                AttemptPushOutCol();
            }

        }

        private void AttemptLaunceCol()
        {
            if (CollManager.charLocomotionManager != null)
            {
                Debug.Log("launced");
                //Debug.Log("grounded: " + collController.isGrounded);
                CollManager.charLocomotionManager.yVelocity.y = Mathf.Sqrt(power * -2 * CollManager.charLocomotionManager.gravityForce);
                //Debug.Log("yvelo: " + collController.GetComponent<CharacterLocomotionManager>().yVelocity.y);
            }
        }

        private void AttemptKnockbackCol()
        {
            if (CollManager.charLocomotionManager != null)
            {
                Debug.Log("Knockback");
                //Debug.Log("dir: " + propertyDirection);

                if (!CollManager.isGrounded)
                {
                    CollManager.charLocomotionManager.yVelocity.y = Mathf.Sqrt(2 * -2 * CollManager.charLocomotionManager.gravityForce);
                }

                CollManager.charLocomotionManager.adMoveForce = power;
                CollManager.charLocomotionManager.adMoveDirection = propertyDirection;
                CollManager.charLocomotionManager.useAdditionalMovement = true;
                CollManager.animator.SetBool("AdMovement", true);

                Vector3 rd = transform.position - CollManager.transform.position;
                rd.y = 0;
                rd.Normalize();
                Quaternion tr = Quaternion.LookRotation(rd);
                CollManager.transform.rotation = tr;

            }
        }

        private void AttemptPullInCol()
        {
            if (CollManager.charLocomotionManager != null)
            {
                Debug.Log("Pulled In");

                CollManager.controller.Move(propertyDirection * power * Time.deltaTime);

            }
        }

        private void AttemptPushOutCol()
        {
            if (CollManager.charLocomotionManager != null)
            {
                Debug.Log("Pushed Out");
            }
        }

    }
}