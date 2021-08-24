using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RpgAdventure
{
    public class MainMenuManager : MonoBehaviour
    {
        public GameObject MainMenu;
        public GameObject OptionMenu;
        public GameObject LoadGameMenu;
        public GameObject PlayerNameFormInput;
        public GameObject StartGamePlayerName;
        public GameObject Controls;

        public Button StartGameBtn;
        public Button LoadGameBtn;
        public Button OptionsBtn;
        public Button ExitBtn;
        public Button ReturnFromLoadGameBtn;
        public Button ReturnFromOptionsBtn;
        public Button StartGameBtn2;
        public Button ReturnFromStartGame;
        public Button ControlsBtn;
        public Button ReturnFromControlsBtn;

        public TMPro.TMP_InputField InputPlayerName;
        public TMPro.TMP_InputField InputPlayerName2;
        public string playerName;
        
        
        private void Awake()
        {
            playerName = "Player";
            Button btn1 = StartGameBtn.GetComponent<Button>();
            Button btn2 = LoadGameBtn.GetComponent<Button>();
            Button btn3 = OptionsBtn.GetComponent<Button>();
            Button btn4 = ExitBtn.GetComponent<Button>();
            Button btn5 = ReturnFromLoadGameBtn.GetComponent<Button>();
            Button btn6 = ReturnFromOptionsBtn.GetComponent<Button>();
            Button btn7 = StartGameBtn2.GetComponent<Button>();
            Button btn8 = ReturnFromStartGame.GetComponent<Button>();
            Button btn9 = ReturnFromControlsBtn.GetComponent<Button>();
            Button btn10 = ControlsBtn.GetComponent<Button>();

            btn1.onClick.AddListener(StartGame);
            btn2.onClick.AddListener(LoadGame);
            btn3.onClick.AddListener(LoadOptions);
            btn4.onClick.AddListener(ExitMenu);
            btn5.onClick.AddListener(ReturnFromLoadGame);
            btn6.onClick.AddListener(ReturnFromOptions);
            btn7.onClick.AddListener(StartGame2);
            btn8.onClick.AddListener(ReturnFromPlayerName);
            btn9.onClick.AddListener(ReturnFromControls);
            btn10.onClick.AddListener(ShowControls);

            InputPlayerName2.text = this.playerName;
            InputPlayerName.onEndEdit.AddListener(SubmitName);
            InputPlayerName2.onEndEdit.AddListener(SubmitName);
        }

        private void SubmitName(string arg1)
        {
            Debug.Log(arg1);
            DataManager.instance.playerName = arg1;
            this.playerName = arg1;
        }
        public void StartGame()
        {
            Debug.Log("You clicked Start Game button");
            InputPlayerName2.text = this.playerName;
            MainMenu.SetActive(false);
            StartGamePlayerName.SetActive(true);
        }

        public void StartGame2()
        {
            Debug.Log("You clicked Start Game button2");
            SceneManager.LoadScene(1);
        }
        public void ReturnFromPlayerName()
        {
            Debug.Log("You click to return from Start Game - Player Name");
            StartGamePlayerName.SetActive(false);
            MainMenu.SetActive(true);
        }
        public void LoadGame()
        {
            Debug.Log("You clicked LoadGame button");
            MainMenu.SetActive(false);
            LoadGameMenu.SetActive(true);
        }
        public void ReturnFromLoadGame()
        {
            Debug.Log("You click to return from Load Game");
            LoadGameMenu.SetActive(false);
            MainMenu.SetActive(true);
        }

        public void LoadOptions()
        {
            Debug.Log("You clicked Options button");
            MainMenu.SetActive(false);
            OptionMenu.SetActive(true);   
        }

        public void ReturnFromOptions()
        {
            Debug.Log("You clicked to return from Options");
            OptionMenu.SetActive(false);
            MainMenu.SetActive(true);
        }

        public void ShowControls()
        {
            Debug.Log("You clicked to show cotnrols");
            MainMenu.SetActive(false);
            Controls.SetActive(true);
        }

        public void ReturnFromControls()
        {
            Debug.Log("You clicked to show cotnrols");
            Controls.SetActive(false);
            MainMenu.SetActive(true);
        }

        public void ExitMenu()
        {
            Debug.Log("You clicked Exit button");
            Application.Quit();
        }
    }
}

