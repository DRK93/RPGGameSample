using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NGS.ExtendableSaveSystem;
using RpgAdventure.Scripts.DataFromSceneToScena;
using RpgAdventure.Scripts.Helpers;
using RpgAdventure.Scripts.Player;

namespace RpgAdventure.Scripts.GameMenu
{
    // class to manage in game menu, save and load game
    public class GameMenuManager : MonoBehaviour
    {
        public GameObject gameMenuUI;
        public GameObject gameMenuPanel;
        public GameObject saveGamePanel;
        public GameObject loadGamePanel;
        public GameObject controlsPanel;
        public GameObject HUD_UI;
        public GameObject loadingScreenUI;
        public Button resumeBtn;
        public Button saveGameBtn;
        public Button loadGameBtn;
        public Button controlsBtn;
        public Button mainMenuBtn;
        public Button exitGameBtn;
        public Button saveReturnBtn;
        public Button loadReturnBtn;
        public Button controlsReturtnBtn;
        public bool m_IsLoadingScreenOff;

        private PauseControl m_pauseControl;
        private LoadingManager m_LoadingManager;
        private float m_TimeCounter = 0f;
        private float m_LoadingTime = 2.0f;
        private bool IsGameMenuActive;
        private string[] nameList = { "save1", "save2", "save3", "save4" };
        private string savedGameTime1;
        private string savedGameTime2;
        private string savedGameTime3;
        private string savedGameTime4;

        void Awake()
        {
            AudioListener.pause = true;
            HUD_UI.SetActive(true);
            m_LoadingManager = GetComponent<LoadingManager>();
            loadingScreenUI.SetActive(true);
            m_LoadingManager.SetLoadingFullTime(m_LoadingTime);

            resumeBtn.onClick.AddListener(ResumeBtn);
            saveGameBtn.onClick.AddListener(SaveGameBtn);
            loadGameBtn.onClick.AddListener(LoadGameBtn);
            controlsBtn.onClick.AddListener(ControlsBtn);
            mainMenuBtn.onClick.AddListener(MainMenuBtn);
            exitGameBtn.onClick.AddListener(ExitBtn);
            saveReturnBtn.onClick.AddListener(SaveGameReturnBtn);
            loadReturnBtn.onClick.AddListener(LoadGameReturnBtn);
            controlsReturtnBtn.onClick.AddListener(ControlReturnBtn);

            IsGameMenuActive = false;
            m_pauseControl = GetComponent<PauseControl>();
            StartCoroutine(StartAudio());
        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(0.5f);
            if (DataManager.instance != null)
            {
                WhichSaveStateLoaded(DataManager.instance.loadingNumber);
            }
        }
        void Update()
        {
            CheckLoadingScreenOff();
        }

        private void CheckLoadingScreenOff()
        {
            if (m_IsLoadingScreenOff == false)
            {
                if (m_TimeCounter <= m_LoadingTime)
                {
                    m_TimeCounter += Time.deltaTime;
                    m_LoadingManager.SetLoadTime(m_TimeCounter);
                }
                else
                {
                    m_IsLoadingScreenOff = true;
                    loadingScreenUI.SetActive(false);
                    m_pauseControl.StartGame();
                }
            }
        }
        private IEnumerator StartAudio()
        {
            yield return new WaitForSeconds(2.0f);
            AudioListener.pause = false;
        }

        public void GameMenuKey()
        {
            if (IsGameMenuActive == false)
            {
                GameMenuAcitvate();
                m_pauseControl.PauseGame();
            }
        }

