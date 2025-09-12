using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace KS
{
    public class UIRebindSettings : MonoBehaviour
    {
        [Header("Reset Binding")]
        [SerializeField] private Selectable gamepadUI;
        [SerializeField] private Button resetAllButton;

        [SerializeField] private List<RebindUI> rebinds = new List<RebindUI>();

        private void OnEnable()
        {
            resetAllButton.onClick.AddListener(() => ResetAllToDefault());
        }

        private void Start()
        {
            //get all rebinds.
            for (int c = 0; c < transform.childCount; c++)
            {
                if (transform.GetChild(c).GetComponent<RebindUI>() != null)
                {
                    rebinds.Add(transform.GetChild(c).GetComponent<RebindUI>());
                }
            }

            UpdateAllButtonNavigation();
        }


        public void ResetAllToDefault()
        {
            //reset them individually.
            for (int i = 0; i < rebinds.Count; i++)
            {
                rebinds[i].ResetBinding();
            }

        }

        private void UpdateAllButtonNavigation()
        {
            RebindUI item;
            Navigation navi;

            for (int i = 0; i < rebinds.Count; i++)
            {
                item = rebinds[i];

                navi = item.gameObject.GetComponentInChildren<Button>().navigation;

                navi.selectOnUp = GetNavigationUp(i, rebinds.Count);
                navi.selectOnDown = GetNavigationDown(i, rebinds.Count);

                item.gameObject.GetComponentInChildren<Button>().navigation = navi;
            }

            AddResetToNavi();
            AddUIGamepadToNavi();
        }

        private Selectable GetNavigationUp(int index, int EntriesAmount)
        {
            if(index == 0)
                return null;

            RebindUI item;
            item = rebinds[index - 1].GetComponent<RebindUI>();

            return item.GetComponentInChildren<Selectable>();
        }

        private Selectable GetNavigationDown(int index, int EntriesAmount)
        {
            if (index == EntriesAmount -1)
                return null;

            RebindUI item;
            item = rebinds[index + 1].GetComponent<RebindUI>();

            return item.GetComponentInChildren<Selectable>();
        }

        private void AddResetToNavi()
        {
            Navigation navi;

            navi = resetAllButton.navigation;
            navi.selectOnUp = rebinds[rebinds.Count - 1].GetComponentInChildren<Selectable>();
            resetAllButton.navigation = navi;

            navi = rebinds[rebinds.Count - 1].GetComponentInChildren<Button>().navigation;
            navi.selectOnDown = resetAllButton.GetComponentInChildren<Selectable>();
            rebinds[rebinds.Count - 1].GetComponentInChildren<Button>().navigation = navi;

        }

        private void AddUIGamepadToNavi()
        {
            if (gamepadUI == null)
                return;

            Navigation navi;

            navi = gamepadUI.navigation;
            navi.selectOnDown = rebinds[0].GetComponentInChildren<Selectable>();
            gamepadUI.navigation = navi;

            navi = rebinds[0].GetComponentInChildren<Button>().navigation;
            navi.selectOnUp = gamepadUI.GetComponentInChildren<Selectable>();
            rebinds[0].GetComponentInChildren<Button>().navigation = navi;
        }
    }
}