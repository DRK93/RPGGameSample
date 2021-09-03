using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.ExtendableSaveSystem
{
    [RequireComponent(typeof(SaveMaster))]
    public class GameMaster : MonoBehaviour
    {
        public void SaveGame1()
        {
            GetComponent<SaveMaster>().Save("Assets/Saves/", "save1", ".data");
            Debug.Log("Game saved");
        }
        public void SaveGame2()
        {
            GetComponent<SaveMaster>().Save("Assets/Saves/", "save2", ".data");
            Debug.Log("Game saved");
        }
        public void SaveGame3()
        {
            GetComponent<SaveMaster>().Save("Assets/Saves/", "save3", ".data");
            Debug.Log("Game saved");
        }
        public void SaveGame4()
        {
            GetComponent<SaveMaster>().Save("Assets/Saves/", "save4", ".data");
            Debug.Log("Game saved");
        }

        public void LoadGame1()
        {
            GetComponent<SaveMaster>().Load("Assets/Saves/", "save1", ".data");
            Debug.Log("Game loaded");
        }
        public void LoadGame2()
        {
            GetComponent<SaveMaster>().Load("Assets/Saves/", "save2", ".data");
            Debug.Log("Game loaded");
        }
        public void LoadGame3()
        {
            GetComponent<SaveMaster>().Load("Assets/Saves/", "save3", ".data");
            Debug.Log("Game loaded");
        }
        public void LoadGame4()
        {
            GetComponent<SaveMaster>().Load("Assets/Saves/", "save4", ".data");
            Debug.Log("Game loaded");
        }
    }
}
