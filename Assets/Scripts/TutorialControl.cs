using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts
{
    public class TutorialControl : MonoBehaviour
    {
        [SerializeField] private GameObject Panel_1;
        [SerializeField] private GameObject Panel_2;
        [SerializeField] private GameObject Panel_3;
        [SerializeField] private GameObject Panel_4;
        [SerializeField] private GameObject Panel_5;
        [SerializeField] private GameObject Panel_6;
        [SerializeField] private GameObject Panel_7;
        [SerializeField] private GameObject Panel_8;

        public void Awake()
        {
            Assert.IsNotNull(Panel_1, "Tutorial Panel 1 is unassigned in the TutorialControl game object/scrip");
            Assert.IsNotNull(Panel_2, "Tutorial Panel 2 is unassigned in the TutorialControl game object/scrip");
            Assert.IsNotNull(Panel_3, "Tutorial Panel 3 is unassigned in the TutorialControl game object/scrip");
            Assert.IsNotNull(Panel_4, "Tutorial Panel 4 is unassigned in the TutorialControl game object/scrip");
            Assert.IsNotNull(Panel_5, "Tutorial Panel 5 is unassigned in the TutorialControl game object/scrip");
            Assert.IsNotNull(Panel_6, "Tutorial Panel 6 is unassigned in the TutorialControl game object/scrip");
            Assert.IsNotNull(Panel_7, "Tutorial Panel 7 is unassigned in the TutorialControl game object/scrip");
            Assert.IsNotNull(Panel_8, "Tutorial Panel 8 is unassigned in the TutorialControl game object/scrip");

            StartPhase_1();
        }

        public void PauseForTutorial()
        {
            TutorialStateManager.isPaused = true;
            Time.timeScale = 0.00f;
        }

        public void UnPauseForTutorial()
        {
            TutorialStateManager.isPaused = false;
            Time.timeScale = 1.0f;
        }

        private void EventAggregatorForTutorial_GuySelected(object sender, System.EventArgs e)
        {
            EndPhase_2();
            StartPhase_3();
        }

        private void EventAggregator_BadGuyDied(object sender, BadGuyDiedEventArgs e)
        {
            EndPhase_4();
        }

        private void StartPhase_1()
        {
            Debug.Log("Start Phase 1");
            TutorialStateManager.isTutorialRunning = true;
            Panel_1.SetActive(true);
            TutorialStateManager.tutorialPhase = TutorialPhases.BadGuys;
            PauseForTutorial();
        }

        private void EndPhase_1()
        {
            Panel_1.SetActive(false);
            UnPauseForTutorial();
        }

        private void StartPhase_2()
        {
            Debug.Log("Start Phase 2");

            EventAggregatorForTutorial.GuySelected += EventAggregatorForTutorial_GuySelected;
            Panel_2.SetActive(true);
            TutorialStateManager.tutorialPhase = TutorialPhases.GoodGuys;
            PauseForTutorial();
        }

        private void EndPhase_2()
        {
            EventAggregatorForTutorial.GuySelected -= EventAggregatorForTutorial_GuySelected;
            Panel_2.SetActive(false);
            UnPauseForTutorial();
        }

        private void StartPhase_3()
        {
            Debug.Log("Start Phase 3");

            Panel_3.SetActive(true);
            PauseForTutorial();
        }

        private void EndPhase_3()
        {
            Panel_3.SetActive(false);
            UnPauseForTutorial();
        }
        private void StartPhase_4()
        {
            Debug.Log("Start Phase 4");

            EventAggregator.BadGuyDied += EventAggregator_BadGuyDied;
            Panel_4.SetActive(true);
            TutorialStateManager.tutorialPhase = TutorialPhases.WaitForBadGuyDeath;
        }

        private void EndPhase_4()
        {
            EventAggregator.BadGuyDied -= EventAggregator_BadGuyDied;
            Panel_4.SetActive(false);
            StartPhase_5();
        }

        private void StartPhase_5()
        {
            Debug.Log("Start Phase 5");
            Panel_5.SetActive(true);
            PauseForTutorial();
        }

        private void EndPhase_5()
        {
            Panel_5.SetActive(false);
        }

        private void StartPhase_6()
        {
            Debug.Log("Start Phase 6");
            Panel_6.SetActive(true);
        }

        private void EndPhase_6()
        {
            Panel_6.SetActive(false);
            StartPhase_7();
        }

        private void StartPhase_7()
        {
            Debug.Log("Start Phase 7");
            Panel_7.SetActive(true);
        }

        private void EndPhase_7()
        {
            Panel_7.SetActive(false);
            StartPhase_8();
        }

        private void StartPhase_8()
        {
            Debug.Log("Start Phase 8");
            Panel_8.SetActive(true);
        }

        private void EndPhase_8()
        {
            Panel_8.SetActive(false);
            TutorialStateManager.tutorialPhase = TutorialPhases.Completed;
            UnPauseForTutorial();
        }

        public void CompletePhase_1()
        {
            EndPhase_1();
            StartPhase_2();
        }

        public void CompletePhase_3()
        {
            EndPhase_3();
            StartPhase_4();
        }

        public void CompletePhase_5()
        {
            EndPhase_5();
            StartPhase_6();
        }
        public void CompletePhase_6()
        {
            EndPhase_6();
            StartPhase_7();
        }

        public void CompletePhase_7()
        {
            EndPhase_7();
            StartPhase_8();
        }

        public void CompletePhase_8()
        {
            EndPhase_8();
        }

    }
}
