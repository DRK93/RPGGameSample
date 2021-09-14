using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RpgAdventure
{
    public class StartingManager : MonoBehaviour
    {

        public GameObject StartingUI;
        public GameObject InventoryUI;
        public GameObject HudUI;
        public GameObject PlayerNameTextOnUI;

        public Button beginBtn;

        private void Awake()
        {
            Button btn1 = beginBtn.GetComponent<Button>();
            btn1.onClick.AddListener(BeginJourney);
        }
        private void Start()
        {
            TextMeshProUGUI playerNameOnLetter = GameObject.Find("StartingCanvas/Panel/PlayerName").GetComponent<TextMeshProUGUI>();
            playerNameOnLetter.text = DataManager.instance.playerName;
            
        }

        private void BeginJourney()
        {
            StartingUI.SetActive(false);
            InventoryUI.SetActive(false);
            HudUI.SetActive(true);
        }
    }
}

