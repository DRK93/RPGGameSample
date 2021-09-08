using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RpgAdventure;

namespace NGS.ExtendableSaveSystem
{
    [RequireComponent(typeof(SaveMaster))]
    public class GameMaster : MonoBehaviour
    {
        public void SaveGame1()
        {
            GetComponent<SaveMaster>().Save("Assets/Saves/", "save1", ".data");
            Debug.Log("Game1 saved");
        }
        public void SaveGame2()
        {
            GetComponent<SaveMaster>().Save("Assets/Saves/", "save2", ".data");
            Debug.Log("Game2 saved");
        }
        public void SaveGame3()
        {
            GetComponent<SaveMaster>().Save("Assets/Saves/", "save3", ".data");
            Debug.Log("Game3 saved");
        }
        public void SaveGame4()
        {
            GetComponent<SaveMaster>().Save("Assets/Saves/", "save4", ".data");
            Debug.Log("Game4 saved");
        }

        public void LoadGame1()
        {
            GetComponent<SaveMaster>().Load("Assets/Saves/", "save1", ".data");
            Debug.Log("Game1 loaded");
            DestroyEnemyAfterLoad();
        }
        public void LoadGame2()
        {
            GetComponent<SaveMaster>().Load("Assets/Saves/", "save2", ".data");
            Debug.Log("Game2 loaded");
            DestroyEnemyAfterLoad();
        }
        public void LoadGame3()
        {
            GetComponent<SaveMaster>().Load("Assets/Saves/", "save3", ".data");
            Debug.Log("Game3 loaded");
            DestroyEnemyAfterLoad();
        }
        public void LoadGame4()
        {
            GetComponent<SaveMaster>().Load("Assets/Saves/", "save4", ".data");
            Debug.Log("Game4 loaded");
            DestroyEnemyAfterLoad();
        }

        public void DestroyEnemyAfterLoad()
        {

        }
    }
}
