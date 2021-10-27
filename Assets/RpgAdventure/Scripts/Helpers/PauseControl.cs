using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class PauseControl : MonoBehaviour
    {
        public static bool gameIsPaused;

        public void PauseGame()
        {
            Time.timeScale = 0f;
            gameIsPaused = true;
            AudioListener.pause = true;
        }
        public void StartGame()
        {
            Time.timeScale = 1f;
            gameIsPaused = false;
            AudioListener.pause = false;
        }
    }
}

