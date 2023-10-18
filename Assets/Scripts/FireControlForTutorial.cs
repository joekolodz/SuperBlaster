using UnityEngine;

namespace Assets.Scripts
{
    public class FireControlForTutorial : FireControl
    {
        protected override void Awake()
        {
            base.Awake();
            UnselectAllGuys();
        }

        protected override void SelectGuy(GoodGuyManager guy)
        {
            base.SelectGuy(guy);
            if (_currentlySelectedGuy != null && TutorialStateManager.tutorialPhase == TutorialPhases.GoodGuys)
            {
                EventAggregatorForTutorial.PublishGuySelected();
            }
        }

        protected override void FireARocket(GoodGuyManager guyFiring, Vector3 mousePos)
        {
            if (TutorialStateManager.isPaused) return;
            base.FireARocket(guyFiring, mousePos);
        }
    }
}
