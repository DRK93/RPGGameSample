using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RpgAdventure
{
    public class PlayerInput : MonoBehaviour
    {

        public bool isPlayerControllerInputBlock;
        public float distanceToInteract = 2.5f;

        private static PlayerInput s_Instance;
        private Vector3 m_Movement;
        private bool m_IsSpell;
        private bool m_IsAttack;
        private bool m_IsBlock;
        private bool m_IsRoll;
        private bool m_IsJump;
        private bool m_IsLeftMouseClicked;
        private Collider m_OptionClickTarget;

        public Vector3 MoveInput
        {
            get
            {
                if (isPlayerControllerInputBlock)
                {
                    return Vector3.zero;
                }
                return m_Movement;
            }
        }
        public static PlayerInput Instance => s_Instance;
        public Collider OptionClickTarget => m_OptionClickTarget;
        public bool IsAttack => !isPlayerControllerInputBlock && m_IsAttack;
        public bool IsSpell => !isPlayerControllerInputBlock && m_IsSpell;
        public bool IsBlocking => !isPlayerControllerInputBlock && m_IsBlock;
        public bool IsRoll => m_IsRoll;
        public bool IsJump => m_IsJump;
        public bool IsLeftMouseClicked => m_IsLeftMouseClicked;
        public bool IsMoveInput => !Mathf.Approximately(MoveInput.magnitude, 0);

        private void Awake()
        {
            s_Instance = this;
        }

        void Update()
        {
            m_Movement.Set(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")
            );

            bool isLeftMouseClick = Input.GetMouseButtonDown(0);
            bool isRightMouseClick = Input.GetMouseButtonDown(1);
            bool isKeyForRollClick = Input.GetKeyDown(KeyCode.Q);
            bool isKeyForJumpClick = Input.GetKeyDown(KeyCode.Space);
            bool isKeyForQuestJournalClick = Input.GetKeyDown(KeyCode.J);
            bool isKeyForSpell = Input.GetKeyDown(KeyCode.Alpha2);

            if (isLeftMouseClick)
            {
                HandleLeftMouseBtnDown();
            }

            if(isRightMouseClick)
            {
                HandleRightMouseBtnDown();
            }
            if (isKeyForSpell)
            {
                HandleKeyboardSpellKey();
            }

            if (isKeyForRollClick)
            {
                HandleKeyboardQKey();
            }

            if (isKeyForJumpClick)
            {
                HandleKeyboardSpaceKey();
            }
            if (isKeyForQuestJournalClick)
            {
                FindObjectOfType<QuestManager>().QuestJournalUI.SetActive(true);
            }


        }

        private void HandleLeftMouseBtnDown()
        {
            WasLeftMouseClicked();

            if (!m_IsAttack && !IsPointerOverUiElement())
            {   
                StartCoroutine(TriggerAttack());
            }
        }

        private void WasLeftMouseClicked()
        {
            StartCoroutine(WasLeftMsClicked());
        }

        private bool IsPointerOverUiElement()
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            if (results.Count>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void HandleRightMouseBtnDown()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hasHit = Physics.Raycast(ray, out RaycastHit hit);

            if (hasHit)
            {
                m_OptionClickTarget = hit.collider;
                StartCoroutine(TriggerOptionTarget(hit.collider));
            }
            if (!m_IsBlock && !IsPointerOverUiElement())
            {
                StartCoroutine(TriggerBlock());
            }
        }

        private void HandleKeyboardQKey()
        {
            if (!m_IsRoll && !IsPointerOverUiElement())
            {
                StartCoroutine(TriggerRoll());
            }
        }
        private void HandleKeyboardSpaceKey()
        {
            if (!m_IsJump && !IsPointerOverUiElement())
            {
                StartCoroutine(TriggerJump());
            }
        }
        private void HandleKeyboardSpellKey()
        {
            if (!m_IsSpell && !IsPointerOverUiElement())
            {
                StartCoroutine(TriggerSpell());
            }
        }
        private IEnumerator TriggerOptionTarget(Collider other)
        {
            m_OptionClickTarget = other;
            yield return new WaitForSeconds(0.03f);
            m_OptionClickTarget = null;
        }
        private IEnumerator TriggerAttack()
        {
            m_IsAttack = true;
            yield return new WaitForSeconds(0.03f);
            m_IsAttack = false;
        }
        private IEnumerator TriggerBlock()
        {
            m_IsBlock = true;
            yield return new WaitForSeconds(0.03f);
            m_IsBlock = false;
        }
        private IEnumerator TriggerRoll()
        {
            m_IsRoll = true;
            yield return new WaitForSeconds(0.05f);
            m_IsRoll = false;
        }
        private IEnumerator TriggerJump()
        {
            m_IsJump = true;
            yield return new WaitForSeconds(0.05f);
            m_IsJump = false;
        }
        private IEnumerator TriggerSpell()
        {
            m_IsSpell = true;
            yield return new WaitForSeconds(0.05f);
            m_IsSpell = false;
        }
        private IEnumerator WasLeftMsClicked()
        {
            m_IsLeftMouseClicked = true;
            yield return new WaitForSeconds(0.1f);
            m_IsLeftMouseClicked = false;
        }
    }
}

