using System.Collections;
using System.Collections.Generic;
using System.IO;
using RpgAdventure.Scripts.DataFromSceneToScena;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RpgAdventure.Scripts.MainMenuManager
{
    // class to manage Main Menu of the game
    public class MainMenuManager : MonoBehaviour
    {
        public GameObject MainMenu;
        public GameObject OptionMenu;
        public GameObject LoadGameMenu;
        public GameObject PlayerNameFormInput;
        public GameObject StartGamePlayerName;
        public GameObject Controls;
        public GameObject HeadquatersLetter;

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

        private string m_savedGameTime1;
        private string m_savedGameTime2;
        private string m_savedGameTime3;
        private string m_savedGameTime4;
        

        private void Awake()
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
            playerName = "Player";
            StartGameBtn.onClick.AddListener(StartGame);
            LoadGameBtn.onClick.AddListener(LoadGame);
            OptionsBtn.onClick.AddListener(LoadOptions);
            ExitBtn.onClick.AddListener(ExitMenu);
            ReturnFromLoadGameBtn.onClick.AddListener(ReturnFromLoadGame);
            ReturnFromOptionsBtn.onClick.AddListener(ReturnFromOptions);
            StartGameBtn2.onClick.AddListener(StartGame2);
            ReturnFromStartGame.onClick.AddListener(ReturnFromPlayerName);
            ReturnFromControlsBtn.onClick.AddListener(ReturnFromControls);
            ControlsBtn.onClick.AddListener(ShowControls);

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
            StartGamePlayerName.SetActive(false);
            HeadquatersLetter.SetActive(true);
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
            if (File.Exists("Assets/Saves/save1.data"))
            {
                m_savedGameTime1 = File.GetLastWriteTime("Assets/Saves/save1.data").ToString("dd/MM/yy HH:mm");
                GameObject.Find("Load1Btn").transform.GetChild(2).GetComponent<Text>().text = m_savedGameTime1;
            }

            if (File.Exists("Assets/Saves/save2.data"))
            {
                m_savedGameTime2 = File.GetLastWriteTime("Assets/Saves/save2.data").ToString("dd/MM/yy HH:mm");
                GameObject.Find("Load2Btn").transform.GetChild(2).GetComponent<Text>().text = m_savedGameTime2;
            }

            if (File.Exists("Assets/Saves/save3.data"))
            {
                m_savedGameTime3 = File.GetLastWriteTime("Assets/Saves/save3.data").ToString("dd/MM/yy HH:mm");
                GameObject.Find("Load3Btn").transform.GetChild(2).GetComponent<Text>().text = m_savedGameTime3;
            }

            if (File.Exists("Assets/Saves/save4.data"))
            {
                m_savedGameTime4 = File.GetLastWriteTime("Assets/Saves/save4.data").ToString("dd/MM/yy HH:mm");
                GameObject.Find("Load4Btn").transform.GetChild(2).GetComponent<Text>().text = m_savedGameTime4;
            }
        }
        private void LoadGameState1()
        {
            if (File.Exists("Assets/Saves/save1.data"))
            {
                DataManager.instance.loadingNumber = 1;
                SceneManager.LoadScene(1);
            }

        }
        private void LoadGameState2()
        {
            if (File.Exists("Assets/Saves/save2.data"))
            {
                DataManager.instance.loadingNumber = 2;
                SceneManager.LoadScene(1);
            }

        }
        private void LoadGameState3()
        {
            if (File.Exists("Assets/Saves/save3.data"))
            {
                DataManager.instance.loadingNumber = 3;
                SceneManager.LoadScene(1);
            }

        }
        private void LoadGameState4()
        {
            if (File.Exists("Assets/Saves/save4.data"))
            {
                DataManager.instance.loadingNumber = 4;
                SceneManager.LoadScene(1);
            }
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

