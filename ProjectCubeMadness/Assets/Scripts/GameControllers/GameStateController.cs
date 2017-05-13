using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    private static bool gameIsPaused;
    public static bool IsPaused {
        get { return gameIsPaused; }
        set {
            gameIsPaused = value;
            if(gameIsPaused)
                Time.timeScale = 0.0000000001f;
            else
                Time.timeScale = 1f;
        } }
}
