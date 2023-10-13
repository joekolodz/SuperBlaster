using UnityEngine;

public class StateManager : ScriptableObject
{
    public static bool isWaitingForNextLevelToStart = false;
    public static bool isPaused = false;
}

public enum TutorialPhases
{
    NotStarted,
    BadGuys,
    GoodGuys,
    WaitForBadGuyDeath,
    Base,
    Shields,
    Completed
}

public class TutorialStateManager : ScriptableObject
{
    public static bool isPaused = false;
    public static bool isTutorialRunning = false;
    public static TutorialPhases tutorialPhase = TutorialPhases.NotStarted;
}
