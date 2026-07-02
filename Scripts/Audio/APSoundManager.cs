using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace KS
{
    public class APSoundManager : MonoBehaviour
    {
        public static APSoundManager Instance;
        IObjectPool<APSoundEmitter> soundEmitterPool;
        readonly List<APSoundEmitter> activeSoundEmitters = new();
        public readonly LinkedList<APSoundEmitter> frequentSoundEmitters = new();

        [SerializeField] APSoundEmitter soundEmitterPrefab;
        [SerializeField] bool collectionCheck = true;
        [SerializeField] int defaultCapacity = 10;
        [SerializeField] int maxPoolSize = 100;
        [SerializeField] int maxSoundInstances = 30;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            InitializePool();
        }

        private void InitializePool()
        {
            soundEmitterPool = new ObjectPool<APSoundEmitter>(
                CreateSoundEmitter,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                collectionCheck,
                defaultCapacity,
                maxPoolSize
                );
        }

        #region pool functions

        public APSoundEmitter Get() 
        {
            return soundEmitterPool.Get();
        }

        public void ReturnToPool(APSoundEmitter soundEmitter)
        {
            soundEmitterPool.Release(soundEmitter);
        }

        public bool CanPlaySound(APSoundData data)
        {
            if (!data.frequentSound) return true;

            if (frequentSoundEmitters.Count >= maxSoundInstances /* &&
                frequentSoundEmitters.TryDequeue(out var soundEmitter)*/)
            {
                try
                {
                    //soundEmitter.Stop();
                    frequentSoundEmitters.First.Value.Stop();
                    return true;
                }
                catch
                {
                    Debug.Log("SoundEmitter is already in Use");
                }
                return false;
            }

            return true;
        }

        public void StopAllSounds()
        {
            LinkedList<APSoundEmitter> tempList = new LinkedList<APSoundEmitter>(activeSoundEmitters);

            foreach (var se in tempList)
            {
                se.Stop();
            }

            frequentSoundEmitters.Clear();
        }

        public APSoundBuilder CreateSound() => new APSoundBuilder(this);

        APSoundEmitter CreateSoundEmitter()
        {
            var soundEmitter = Instantiate(soundEmitterPrefab);
            soundEmitter.gameObject.SetActive(false);
            return soundEmitter;
        }

        private void OnTakeFromPool(APSoundEmitter soundEmitter)
        {
            soundEmitter.gameObject.SetActive(true);
            activeSoundEmitters.Add(soundEmitter);
        }

        private void OnReturnedToPool(APSoundEmitter soundEmitter)
        {
            if (soundEmitter.Node != null)
            {
                frequentSoundEmitters.Remove(soundEmitter.Node);
                soundEmitter.Node = null;
            }

            soundEmitter.gameObject.SetActive(false);
            activeSoundEmitters.Remove(soundEmitter);
        }

        private void OnDestroyPoolObject(APSoundEmitter soundEmitter)
        {
            if(soundEmitter != null)
                Destroy(soundEmitter.gameObject);
        }

        #endregion

    }
}