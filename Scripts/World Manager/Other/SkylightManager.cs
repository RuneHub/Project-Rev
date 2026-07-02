using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class SkylightManager : MonoBehaviour
    {
        [SerializeField] private Material skyboxMat;
        [SerializeField] private Light lightMain;
        [SerializeField] private Light lightContrast;

        [Space(10), SerializeField] private Color ph1MainLightColor;
        [SerializeField] private Color ph2MainLightColor;
        [SerializeField] private Color ph1SubLightColor;
        [SerializeField] private Color ph2SubLightColor;

        [Space(10)]
        [SerializeField] private Color cloudColor;
        [SerializeField] private List<MeshRenderer> materialList = new List<MeshRenderer>();

        [SerializeField] private float phaseChangeDuration;

        private void Awake()
        {
            //ph1MainLightColor = lightMain.color;
            //ph1SubLightColor = lightContrast.color;

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

                lightMain.color = Color.Lerp(lightMain.color, ph2MainLightColor, phaseChangeDuration * Time.deltaTime);
                lightContrast.color = Color.Lerp(lightContrast.color, ph2SubLightColor, phaseChangeDuration * Time.deltaTime);

                //for (int x = 0; x < materialList.Count; x++)
                //{
                //    materialList[x].material.color = Color.Lerp(materialList[x].material.color, fade, phaseChangeDuration * Time.unscaledDeltaTime);
                //}

                DynamicGI.UpdateEnvironment();

                yield return null;
            }
        }
        
        private IEnumerator SkyPhase1Change()
        {
            for (float i = 1; i > 0; i -= Time.deltaTime)
            {

                skyboxMat.SetFloat("_Blend", i / phaseChangeDuration);

                lightMain.color = Color.Lerp(lightMain.color, ph1MainLightColor, phaseChangeDuration * Time.deltaTime);
                lightContrast.color = Color.Lerp(lightContrast.color, ph1SubLightColor, phaseChangeDuration * Time.deltaTime);

                //for (int x = 0; x < materialList.Count; x++)
                //{
                //    materialList[x].material.color = Color.Lerp(materialList[x].material.color, cloudColor, phaseChangeDuration * Time.unscaledDeltaTime);
                //}

                DynamicGI.UpdateEnvironment();

                yield return null;
            }
        }

    }
}