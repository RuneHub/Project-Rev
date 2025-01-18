using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class CelestialCloneCombatAnimationEvents : MonoBehaviour
    {
        CelestialCloneManager cc;
        public Transform outputL;
        public Transform outputR;
        [Space(10)]
        public GameObject LeftHandWeapon;
        public GameObject RightHandWeapon;
        public GameObject LeftHolsterWeapon;
        public GameObject RightHolsterWeapon;


        private void Awake()
        {
            cc = GetComponentInParent<CelestialCloneManager>(); ;
        }

        #region Visuals
        public void SwapWeaponToHand(string _side)
        {
            if (_side == "Left")
            {
                LeftHandWeapon.SetActive(true);
                LeftHolsterWeapon.SetActive(false);
            }
            else if (_side == "Right")
            {
                RightHandWeapon.SetActive(true);
                RightHolsterWeapon.SetActive(false);
            }
            else if (_side == "Both")
            {
                LeftHandWeapon.SetActive(true);
                RightHandWeapon.SetActive(true);

                LeftHolsterWeapon.SetActive(false);
                RightHolsterWeapon.SetActive(false);
            }

        }

        public void SwapWeaponToHolster(string _side)
        {
            if (_side == "Left")
            {
                LeftHolsterWeapon.SetActive(true);
                LeftHandWeapon.SetActive(false);
            }
            else if (_side == "Right")
            {
                RightHolsterWeapon.SetActive(true);
                RightHandWeapon.SetActive(false);
            }
            else if (_side == "Both")
            {
                LeftHolsterWeapon.SetActive(true);
                RightHolsterWeapon.SetActive(true);

                LeftHandWeapon.SetActive(false);
                RightHandWeapon.SetActive(false);
            }

        }
        #endregion

        public void Shoot(string side)
        {
            if (side == "Left")
            {
                cc.HandleShooting(outputL);
            }
            else if (side == "Right")
            {
                cc.HandleShooting(outputR);
            }
        }

        public void Cancellable()
        {
            cc.isInteracting = false;
        }

    }
       
}