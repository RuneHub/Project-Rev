using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace KS { 
    public class ScreenManager : MonoBehaviour
    {
        public static ScreenManager instance;

        [SerializeField] private ScreenShatter Shatter;

        #region Damage effect
        [Header("Damage effect")]
        [SerializeField] private float damageDisplayTime = 1.5f;
        [SerializeField] private float damageFadeoutTime = 0.5f;
        [SerializeField] private AnimationCurve damageDisplayEffect;
        [SerializeField] private ScriptableRendererFeature fullscreenDamage;
        [SerializeField] private Material damageMaterial;
        private int damageIntensity = Shader.PropertyToID("_VignetteIntensity");
        #endregion

        #region Speed lines effect
        [Header("Speed lines effect")]
        public bool speedLinesON = false;
        [SerializeField] private float speedlinesFadeTime = 1.5f;
        [SerializeField] private AnimationCurve speedlinesDisplayShowEffect;
        [SerializeField] private AnimationCurve speedlinesDisplayFadeOutEffect;
        [SerializeField] private ScriptableRendererFeature fullscreenSpeedlines;
        [SerializeField] private Material speedLinesMaterial;
        private int speedlinesSize= Shader.PropertyToID("_MaskSize");
        #endregion

        #region Celestial
        [Header("Celestial effect")]
        public bool celestialON = false;

        [SerializeField] private float celestialFadeTime = 1.5f;
        [SerializeField] private AnimationCurve celestialDisplayShowEffect;
        [SerializeField] private AnimationCurve celestialDisplayFadeOutEffect;
        [SerializeField] private ScriptableRendererFeature fullscreenCelestial;
        [SerializeField] private Material celestialMaterial;
        private int celestialSize = Shader.PropertyToID("_MaskSize");
        #endregion

        private void Awake()
        {
            instance = this;

        }

        private void Start()
        {
            fullscreenDamage.SetActive(false);
            fullscreenSpeedlines.SetActive(false);
            fullscreenCelestial.SetActive(false);
        }

        #region Screen Shatter
        [ContextMenu("Shatter")]
        public void startShatter()
        {
            if (Shatter != null)
            {
                Shatter.startShatteringSequence();
            }
        }
        #endregion

        #region Hurt Effect
        public void StartFullscreenDamageSequence()
        {
            StartCoroutine(FullscreenDamage());
        }

        private IEnumerator FullscreenDamage()
        {
            fullscreenDamage.SetActive(true);
            //damageMaterial.SetFloat(damageIntensity, 7);

            float time = 0;
            float t = 0;
            while (time < damageDisplayTime)
            {
                time += Time.deltaTime;
                t = damageDisplayEffect.Evaluate(time);

                //Debug.Log("t: " + t);
                damageMaterial.SetFloat(damageIntensity, t);

            }

            yield return new WaitForSeconds(damageDisplayTime);

            float elapsed = 0;
            while (elapsed < damageFadeoutTime)
            {
                elapsed += Time.deltaTime;
                float lerp = Mathf.Lerp(t, 0, (elapsed / damageFadeoutTime));

                damageMaterial.SetFloat(damageIntensity, lerp);

                yield return null;
            }
            
            fullscreenDamage.SetActive(false);
        }
        #endregion

        #region Speeds lines Effect
        public void FullscreenSpeedlines(bool onoff)
        {
            speedLinesON = onoff;

            if (speedLinesON)
            {
                fullscreenSpeedlines.SetActive(true);
            }

            StartCoroutine(ShowSpeedLines());
            
            if (!speedLinesON)
            {

                fullscreenSpeedlines.SetActive(false);
            }

        }

        private IEnumerator ShowSpeedLines()
        {
            float time = 0;
            float t = 0;
            while (time < speedlinesFadeTime)
            {
                time += Time.deltaTime;
                if (speedLinesON)
                {
                    t = speedlinesDisplayShowEffect.Evaluate(time);
                }
                else
                {
                    t = speedlinesDisplayFadeOutEffect.Evaluate(time);
                }

                speedLinesMaterial.SetFloat(speedlinesSize, t);                
            }

            yield return new WaitForSeconds(speedlinesFadeTime);
        }

        #endregion

        #region Celestial Effect
        [ContextMenu("Celestial Effect")]
        public void FullscreenCelestial(float sizeAmount, float moveTime)
        {
            Debug.Log("FullscreenCelestial");
            celestialON = true;

            fullscreenCelestial.SetActive(true);

            StartCoroutine(ShowCelestial());

        }

        public void FullscreenCelestialOFF()
        {
            celestialON = false;
            StartCoroutine(ShowCelestial());
            fullscreenCelestial.SetActive(false);
        }

        private IEnumerator ShowCelestial()
        {
            float time = 0;
            float t = 0;
            while (time < celestialFadeTime)
            {
                time += Time.deltaTime;
                if (celestialON)
                {
                    t = celestialDisplayShowEffect.Evaluate(time);
                }
                else
                {
                    t = celestialDisplayFadeOutEffect.Evaluate(time);
                }

                celestialMaterial.SetFloat(celestialSize, t);
            }

            yield return new WaitForSeconds(celestialFadeTime);
        }

        #endregion

    }
}