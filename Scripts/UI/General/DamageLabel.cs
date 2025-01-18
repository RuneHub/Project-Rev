using System.Collections;
using TMPro;
using UnityEngine;

namespace KS
{
    public class DamageLabel : MonoBehaviour
    {
        private FloatingUIManager dmgPoolManager;

        [Header("Damage pop-ups")]
        [SerializeField] private TMP_Text damageText;
        [SerializeField] private float normalFontSize = 42;
        [SerializeField] private float critFontSize = 52;
        [SerializeField] private Color normalFontColor = Color.white;

        [Header("Display")]
        [SerializeField] private float appearRadius = 300;
        private float displayDuration;
        [SerializeField] private float startColorFadeAtPercent = 0.8f;

        [Header("Typewriter")]
        [SerializeField] private float charSpeed = 1f;
        private WaitForSeconds simpleDelay;

        [Header("Animations")]
        [SerializeField] private AnimationCurve scaleCurve;
        private float time = 0;
        float fadeStartTime;

        private void Awake()
        {
            Reset();
            simpleDelay = new WaitForSeconds(charSpeed);
        }

        private void Update()
        {
           transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        }

        #region Damage popups
        public void Initialize(float _displayDuration, FloatingUIManager _dmgPoolManager)
        {
            dmgPoolManager = _dmgPoolManager;
            displayDuration = _displayDuration;

        }

        public void Display(int damage, Vector3 pos, bool _direction, bool isCrit, Color displayColor)
        {
            Vector3 offset = Random.insideUnitSphere * appearRadius;
            transform.position = pos + offset;

            damageText.SetText(damage.ToString()); 
            
            damageText.color = displayColor;
            damageText.enableVertexGradient = isCrit;
            damageText.fontSize = isCrit ? critFontSize : normalFontSize;
            if (isCrit)
                damageText.fontStyle = FontStyles.Underline;
            else
                damageText.fontStyle = FontStyles.Normal;

            StartCoroutine(Typewriter());
            StartCoroutine(OpacityLerp()); 
            StartCoroutine(ReturnDamageLabelToPool(displayDuration));
        }

        public void Reset()
        {
            damageText.maxVisibleCharacters = 0;
            time = 0;
            fadeStartTime = startColorFadeAtPercent * displayDuration;
        }

        private IEnumerator Typewriter()
        {

            foreach (char numb in damageText.text)
            {
                damageText.maxVisibleCharacters++;
                yield return simpleDelay;
            }
        }

        private IEnumerator OpacityLerp()
        {
            while (time < displayDuration)
            {
                if (time > fadeStartTime)
                {
                    Color color = damageText.color;
                    float newAlpa = Mathf.Lerp(1, 0, (time - fadeStartTime) / (displayDuration - fadeStartTime));
                    color.a = newAlpa;
                    damageText.color = color;
                }

                time += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator ReturnDamageLabelToPool(float displayLength)
        {
            yield return new WaitForSeconds(displayLength);
            dmgPoolManager.ReturnDamageLabelToPool(this);
        }

        #endregion

    }

}