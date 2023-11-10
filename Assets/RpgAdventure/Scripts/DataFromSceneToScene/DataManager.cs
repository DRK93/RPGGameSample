using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure.Scripts.DataFromSceneToScena
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager instance;

        public string playerName = "Player";
        public int loadingNumber = 0;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
    }
}

