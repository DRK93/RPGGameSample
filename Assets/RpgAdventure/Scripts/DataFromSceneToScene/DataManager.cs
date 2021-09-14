using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager instance;

        public string playerName;
        public int loadingNumber = 0;
        private void Awake()
        {
            playerName = "Player";
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

