using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KS
{
    public class UIActivationOnSelected : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public BaseUIManager UIManager;
        public bool activate;
        public bool overlap;
        public bool deactivateOverlapTarget;

        public GameObject target;
        public List<GameObject> overlapTargets = new List<GameObject>();

        public void OnSelect(BaseEventData eventData)
        {
            if (UIManager != null) 
            {
                if (activate)
                {
                    for (int i = 0; i < overlapTargets.Count; i++)
                    {
                        if (!overlapTargets[i].activeSelf ||
                            overlap && overlapTargets[i].activeSelf)
                        {
                            target.SetActive(true);
                        }
                        else if (!overlap
                            && overlapTargets[i].activeSelf
                            && deactivateOverlapTarget)
                        {
                            overlapTargets[i].SetActive(false);
                            target.SetActive(true);
                        }
                    }

                }

               

            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            target.SetActive(false);
        }

    }
}