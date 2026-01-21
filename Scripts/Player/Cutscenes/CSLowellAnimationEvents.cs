using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class CSLowellAnimationEvents : MonoBehaviour
    {

        public GameObject LeftHandWeapon;
        public GameObject RightHandWeapon;
        public GameObject LeftHolsterWeapon;
        public GameObject RightHolsterWeapon;
        [Space]
        public GameObject LeftSpinEffect;
        public GameObject RightSpinEffect;
        [Space]
        public float dissolveRate = 0.0125f;
        public float refreshRate = 0.025f;
        public bool dissolving = false;
        [SerializeField] private SkinnedMeshRenderer[] SMR;
        [SerializeField] private MeshRenderer[] MR;

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

        //spin effect turning on
        public void SpinFXOn(string _side)
        {
            if (_side == "Left")
            {
                LeftSpinEffect.SetActive(true);
            }
            else if (_side == "Right")
            {

                RightSpinEffect.SetActive(true);
            }
            else if (_side == "Both")
            {
                LeftSpinEffect.SetActive(true);
                RightSpinEffect.SetActive(true);
            }
        }

        //spin effect turning off
        public void SpinFXOff(string _side)
        {
            if (_side == "Left")
            {
                LeftSpinEffect.SetActive(false);
            }
            else if (_side == "Right")
            {

                RightSpinEffect.SetActive(false);
            }
            else if (_side == "Both")
            {
                LeftSpinEffect.SetActive(false);
                RightSpinEffect.SetActive(false);
            }
        }
        #endregion

        #region Clone
        //setting the dissolve to 0 making the clone visible
        public void HandleVisibile()
        {
            for (int x = 0; x < SMR.Length; x++)
            {
                SMR[x].materials[0].SetFloat("_DissolveAmount", 0);
                SMR[x].materials[1].SetFloat("_DissolveAmount", 0);
            }

            for (int x = 0; x < MR.Length; x++)
            {
                MR[x].materials[0].SetFloat("_DissolveAmount", 0);
            }
        }

        //setting the dissolve to 1, to make the instant invsible
        public void HandleInvisible()
        {
            for (int x = 0; x < SMR.Length; x++)
            {
                SMR[x].materials[0].SetFloat("_DissolveAmount", 1);
                SMR[x].materials[1].SetFloat("_DissolveAmount", 1);
            }

            for (int x = 0; x < MR.Length; x++)
            {
                MR[x].materials[0].SetFloat("_DissolveAmount", 1);
            }
        }

        //start the coroutine to for the dissove effect.
        public void HandleDissolve()
        {
            StartCoroutine(StartDissolve());
        }

        //the dissove effect,
        //uses all of the SkinnedMeshRenderers & MeshRenderers components,
        //starts adding to the "DissolveAmount" for the effect to happen.
        //the speed it dissoves depends on the "dissolveRate" variable.
        private IEnumerator StartDissolve()
        {
            dissolving = true;

            if (SMR.Length > 0)
            {
                float counter = 0;
                while (SMR[0].material.GetFloat("_DissolveAmount") < 1)
                {
                    counter += dissolveRate;
                    for (int x = 0; x < SMR.Length; x++)
                    {
                        SMR[x].materials[0].SetFloat("_DissolveAmount", counter);
                        SMR[x].materials[1].SetFloat("_DissolveAmount", counter);
                    }

                    for (int x = 0; x < MR.Length; x++)
                    {
                        MR[x].materials[0].SetFloat("_DissolveAmount", counter);
                    }

                    yield return new WaitForSeconds(refreshRate);
                    

                }
            }
        }
        #endregion

    }
}