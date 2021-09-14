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
        public Button LoadGame1Btn;
        public Button LoadGame2Btn;
        public Button LoadGame3Btn;
        public Button LoadGame4Btn;

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

            LoadGame1Btn.onClick.AddListener(LoadGameState1);
            LoadGame2Btn.onClick.AddListener(LoadGameState2);
            LoadGame3Btn.onClick.AddListener(LoadGameState3);
            LoadGame4Btn.onClick.AddListener(LoadGameState4);

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
            InputPlayerName2.text = this.playerName;
            MainMenu.SetActive(false);
            StartGamePlayerName.SetActive(true);
        }

        public void StartGame2()
        {
            SceneManager.LoadScene(1);
        }
        public void ReturnFromPlayerName()
        {
            StartGamePlayerName.SetActive(false);
            MainMenu.SetActive(true);
        }
        public void LoadGame()
        {
            MainMenu.SetActive(false);
            LoadGameMenu.SetActive(true);
        }
        private void LoadGameState1()
        {
            Debug.Log("Loading save game 1");
            DataManager.instance.loadingNumber = 1;
            SceneManager.LoadScene(1);
        }
        private void LoadGameState2()
        {
            Debug.Log("Loading save game 2");
            DataManager.instance.loadingNumber = 2;
            SceneManager.LoadScene(1);
        }
        private void LoadGameState3()
        {
            Debug.Log("Loading save game 3");
            DataManager.instance.loadingNumber = 3;
            SceneManager.LoadScene(1);
        }
        private void LoadGameState4()
        {
            Debug.Log("Loading save game 4");
            DataManager.instance.loadingNumber = 4;
            SceneManager.LoadScene(1);
        }
        public void ReturnFromLoadGame()
        {
            LoadGameMenu.SetActive(false);
            MainMenu.SetActive(true);
        }

        public void LoadOptions()
        {
            MainMenu.SetActive(false);
            OptionMenu.SetActive(true);   
        }

        public void ReturnFromOptions()
        {
            OptionMenu.SetActive(false);
            MainMenu.SetActive(true);
        }

        public void ShowControls()
        {
            MainMenu.SetActive(false);
            Controls.SetActive(true);
        }

        public void ReturnFromControls()
        {
            Controls.SetActive(false);
            MainMenu.SetActive(true);
        }

        public void ExitMenu()
        {
            Application.Quit();
        }
    }
}

