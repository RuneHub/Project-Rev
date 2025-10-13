using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class SkylightManager : MonoBehaviour
    {
        [SerializeField] private Material skyboxMat;
        [SerializeField] private Light dirLightMain;
        [SerializeField] private Light dirLightSub;

        [SerializeField] private Color ph1MainLightColor;
        [SerializeField] private Color ph2MainLightColor;
        [SerializeField] private Color ph1SubLightColor;
        [SerializeField] private Color ph2SubLightColor;

        [Space(10)]
        [SerializeField] private Color cloudColor;
        [SerializeField] private List<MeshRenderer> materialList = new List<MeshRenderer>();

        [SerializeField] private float phaseChangeDuration;

        private void Awake()
        {
            ph1MainLightColor = dirLightMain.color;
            ph1SubLightColor = dirLightSub.color;

            //ChangeSkyBack();
        }
        
        [ContextMenu("change2Dark")]
        public void ChangeSky()
        {
            StartCoroutine(SkyPhase2Change());
        }

        [ContextMenu("Change2Light")]
        public void ChangeSkyBack()
        {
            StartCoroutine(SkyPhase1Change());
        }

        private IEnumerator SkyPhase2Change()
        {
            Color fade = new Color(0, 0, 0, 0);
            for (float i = 0; i < phaseChangeDuration; i += Time.deltaTime)
            {

                skyboxMat.SetFloat("_Blend", i / phaseChangeDuration);

                //dirLightMain.color = Color.Lerp(dirLightMain.color, ph2MainLightColor, phaseChangeDuration * Time.deltaTime);
                //dirLightSub.color = Color.Lerp(dirLightSub.color, ph2SubLightColor, phaseChangeDuration * Time.deltaTime);

                //for (int x = 0; x < materialList.Count; x++) 
                //{
                //    materialList[x].material.color = Color.Lerp(materialList[x].material.color, fade, phaseChangeDuration * Time.unscaledDeltaTime);
                //}

                yield return null;
            }
        }
        
        private IEnumerator SkyPhase1Change()
        {
            for (float i = 1; i > 0; i -= Time.deltaTime)
            {

                skyboxMat.SetFloat("_Blend", i / phaseChangeDuration);

                //dirLightMain.color = Color.Lerp(dirLightMain.color, ph1MainLightColor, phaseChangeDuration * Time.deltaTime);
                //dirLightSub.color = Color.Lerp(dirLightSub.color, ph1SubLightColor, phaseChangeDuration * Time.deltaTime);

                //for (int x = 0; x < materialList.Count; x++)
                //{
                //    materialList[x].material.color = Color.Lerp(materialList[x].material.color, cloudColor, phaseChangeDuration * Time.unscaledDeltaTime);
                //}

                yield return null;
            }
        }

    }
}