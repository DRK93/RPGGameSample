using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NGS.ExtendableSaveSystem;

namespace RpgAdventure
{
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

        private LoadingManager m_LoadingManager;
        private float m_TimeCounter = 0f;
        private float m_LoadingTime = 2.0f;
        private bool IsGameMenuKey;
        private bool IsGameMenuActive;

        void Awake()
        {
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
            IsGameMenuKey = false;
            
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
                }
            }

            IsGameMenuKey = Input.GetKeyDown(KeyCode.Escape);
            if (IsGameMenuKey && IsGameMenuActive == false)
            {
                gameMenuUI.SetActive(true);
                gameMenuPanel.SetActive(true);
                IsGameMenuActive = true;
            }
        }

        private void ResumeBtn()
        {
            gameMenuUI.SetActive(false);
            IsGameMenuActive = false;
        }
        private void SaveGameBtn()
        {
            gameMenuPanel.SetActive(false);
            saveGamePanel.SetActive(true);
        }
        private void SaveGameReturnBtn()
        {
            saveGamePanel.SetActive(false);
            gameMenuPanel.SetActive(true);
        }
        private void LoadGameBtn()
        {
            gameMenuPanel.SetActive(false);
            loadGamePanel.SetActive(true);
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
    }
}

