using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace KSTween 
{
    public abstract class Tween<DriverValueType, ComponentType> : MonoBehaviour
    {
        internal ComponentType component { get; private set; }

        internal DriverValueType valueFrom = default;
        internal DriverValueType valueTo= default;
        internal DriverValueType valueCurrent = default;

        private float duration = 0;
        float time = 0;
        private bool didTimeReachEnd = false;

        private bool hasOnStart = false;
        private bool didTriggerOnStart = false;
        private Action onStart = null;

        private bool isPaused = false;
        private bool isWithdrawn = false;

        private bool isCompleted = false;
        private bool hasOnComplete = false;
        private Action onComplete = null;

        private bool hasOnCancel = false;
        private Action onCancel = null;

        public abstract void OnInit();

        public abstract void OnUpdate();

        internal Tween<DriverValueType, ComponentType> Finalize(DriverValueType _valueTo, float _duration)
        {
            if (!this.gameObject.activeInHierarchy)
            {
                this.Withdraw();
            }
            else
            {
                duration = _duration;
                valueTo = _valueTo;
            }
            return this;
        }

        internal float getDuration()
        {
            return duration;
        }

        internal float getElapsedTime()
        {
            return time;
        }

        private void Start()
        {
            if (!this.OnInitialize())
                this.Withdraw();

            this.OnInit();
        }

        private void Update()
        {
            if (this.isWithdrawn)
                return;
            if (this.isPaused)
                return;

            if (!didTriggerOnStart)
            {
                if (hasOnStart)
                {
                    this.onStart();
                }
                didTriggerOnStart = true;
            }

            var _timeDelta = Time.deltaTime / this.duration;
            this.time += _timeDelta;

            if (this.time > 1)
            {
                this.time = 1;
                didTimeReachEnd = true;
            }

            this.OnUpdate();

            if (didTimeReachEnd)
            {
                if (this.hasOnComplete)
                {
                    this.onComplete();
                }
                this.isCompleted = true;
                this.Withdraw();
            }
        }

        private void OnDisable()
        {
            this.Withdraw();
        }

        private void Withdraw()
        {
            this.isWithdrawn = true;
            UnityEngine.Object.Destroy(this);
        }

        public void Cancel()
        {
            if (hasOnCancel)
                this.onCancel();

            this.Withdraw();
        }

        public bool OnInitialize()
        {
            component = gameObject.GetComponent<ComponentType>();
            return component != null;
        }

        public Tween<DriverValueType, ComponentType> SetPaused(bool isPaused)
        {
            this.isPaused = isPaused;
            return this;
        }

        public Tween<DriverValueType, ComponentType> SetOnCancel(Action onCancel)
        {
            this.hasOnCancel = true;
            this.onCancel = onCancel;
            return this;
        }

        public Tween<DriverValueType, ComponentType> SetOnComplete(Action onComplete)
        {
            this.hasOnComplete = true;
            this.onComplete = onComplete;
            return this;
        }

        public async Task Await()
        {
            while (!this.isCompleted && !this.isWithdrawn)
            {
                if (!Application.isPlaying)
                    return;

                await Task.Yield();
            }
        }

        public IEnumerator Yield()
        {
            while (!this.isCompleted && !this.isWithdrawn)
            {
                yield return 0;
            }
        }

        internal static Driver Add<Driver>(GameObject gameObject) where Driver : Tween<DriverValueType, ComponentType> =>
            gameObject.AddComponent<Driver>();

        internal static Driver Add<Driver>(Component component) where Driver : Tween<DriverValueType, ComponentType> =>
            component.gameObject.AddComponent<Driver>();

    }

}