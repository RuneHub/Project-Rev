using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{

    public class StartStormsEyeInteract : Interactible
    {
        public BMech_HPTrigger hpTrigger;
        public AirshipStatus airshipStatus;

        protected override void Awake()
        {
            base.Awake();

        }

        protected override void Start()
        {
            base.Start();
        }
        public override void Interact(PlayerManager player)
        {
            base.Interact(player);
            StartCoroutine(StartInteraction());
        }

        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
        }

        public override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
        }

        IEnumerator StartInteraction()
        {
            isInteractable = false;
            airshipStatus.SwapParts();
            yield return new WaitForSeconds(1);
            hpTrigger.PlayMechanic();
            yield return new WaitForSeconds(1);
            gameObject.SetActive(false);

        }

    }
}