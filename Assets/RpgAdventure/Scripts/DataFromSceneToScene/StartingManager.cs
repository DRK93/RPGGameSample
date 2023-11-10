using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace RpgAdventure.Scripts.DataFromSceneToScena
{
    public class StartingManager : MonoBehaviour
    {
        public GameObject PlayerNameTextOnUI;
        public Button beginBtn;

        private void Awake()
        {
            beginBtn.onClick.AddListener(BeginJourney);
        }
        private void Start()
        {
            TextMeshProUGUI playerNameOnLetter = GameObject.Find("StartingCanvas/Panel/PlayerName").GetComponent<TextMeshProUGUI>();
            playerNameOnLetter.text = GameObject.Find("DataManager").GetComponent<DataManager>().playerName;
        }

        private void BeginJourney()
        {
            SceneManager.LoadScene(1);
        }
    }
}

