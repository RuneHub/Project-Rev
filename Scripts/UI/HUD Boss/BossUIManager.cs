using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KS
{
    public class BossUIManager : MonoBehaviour
    {
        [SerializeField] BossManager manager;

        [Header("Boss Vitality")]
        [SerializeField] public GameObject bossVitality;

        [SerializeField] public Slider bossHealthSlider;

        private void Awake()
        {
            manager = FindObjectOfType<BossManager>();

            bossHealthSlider = bossVitality.transform.Find("HealthBar").gameObject.GetComponent<Slider>();

            if (bossHealthSlider == null)
            {
                Debug.LogError("Boss health slider not found!");
            }
        }

        private void Update()
        {
            CheckBossVitality();
        }

        public void SetUpBossVitality()
        {
            bossHealthSlider.maxValue = manager.bossStat.maxHealth;
            bossHealthSlider.value = manager.bossStat.currentHealth;
        }

        private void CheckBossVitality()
        {
            bossHealthSlider.value = manager.bossStat.currentHealth;
        }

    }
}
