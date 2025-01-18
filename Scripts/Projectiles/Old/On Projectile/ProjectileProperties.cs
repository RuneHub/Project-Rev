using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

namespace KS
{
    public class ProjectileProperties : MonoBehaviour
    {
        [Header("Properties")]
        public DamageProperties damageProperties;

        public float power;
        public float threshold;

        [Header("Automatic values")]
        public float attackValue;   //the attack value.
        public float CriticalHitRate; //the % chance to get a ciritical hit.
        public float CriticalHitBuff; //the amount of % that increases.

        private bool isKnockedBack;
        private bool isLaunched;
        private bool isPulledIn;

        private CharacterController collController;

        public DamageProperties getProperty()
        {
            return damageProperties;
        }

        //ActiveProperty for knock back, where the collided object gets launched in the same direction as the KnockBackDir.
        public void ActiveProperty(Collider col, Vector3 knockBackDir, float knockBackPower)
        {

        }

        //ActiveProperty for launcher, where the collided object get launched straight into the air, with a power float number.
        public void ActiveProperty(Collider col, float launchPower)
        {
            collController = col.transform.root.GetComponent<CharacterController>();
            isLaunched = true;
        }

        //ActiveProperty for Pull in, where the collided object gets pulled in into the given direction.
        public void ActiveProperty(Collider col, Vector3 pullInDir, float positionDistanceThreshold, float pullInPower)
        {
            //this link might help: https://pastebin.com/UvrLm2Qz
        }

        private void Update()
        {
            if (isKnockedBack)
            {
               
            }

            if (isLaunched)
            {
                isLaunched = false;
                AttemptLaunceCol();
            }

            if (isPulledIn)
            {

            }

        }

        private void AttemptLaunceCol()
        {
            //collController.Move(Vector3.up * power * Time.deltaTime);
            if (collController.GetComponent<CharacterLocomotionManager>() != null)
            {
                Debug.Log("grounded: " + collController.isGrounded);
                collController.GetComponent<CharacterLocomotionManager>().yVelocity.y = Mathf.Sqrt(power * -2 * collController.GetComponent<CharacterLocomotionManager>().gravityForce);
                Debug.Log("yvelo: " + collController.GetComponent<CharacterLocomotionManager>().yVelocity.y);
            }
        }

    }
}