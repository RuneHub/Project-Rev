using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KS
{
    public class PlayerUISelectButton : MonoBehaviour
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            button.Select();
            // .Select() might not fire "OnSelect" in older Unity versions, this fixes that. might be removed depending on what Unity version this is used in.
            button.OnSelect(null); 
        }

    }
}