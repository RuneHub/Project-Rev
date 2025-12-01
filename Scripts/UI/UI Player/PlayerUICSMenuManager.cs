using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KS
{
    public class PlayerUICSMenuManager : BaseUIManager
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private CutsceneManager csManager;


        public override void OpenMenu()
        {
            UIManager.instance.menuWindowIsOpen = true;
            UIManager.instance.CSMenuIsOpen = true;

            base.OpenMenu();
            Time.timeScale = 0;

            csManager = playerManager.currentCSManager;

        }

        public override void CloseMenu()
        {
            UIManager.instance.menuWindowIsOpen = false;
            UIManager.instance.CSMenuIsOpen = false;
            base.CloseMenu();
            UIManager.instance.hudManager.ToggleHUD(false);
            UIManager.instance.guideBar.SetActive(false);

            Time.timeScale = 1;
        }


        public void SkipToEnd()
        {
            StartCoroutine(SkipTowardsPoint());
        }

        //The Sequence to skip the cutscene to a certain point, the actual skip line is in the "csManager" script.
        //fades out & in and sets the timescale back to 1.
        private IEnumerator SkipTowardsPoint()
        {
            //close menu
            CloseMenu();
            //fade to black screen.
            csManager.BlackScreenFadeOut();
            //wait for fade
            yield return new WaitForSeconds(csManager.GetCanvasFadingDuration());
            //skip to end of the Cutscene.
            csManager.SkipToPoint();
            //remove incutscene
            playerManager.InCutscene = false;
            //set timescale back to 1.
            Time.timeScale = 1;
            //fade back in.
            csManager.BlackScreenFadeIn();
            yield return new WaitForSeconds(csManager.GetCanvasFadingDuration());
        }

    }
}