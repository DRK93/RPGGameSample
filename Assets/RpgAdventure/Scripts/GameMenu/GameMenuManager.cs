using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RpgAdventure
{
    public class GameMenuManager : MonoBehaviour
    {
        public GameObject gameMenuUI;
        public GameObject gameMenuPanel;
        public GameObject saveGamePanel;
        public GameObject loadGamePanel;
        public GameObject controlsPanel;
        public Button resumeBtn;
        public Button saveGameBtn;
        public Button loadGameBtn;
        public Button controlsBtn;
        public Button mainMenuBtn;
        public Button exitGameBtn;
        public Button saveReturnBtn;
        public Button loadReturnBtn;
        public Button controlsReturtnBtn;

        private bool IsGameMenuKey;
        private bool IsGameMenuActive;

        void Awake()
        {
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

        void Update()
        {
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
    }
}

