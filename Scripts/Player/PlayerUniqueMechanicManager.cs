using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class PlayerUniqueMechanicManager : MonoBehaviour
    {
        private PlayerManager manager;

        private PlayerUniqueUI uniqueUI;

        public enum MechLoadedLevel {lvl0, lvl1, lvl2, lvl3, lvl4 };

        [Header("Loaded")]
        public MechLoadedLevel loadedLevel = MechLoadedLevel.lvl0;
        const int maxGauge = 100;
        [Range(0, maxGauge)] public float LoadedGaugeLevel = 0;
        [SerializeField] private float IncreaseRate = 33.35f;

        [Header("timer")]
        [SerializeField] private float waitTime = 5;
        [SerializeField] private float totalTime = 30;
        [SerializeField] private float reduceAmount;

        [Header("Loaded Power")]
        public float loadedPower = 0;

        [Header("Loaded lvl 1")]
        public float loadedPowerlvl1 = 300;
        public StatusEffectsSO lvl1Buff;
        [Header("Loaded lvl 2")]
        public float loadedPowerlvl2 = 300;
        public StatusEffectsSO lvl2Buff;
        [Header("Loaded lvl 3")]
        public float loadedPowerlvl3 = 300;
        public StatusEffectsSO lvl3Buff;
        [Header("Loaded lvl 4")]
        public float loadedPowerlvl4 = 400;
        public StatusEffectsSO lvl4Buff;

        private bool SetLoaded;

        private void Awake()
        {
            manager = GetComponent<PlayerManager>();
            uniqueUI = manager.uniqueUI;

            uniqueUI.SetHud(maxGauge);

        }
        
        [ContextMenu("IncreaseGauge")]
        //Increases loaded level by the given rate, also Updates the UI.
        public void IncreaseLoadedGauge()
        {
            LoadedGaugeLevel += IncreaseRate;
            uniqueUI.UpdateGaugeAmount();
            //StartIntervalTime();
        }

        //reset Loaded Level, removes all buffs, resets loadedGauge
        public void ResetLoadedLevel()
        {
            LoadedGaugeLevel = 0; 
            SetLoadedLevel();
            manager.playerStats.RemoveStatusEffect(lvl1Buff);
            manager.playerStats.RemoveStatusEffect(lvl2Buff);
            manager.playerStats.RemoveStatusEffect(lvl3Buff);
            manager.playerStats.RemoveStatusEffect(lvl4Buff);
            uniqueUI.ResetGauge();
        }

        //update function
        public void UniqueMechUpdate()
        {
            CheckLoadedGaugeLevel();
            uniqueUI.UpdateGauge();
        }

        //checks the loaded Gauge, sets level depending on the gauge amount
        private void CheckLoadedGaugeLevel()
        {
            if (LoadedGaugeLevel == 0)
            {
                if (loadedLevel != MechLoadedLevel.lvl0)
                {
                    SetLoaded = false;
                }

                loadedLevel = MechLoadedLevel.lvl0;
            }
            else if (LoadedGaugeLevel < (maxGauge / 3))
            {
                if (loadedLevel != MechLoadedLevel.lvl1)
                {
                    SetLoaded = false;
                }

                loadedLevel = MechLoadedLevel.lvl1;
            }
            else if (LoadedGaugeLevel > (maxGauge / 3)
                && LoadedGaugeLevel < (maxGauge / 3 * 2))
            {
                if (loadedLevel != MechLoadedLevel.lvl2)
                {
                    SetLoaded = false;
                }

                loadedLevel = MechLoadedLevel.lvl2;
            }
            else if (LoadedGaugeLevel > (maxGauge / 3 * 2)
                && LoadedGaugeLevel < maxGauge)
            {
                if (loadedLevel != MechLoadedLevel.lvl3)
                {
                    SetLoaded = false;
                }

                loadedLevel = MechLoadedLevel.lvl3;
            }
            else if (LoadedGaugeLevel >= maxGauge)
            {
                if (loadedLevel != MechLoadedLevel.lvl4)
                {
                    SetLoaded = false;
                }
                loadedLevel = MechLoadedLevel.lvl4;
            }

            if (!SetLoaded)
            {
                SetLoadedLevel();
            }

        }

        //sets the setting for certain Loaded Levels.
        private void SetLoadedLevel()
        {
            switch (loadedLevel)
            {
                case MechLoadedLevel.lvl0:
                    loadedPower = 100;
                    break;
                case MechLoadedLevel.lvl1:
                    loadedPower =  loadedPowerlvl1;
                    manager.playerStats.AddStatusEffect(lvl1Buff);
                    break;
                case MechLoadedLevel.lvl2:
                    loadedPower = loadedPowerlvl2;
                    manager.playerStats.AddStatusEffect(lvl2Buff);
                    break;
                case MechLoadedLevel.lvl3:
                    loadedPower = loadedPowerlvl3;
                    manager.playerStats.AddStatusEffect(lvl3Buff);
                    break;
                case MechLoadedLevel.lvl4:
                    loadedPower = loadedPowerlvl4;
                    manager.playerStats.AddStatusEffect(lvl4Buff);
                    //manager.playerStats.AddStatusEffect(SolsticeBuff);
                    break;
            }

            SetLoaded = true;

        }


        private void StartIntervalTime()
        {
            StopCoroutine(RunIntervalTime());
            StartCoroutine(RunIntervalTime());
        }

        IEnumerator RunIntervalTime()
        {
            Debug.Log("Interval");
            yield return new WaitForSeconds(waitTime);

            float time = (totalTime / maxGauge) * LoadedGaugeLevel;
            Debug.Log("time: " + time);
            reduceAmount = (LoadedGaugeLevel / time);
            Debug.Log("ReduceAmoutnt: " +  reduceAmount);

            while (time > 0)//(LoadedGaugeLevel > 0)
            { 
                yield return new WaitForSeconds(1);
                Debug.Log("reducing");
                time--;
                LoadedGaugeLevel -= reduceAmount;

                CheckLoadedGaugeLevel();
                uniqueUI.UpdateGaugeAmount();
            }

            Debug.Log("Done");

        }

    }
}