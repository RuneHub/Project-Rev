using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace KS
{
    public class FloatingUIManager : MonoBehaviour
    {
        public static FloatingUIManager instance;

        private Camera cam;

        [Header("Lockon")]
        public UIWorldLookAt LockOnObject;

        [Header("Damage Label Popup")]
        [SerializeField] private DamageLabel label;

        private ObjectPool<DamageLabel> dmgLabPool;

        [Header("Display Setup")]
        [Range(0.8f, 1.5f), SerializeField] public float displayLength = 1f;

        private void Awake()
        {
            cam = Camera.main;

            if (!instance)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            dmgLabPool = new ObjectPool<DamageLabel>(
                () =>
                {
                    DamageLabel dmgLab = Instantiate(label, transform);
                    dmgLab.Initialize(displayLength, this);
                    return dmgLab;
                },
                dmgLab => dmgLab.gameObject.SetActive(true),
                dmgLab => dmgLab.gameObject.SetActive(false)
            );

        }

        #region Lockon
        public void EnableLockOnVisual(Transform lockOnTarget)
        {
            LockOnObject.transform.gameObject.SetActive(true);
            LockOnObject.lookAt = lockOnTarget;
        }

        public void DisableSetLockOnVisual()
        {
            LockOnObject.transform.gameObject.SetActive(false);
        }
        #endregion

        #region Damage Popups

        public void DamageDone(int dmg, Vector3 pos, bool isCrit, Color dispayColor)
        {

            Vector3 screenPos = cam.WorldToScreenPoint(pos);
            screenPos.z = 0;
            bool dir = screenPos.x > Screen.width * 0.5f;

            SpawnDamagePopup(dmg, screenPos, dir, isCrit, dispayColor);
        }

        private void SpawnDamagePopup(int dmg, Vector3 pos, bool dir, bool isCrit, Color dispayColor)
        {
            DamageLabel dmgLab = dmgLabPool.Get();
            dmgLab.Display(dmg, pos, dir, isCrit, dispayColor);
        }

        public void ReturnDamageLabelToPool(DamageLabel dmgLab)
        {
            dmgLab.Reset();
            dmgLabPool.Release(dmgLab);
        }

        #endregion

    }
}