        private void GameMenuAcitvate()
        {
            gameMenuUI.SetActive(true);
            gameMenuPanel.SetActive(true);
            IsGameMenuActive = true;
        }
        private void ResumeBtn()
        {
            m_pauseControl.StartGame();
            GameObject.Find("Player").GetComponent<PlayerInput>().IsAnyCardOpen = false;
            gameMenuUI.SetActive(false);
            IsGameMenuActive = false;

        }
        private void SaveGameBtn()
        {
            gameMenuPanel.SetActive(false);
            saveGamePanel.SetActive(true);
            foreach (var name in nameList)
                CheckIfSaveUpdated(name);
        }
        private void SaveGameReturnBtn()
        {
            saveGamePanel.SetActive(false);
            gameMenuPanel.SetActive(true);
        }
        private void LoadGameBtn()
        {
            m_pauseControl.StartGame();
            gameMenuPanel.SetActive(false);
            loadGamePanel.SetActive(true);
            foreach (var name in nameList)
                CheckIfLoadUpdated(name);
        }
        private void LoadGameReturnBtn()
        {
            loadGamePanel.SetActive(false);
            gameMenuPanel.SetActive(true);   
        }
        private void ControlsBtn()
        {
            gameMenuPanel.SetActive(false);
            controlsPanel.SetActive(true);
        }
        private void ControlReturnBtn()
        {
            controlsPanel.SetActive(false);
            gameMenuPanel.SetActive(true);
        }
        private void MainMenuBtn()
        {
            SceneManager.LoadScene(0);
        }
        private void ExitBtn()
        {
            Application.Quit();
        }
        private void WhichSaveStateLoaded(int number)
        {
            switch (number)
            {
                case 0:
                    break;
                case 1:
                    GameObject.Find("GameMenuManager").GetComponent<GameMaster>().LoadGame1();
                    break;
                case 2:
                    GameObject.Find("GameMenuManager").GetComponent<GameMaster>().LoadGame2();
                    break;
                case 3:
                    GameObject.Find("GameMenuManager").GetComponent<GameMaster>().LoadGame3();
                    break;
                case 4:
                    GameObject.Find("GameMenuManager").GetComponent<GameMaster>().LoadGame4();
                    break;
            }
        }

        public void CheckIfSaveUpdated(string savePath)
        {
            switch (savePath)
            {
                case "save1":
                    if (File.Exists("Assets/Saves/save1.data"))
                    {
                        savedGameTime1 = File.GetLastWriteTime("Assets/Saves/save1.data").ToString("dd/MM/yy HH:mm");
                        GameObject.Find("Save1Btn").transform.GetChild(2).GetComponent<Text>().text = savedGameTime1;
                    }
                    break;
                case "save2":
                    if (File.Exists("Assets/Saves/save2.data"))
                    {
                        savedGameTime2 = File.GetLastWriteTime("Assets/Saves/save2.data").ToString("dd/MM/yy HH:mm");
                        GameObject.Find("Save2Btn").transform.GetChild(2).GetComponent<Text>().text = savedGameTime2;
                    }
                    break;
                case "save3":
                    if (File.Exists("Assets/Saves/save3.data"))
                    {
                        savedGameTime3 = File.GetLastWriteTime("Assets/Saves/save3.data").ToString("dd/MM/yy HH:mm");
                        GameObject.Find("Save3Btn").transform.GetChild(2).GetComponent<Text>().text = savedGameTime3;
                    }
                    break;
                case "save4":
                    if (File.Exists("Assets/Saves/save4.data"))
                    {
                        savedGameTime4 = File.GetLastWriteTime("Assets/Saves/save4.data").ToString("dd/MM/yy HH:mm");
                        GameObject.Find("Save4Btn").transform.GetChild(2).GetComponent<Text>().text = savedGameTime4;
                    }
                    break;
                default:
                    break;
            }
        }
        private void CheckIfLoadUpdated(string savePath)
        {
            switch (savePath)
            {
                case "save1":
                    if (File.Exists("Assets/Saves/save1.data"))
                    {
                        savedGameTime1 = File.GetLastWriteTime("Assets/Saves/save1.data").ToString("dd/MM/yy HH:mm");
                        GameObject.Find("Load1Btn").transform.GetChild(2).GetComponent<Text>().text = savedGameTime1;
                    }
                    break;
                case "save2":
                    if (File.Exists("Assets/Saves/save2.data"))
                    {
                        savedGameTime2 = File.GetLastWriteTime("Assets/Saves/save2.data").ToString("dd/MM/yy HH:mm");
                        GameObject.Find("Load2Btn").transform.GetChild(2).GetComponent<Text>().text = savedGameTime2;
                    }
                    break;
                case "save3":
                    if (File.Exists("Assets/Saves/save3.data"))
                    {
                        savedGameTime3 = File.GetLastWriteTime("Assets/Saves/save3.data").ToString("dd/MM/yy HH:mm");
                        GameObject.Find("Load3Btn").transform.GetChild(2).GetComponent<Text>().text = savedGameTime3;
                    }
                    break;
                case "save4":
                    if (File.Exists("Assets/Saves/save4.data"))
                    {
                        savedGameTime4 = File.GetLastWriteTime("Assets/Saves/save4.data").ToString("dd/MM/yy HH:mm");
                        GameObject.Find("Load4Btn").transform.GetChild(2).GetComponent<Text>().text = savedGameTime4;
                    }
                    break;
                default:
                    break;

            }
        }
    }
}

