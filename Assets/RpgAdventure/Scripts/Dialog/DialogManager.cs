using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RpgAdventure
{
    // class to manage dialogs and dialogs UI whose are buttons with text
    // creating dialogs, remove quest option, check distance between Player and NPC
    public class DialogManager : MonoBehaviour
    {
        public GameObject dialogUI;
        public Text dialogHeaderText;
        public Text dialogAnswerText;
        public GameObject dialogOptionList;
        public Button dialogOptionPrefab;
        public GameObject InventoryUI;
        public Button dialogContinueBtn;
        public Button dialogQuitBtn;
        public GameObject dialogContinuePanel;
        public GameObject dialogQuitPanel;
        public Text testText4;
        public Text testText5;
        public Text testText6;
        public Text testText7;
        private GameObject m_Player;
        private QuestGiver m_NPC;
        private Dialog m_ActiveDialog;
        private float talkingRadius = 3.5f;
        private float m_DialogOptionTopPosition;
        private bool m_ForceDialogQuit;
        private bool m_isDialogOpened;

        const float c_DistanceBetweenOptions = 60.0f;

        public bool HasActiveDialog => m_ActiveDialog != null;
        public float DialogDistance;
    void Awake()
        {
            m_Player = GameObject.Find("Player");
            dialogContinueBtn.onClick.AddListener(ContinueDialogClick);
            dialogQuitBtn.onClick.AddListener(QuitDialogClick);
            m_ActiveDialog = null;
            m_isDialogOpened = false;
        }

        private void Update()
        {
            if (m_isDialogOpened == true)
            {
                DialogDistance = Vector3.Distance(
                m_Player.transform.position,
                m_NPC.transform.position);
                if (DialogDistance > talkingRadius + 1.0f)
                {
                    StopDialog();
                }
            }
        }

        public void StartDialog(QuestGiver NPC)
        {
            m_ActiveDialog = NPC.dialog;
            dialogHeaderText.text = NPC.name;
            m_NPC = NPC;
            m_isDialogOpened = true;
            dialogUI.SetActive(true);
            InventoryUI.SetActive(false);
            dialogQuitPanel.SetActive(false);
            ClearDialogOptions();
            DisplayAnswewrText(m_ActiveDialog.welcomeText);
        }

        private void StopDialog()
        {
            m_NPC = null;
            m_ActiveDialog = null;
            m_ForceDialogQuit = false;
            dialogUI.SetActive(false);
            m_isDialogOpened = false;
        }

        private void CreateDialogMenu()
        {
            m_DialogOptionTopPosition = 0;
            var dialogQueries = Array.FindAll(m_ActiveDialog.dialogQueries,
                dialogQuery => !dialogQuery.isAsked);
            var indexer = 0;
            foreach(var dialogQuery in dialogQueries)
            {
                m_DialogOptionTopPosition = c_DistanceBetweenOptions * indexer;
                var dialogOption = CreateDialogOption(dialogQuery.text);
                RegisterOptionClickHandler(dialogOption, dialogQuery);
                indexer++;
            }
        }

        private Button CreateDialogOption(string dialogOptionText)
        {
            Button buttonInstance = Instantiate(dialogOptionPrefab, dialogOptionList.transform);
            buttonInstance.GetComponentInChildren<Text>().text = dialogOptionText;

            RectTransform rt = buttonInstance.GetComponent<RectTransform>();
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, m_DialogOptionTopPosition, rt.rect.height);
            return buttonInstance;
        }
        private void RegisterOptionClickHandler(Button dialogOption, DialogQuery dialogQuery)
        {
            EventTrigger trigger = dialogOption.gameObject.AddComponent<EventTrigger>();
            var pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;

            pointerDown.callback.AddListener((e) =>
            {
                if (!String.IsNullOrEmpty(dialogQuery.answer.questId))
                {
                    m_Player.GetComponent<QuestLog>().AddQuest(m_NPC.quest);
                    m_NPC.QMarkAssigned = true;
                }
                if (dialogQuery.answer.forceDialogQuit)
                {
                    m_ForceDialogQuit = true;
                }

                if (!dialogQuery.isAlwaysAsked)
                {
                    dialogQuery.isAsked = true;
                }
                ClearDialogOptions();
                DisplayAnswewrText(dialogQuery.answer.text);
            });
            trigger.triggers.Add(pointerDown);
        }

        private void DisplayAnswewrText(string answerText)
        {
            dialogAnswerText.gameObject.SetActive(true);    
            dialogAnswerText.text = answerText;
            dialogContinuePanel.SetActive(true);
            dialogQuitPanel.SetActive(false);
        }

        private void DisplayDialogOptions()
        {
            HideAnswewrText();
            dialogContinuePanel.SetActive(false);
            dialogQuitPanel.SetActive(true);
            CreateDialogMenu();
        }
        private void HideAnswewrText()
        {
            dialogAnswerText.gameObject.SetActive(false);
        }

        private void ClearDialogOptions()
        {
            foreach(Transform child in dialogOptionList.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void ContinueDialogClick()
        {
            if (m_ForceDialogQuit)
            {
                StopDialog();
            }
            else
            {
                DisplayDialogOptions();
            }

        }

        private void QuitDialogClick()
        {
            StopDialog();
        }
        public void RemoveQuestOption(string questUID)
        {
            foreach (var questGiverNPC in GameObject.Find("QuestManager").GetComponent<QuestManager>().QuestGivers)
            {
                if (questGiverNPC.quest.uid == questUID)
                {
                    foreach (var dialogQuer in questGiverNPC.dialog.dialogQueries)
                    {
                        if (dialogQuer.answer.questId == questUID)
                        {
                            dialogQuer.isAsked = true;
                            m_Player.GetComponent<QuestLog>().AddQuest(questGiverNPC.quest);
                            questGiverNPC.QMarkAssigned = true;
                        }
                    }
                }
            }
        }
    }

}
