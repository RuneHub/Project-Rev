using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

namespace KS
{
    public class ScreenShatter : MonoBehaviour
    {
        public float CrackToShatterTime;
        public GameObject child;

        [Header("Effect")]
        public float effectStart;
        public float effectEnd;
        public float effectDuration;

        [Header("Fresnel")]
        public float fresnelStart;
        public float fresnelEnd;
        public float FresnelDuration;
        public float FresnelReverseDuration;

        [Header("Fracture")]
        [SerializeField] private Vector3 fractureCenter;
        [SerializeField] private Transform centerObject;
        [SerializeField] private Material fractureMaterial;
        private int shatterCenter = Shader.PropertyToID("_FractureCenter");

        [SerializeField] private MeshRenderer[] MR;

        private void Start() 
        { 
            child.SetActive(false);
        }

        private void Update()
        {
            //fractureCenter = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z * -1);
            fractureCenter = centerObject.position + centerObject.localPosition;
        }

        public void startShatteringSequence()
        {
            StartCoroutine(RunShatterSequence());

        }

        IEnumerator RunShatterSequence()
        {
            child.SetActive(true);

            fractureMaterial.SetVector(shatterCenter, fractureCenter);

            //fresnel
            for (int i = 0; i < MR.Length; i++) 
            {
                StartCoroutine(AnimateMaterialFloat(MR[i].GetComponent<MeshRenderer>().material, "_FresnelPower", fresnelStart, fresnelEnd, FresnelDuration));
            }
            yield return new WaitForSeconds(CrackToShatterTime);

            for (int i = 0; i < MR.Length; i++)
            {
                StartCoroutine(AnimateMaterialFloat(MR[i].GetComponent<MeshRenderer>().material, "_Effect", effectStart, effectEnd, effectDuration));
            }
            yield return new WaitForSeconds(effectDuration);
            Debug.Log("Finished");
            StartCoroutine(ResetSequence());
        }

        IEnumerator AnimateMaterialFloat(Material mat, string shaderVarRef, float start, float goal, float duration)
        {
            float elapsed = 0;
            float effect = 0;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                effect = Mathf.Lerp(start, goal, elapsed / duration);

                mat.SetFloat(shaderVarRef, effect);
                yield return null;
            }
        }

        //set the fresnel effect back to it becomes hard to see again, then reset the effect and turn the child off, so it is invisibile
        IEnumerator ResetSequence()
        {
            for (int i = 0; i < MR.Length; i++)
            {
                StartCoroutine(AnimateMaterialFloat(MR[i].GetComponent<MeshRenderer>().material, "_FresnelPower", fresnelEnd, fresnelStart, FresnelReverseDuration));
            }
            yield return new WaitForSeconds(FresnelReverseDuration + 0.5f);

            for (int i = 0; i < MR.Length; i++)
            {
                MR[i].GetComponent<MeshRenderer>().enabled = false;
                MR[i].GetComponent<MeshRenderer>().material.SetFloat("_Effect", effectStart);
                MR[i].GetComponent<MeshRenderer>().enabled = true;
            }
            yield return new WaitForSeconds(2);
            child.SetActive(false);
        }

    }
}