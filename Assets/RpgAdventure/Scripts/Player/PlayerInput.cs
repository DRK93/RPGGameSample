using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RpgAdventure
{
    //class to manage Player Input from keyboard and mouse
    // check if Player could use abilities, which one Player want to use
    // check if Player could interact with something
    public class PlayerInput : MonoBehaviour
    {

        public bool isPlayerControllerInputBlock;
        public float distanceToInteract = 3.5f;
        public int spellNumber;
        [SerializeField]
        private GameObject useablbeManager;
        private UsableAbilities usableAbilities;
        private DialogManager m_DialogManger;
        private QuestGiver m_NPC;
        private PauseControl m_pauseControl;
        private static PlayerInput s_Instance;
        private Vector3 m_Movement;
        private bool m_IsSpell;
        private bool m_IsAttack;
        private bool m_IsBlock;
        private bool m_IsRoll;
        private bool m_IsJump;
        private bool m_IsLeftMouseClicked;
        private bool m_IsAnyCardOpen;
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
        public bool IsAnyCardOpen
        {
            set { m_IsAnyCardOpen = value; }
        }
        private void Awake()
        {
            s_Instance = this;
            m_IsAnyCardOpen = false;
        }
        private void Start()
        {
            usableAbilities = useablbeManager.GetComponent<UsableAbilities>();
            m_DialogManger = GameObject.Find("DialogManager").GetComponent<DialogManager>();
            m_pauseControl = GameObject.Find("GameMenuManager").GetComponent<PauseControl>();
        }

        void Update()
        {
            m_Movement.Set
            (
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
            );

            if (Input.anyKey)
            {
                bool isLeftMouseClick = Input.GetMouseButtonDown(0);
                bool isRightMouseClick = Input.GetMouseButtonDown(1);


                if (isLeftMouseClick)
                {
                    HandleLeftMouseBtnDown();
                    return;
                }

                if (isRightMouseClick)
                {
                    HandleRightMouseBtnDown();
                    return;
                }
                if (!PauseControl.gameIsPaused)
                {
                    HandleKeyboardKeys();
                }
            }
        }

        private void HandleLeftMouseBtnDown()
        {
            WasLeftMouseClicked();

            if (!m_IsAttack && !IsPointerOverUiElement() && (m_DialogManger.HasActiveDialog == false))
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
                if (!m_IsBlock && !IsPointerOverUiElement() && m_DialogManger.HasActiveDialog == false)
                    if (!s_Instance.OptionClickTarget.CompareTag("QuestGiver"))
                    {
                        StartCoroutine(TriggerBlock());
                    }
                    else
                    {
                        m_NPC = s_Instance.OptionClickTarget.GetComponent<QuestGiver>();
                        if (Vector3.Distance(s_Instance.transform.position, m_NPC.transform.position) > 2.5f)
                        {
                            StartCoroutine(TriggerBlock());
                        }
                        else
                        {
                            m_DialogManger.StartDialog(m_NPC);
                        }
                    }
            }
            else
            {
                if (!m_IsBlock && !IsPointerOverUiElement() && m_DialogManger.HasActiveDialog == false)
                    StartCoroutine(TriggerBlock());
            }
            
        }
        private void HandleKeyboardKeys()
        {
            
            bool isKeyForRollClick = Input.GetKeyDown(KeyCode.Q);
            bool isKeyForJumpClick = Input.GetKeyDown(KeyCode.Space);
            bool isKeyForAbility1 = Input.GetKeyDown(KeyCode.Alpha1);
            bool isKeyForAbility2 = Input.GetKeyDown(KeyCode.Alpha2);
            bool isKeyForAbility3 = Input.GetKeyDown(KeyCode.Alpha3);
            bool isKeyForAbility4 = Input.GetKeyDown(KeyCode.Alpha4);
            bool isKeyForAbility5 = Input.GetKeyDown(KeyCode.Alpha5);
            bool isKeyForAbility6 = Input.GetKeyDown(KeyCode.Alpha6);
            bool isKeyForInventory = Input.GetKeyDown(KeyCode.I);
            bool isGameMenuKey = Input.GetKeyDown(KeyCode.Escape);
            bool isCardStatsKey = Input.GetKeyDown(KeyCode.C);
            bool isKeyForQuestJournalClick = Input.GetKeyDown(KeyCode.J);
            if (usableAbilities.CanBeUsed == true)
            {
                if (isKeyForAbility1)
                    usableAbilities.UsingAbilityKeyBoard(1);
                if (isKeyForAbility2)
                    usableAbilities.UsingAbilityKeyBoard(2);
                if (isKeyForAbility3)
                    usableAbilities.UsingAbilityKeyBoard(3);
                if (isKeyForAbility4)
                    usableAbilities.UsingAbilityKeyBoard(4);
                if (isKeyForAbility5)
                    usableAbilities.UsingAbilityKeyBoard(5);
                if (isKeyForAbility6)
                    usableAbilities.UsingAbilityKeyBoard(6);
            }

            if (m_DialogManger.HasActiveDialog == false)
                {
                if (isKeyForRollClick)
                {
                    HandleKeyboardQKey();
                }

                if (isKeyForJumpClick)
                {
                    HandleKeyboardSpaceKey();
                }
                
                if (m_IsAnyCardOpen == false)
                {
                    if (isKeyForQuestJournalClick)
                    {
                        
                        HandleJournalKey();
                    }
                    if (isCardStatsKey)
                    {
                        HandlePlayerCardKey();
                    }
                    if (isGameMenuKey)
                    {
                        HandleMenuKey();
                    }
                    if(isKeyForInventory)
                    {
                        HandleInventoryKey();
                    }
                }
            }
        }
        private void HandleKeyboardQKey()
        {
            if (!m_IsRoll )
            {
                StartCoroutine(TriggerRoll());
            }
        }
        private void HandleKeyboardSpaceKey()
        {
            if (!m_IsJump )
            {
                StartCoroutine(TriggerJump());
            }
        }
        private void HandleJournalKey()
        {
            m_IsAnyCardOpen = true;
            FindObjectOfType<QuestManager>().QuestJournalUI.SetActive(true);
        }

        private void HandleInventoryKey()
        {
            GameObject.Find("InventoryManager").GetComponent<InventoryManager>().HideShowInventory();
        }

        private void HandleMenuKey()
        {
            m_IsAnyCardOpen = true;
            GameObject.Find("GameMenuManager").GetComponent<GameMenuManager>().GameMenuKey();
        }

        private void HandlePlayerCardKey()
        {
            m_IsAnyCardOpen = true;
            GameObject.Find("PlayerStatsManager").GetComponent<PlayerStatsManager>().CardStatsKey();
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
            yield return new WaitForSeconds(0.01f);
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
            yield return new WaitForSeconds(0.4f);
            m_IsJump = false;
        }
        private IEnumerator WasLeftMsClicked()
        {
            m_IsLeftMouseClicked = true;
            yield return new WaitForSeconds(0.1f);
            m_IsLeftMouseClicked = false;
        }
    }
}

