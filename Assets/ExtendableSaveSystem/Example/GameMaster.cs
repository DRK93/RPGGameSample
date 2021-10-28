using System.Collections;
using System.Collections.Generic;
using System.IO;
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
            GetComponent<GameMenuManager>().CheckIfSaveUpdated("save1");
        }
        public void SaveGame2()
        {
            GetComponent<SaveMaster>().Save("Assets/Saves/", "save2", ".data");
            GetComponent<GameMenuManager>().CheckIfSaveUpdated("save2");
        }
        public void SaveGame3()
        {
            GetComponent<SaveMaster>().Save("Assets/Saves/", "save3", ".data");
            GetComponent<GameMenuManager>().CheckIfSaveUpdated("save3");
        }
        public void SaveGame4()
        {
            GetComponent<SaveMaster>().Save("Assets/Saves/", "save4", ".data");
            GetComponent<GameMenuManager>().CheckIfSaveUpdated("save4");
        }

        public void LoadGame1()
        {
            GetComponent<SaveMaster>().Load("Assets/Saves/", "save1", ".data");
        }
        public void LoadGame2()
        {
            GetComponent<SaveMaster>().Load("Assets/Saves/", "save2", ".data");
        }
        public void LoadGame3()
        {
            GetComponent<SaveMaster>().Load("Assets/Saves/", "save3", ".data");
        }
        public void LoadGame4()
        {
            GetComponent<SaveMaster>().Load("Assets/Saves/", "save4", ".data");
        }
    }
}
