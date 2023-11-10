using System;
using System.Collections;
using System.Collections.Generic;
using RpgAdventure.Scripts.Audio;
using RpgAdventure.Scripts.CamerasLogic;
using RpgAdventure.Scripts.DamageSystem;
using RpgAdventure.Scripts.Events;
using RpgAdventure.Scripts.Helpers;
using RpgAdventure.Scripts.Hud;
using RpgAdventure.Scripts.Inventory;
using RpgAdventure.Scripts.Weapons;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace RpgAdventure.Scripts.Player
    {
    //class manage Player character behaviour - especially animator input
    //compute movement, choose animation for particular ability and receive information from fighting mechanic
    public class PlayerController : MonoBehaviour, IAttackAnimListener, IMessageReceiver
    {
        public static PlayerController Instance
        {
            get
            {
                return s_Instance;
            }
        }

        public bool IsRespawning { get { return m_IsRespawning; } }

        public MeleeWeapon meleeWeapon;
        public float maxForwardSpeed = 8.0f;
        public float rotationSpeed;
        public float m_MaxRotationSpeed = 1200;
        public float m_MinRotationSpeed = 400;
        public float gravity = 20.0f;
        public float groundDistance = 0.2f;
        public LayerMask whatIsGround;
        public Transform weaponHand;
        public RandomAudioPlayer sprintAudio;

        private static PlayerController s_Instance;
        private Animator m_Animator;
        private PlayerInput m_PlayerInput;
        private Damageable m_Damageable;
        private CharacterController m_CHController;
        private CameraController m_CameraController;
        private HudManager m_HudManager;
        private Quaternion m_TargetRotation;

        private AnimatorStateInfo m_CurrentStateInfo;
        private AnimatorStateInfo m_NextStateInfo;
        private bool m_IsAnimatorTransisioning;
        private bool m_IsRespawning;
        private bool m_IsPossibleToTempoAttack;


        private float m_DesiredForwardSpeed;
        private float m_ForwardSpeed;
        private float m_VerticalSpeed;
        private float m_CurrentJumpingSpeed;
        private Vector3 m_StartingPosition;

        const float k_Acceleration = 20f;
        const float k_Deceleration = 35.0f;
        
        private readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        private readonly int m_HashMeleeAttack = Animator.StringToHash("MeleeAttack");
        private readonly int m_HashDeath = Animator.StringToHash("Death");
        private readonly int m_HashFootFall = Animator.StringToHash("FootFall");
        private readonly int m_HashBlockAttack = Animator.StringToHash("BlockingStance");
        private readonly int m_HashBlockingHit = Animator.StringToHash("BlockingEnemyHit");
        private readonly int m_HashGetDmgLight = Animator.StringToHash("GetDmgLight");
        private readonly int m_HashGetDmgHeavy = Animator.StringToHash("GetDmgHeavy");
        private readonly int m_HashRoll = Animator.StringToHash("Roll");
        private readonly int m_HashJump = Animator.StringToHash("Jump");
        private readonly int m_HashAttackTempoTrigger = Animator.StringToHash("AttackTempo");
        private readonly int m_HashSpell = Animator.StringToHash("SpellAttack");
        private readonly int m_HashSpell2 = Animator.StringToHash("SpellAttack2");
        private readonly int m_HashSpell3 = Animator.StringToHash("SpellAttack3");
        private readonly int m_HashSpell4 = Animator.StringToHash("SpellAttack4");
        private readonly int m_HashBlockInput = Animator.StringToHash("BlockInput");
        private readonly int m_HashJumping = Animator.StringToHash("Jumping");
        private readonly int m_HashFalling = Animator.StringToHash("Falling");
        private readonly int m_HashGrounded = Animator.StringToHash("Grounded");
        private readonly int m_HashIsDead = Animator.StringToHash("IsDead");

        private void Awake()
        {
            m_StartingPosition = transform.position;
            m_CHController = GetComponent<CharacterController>();
            m_PlayerInput = GetComponent<PlayerInput>();
            m_Animator = GetComponent<Animator>();
            m_CameraController = Camera.main.GetComponent<CameraController>();
            m_HudManager = FindObjectOfType<HudManager>();
            m_Damageable = GetComponent<Damageable>();
            s_Instance = this;
            m_HudManager.SetMaxHealth(m_Damageable.GetComponent<PlayerStats>().maxHitPoints, m_Damageable.GetComponent<PlayerStats>().currentHitPoints);
        }
        void FixedUpdate()
        {
            CacheAnimationState();
            UpdateInputBlocking();
            ComputeForwardMovement();
            ComputeVerticalMovement();
            ComputeRotation();
            ComputeIfJump();
            CheckAnimatorTriggers();
            PlaySprintAudio();
        }

        private void CacheAnimationState()
        {
            m_CurrentStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            m_NextStateInfo = m_Animator.GetNextAnimatorStateInfo(0);
            m_IsAnimatorTransisioning = m_Animator.IsInTransition(0);
        }
        private void UpdateInputBlocking()
        {
            bool inputBlocked = m_CurrentStateInfo.tagHash == m_HashBlockInput && !m_IsAnimatorTransisioning;
            inputBlocked = inputBlocked | m_NextStateInfo.tagHash == m_HashBlockInput;
            m_PlayerInput.isPlayerControllerInputBlock = inputBlocked;
        }
        private void ComputeForwardMovement()
        {
            Vector3 moveInput = m_PlayerInput.MoveInput.normalized;
            m_DesiredForwardSpeed = moveInput.magnitude * maxForwardSpeed;
            float accelaration;
            if (m_PlayerInput.IsMoveInput || (m_Animator.GetBool("Jumping") == true) || (m_Animator.GetBool("Falling") == true))
            {
                accelaration = k_Acceleration;
            }
            else
            {
                accelaration = k_Deceleration;
            }
            m_ForwardSpeed = Mathf.MoveTowards(
                m_ForwardSpeed,
                m_DesiredForwardSpeed,
                Time.fixedDeltaTime * accelaration
                );
            if (CheckWhatIsBelow() == "WaterPlane")
            {
                transform.position = transform.position - transform.forward * 0.5f;
            }
            m_Animator.SetFloat(m_HashForwardSpeed, m_ForwardSpeed);
        }

        private string CheckWhatIsBelow()
        {
            RaycastHit rayCastHit1;
            if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), Vector3.down, out rayCastHit1, 40))
            {
                return rayCastHit1.transform.name;
            }
            else
                return "Nothing below, good luck at falling down!!!";
        }
        private void ComputeVerticalMovement()
        {

            if (m_Animator.GetBool(m_HashJumping) == true)
            {

                m_VerticalSpeed = 0.3f * m_CurrentJumpingSpeed;
                m_CurrentJumpingSpeed -= 200 * Time.deltaTime;
            }
            else
            {
                m_VerticalSpeed = -0.2f * gravity;
            }
        }

        private void ComputeRotation()
        {
            Vector3 moveInput = m_PlayerInput.MoveInput.normalized;
            Vector3 cameraDirection = Quaternion.Euler(
                0,
                m_CameraController.PlayerCam.m_XAxis.Value,
                0
                ) * Vector3.forward;
            Quaternion targetRotation;
            if (Mathf.Approximately(Vector3.Dot(moveInput, Vector3.forward), -1.0f))
            {
                targetRotation = Quaternion.LookRotation(-cameraDirection);
            }
            else
            {
                Quaternion movementRotation = Quaternion.FromToRotation(Vector3.forward, moveInput);
                targetRotation = Quaternion.LookRotation(movementRotation * cameraDirection);
            }
            m_TargetRotation = targetRotation;
        }

        private void ComputeIfJump()
        {
            if (m_PlayerInput.IsMoveInput || m_PlayerInput.IsJump)
            {
                float rotationSpeed = Mathf.Lerp(m_MaxRotationSpeed, m_MinRotationSpeed, m_ForwardSpeed / m_DesiredForwardSpeed);
                m_TargetRotation = Quaternion.RotateTowards
                    (transform.rotation,
                    m_TargetRotation,
                    rotationSpeed * Time.fixedDeltaTime
                    );
                transform.rotation = m_TargetRotation;
            }
        }

        private void CheckAnimatorTriggers()
        {
            m_Animator.ResetTrigger(m_HashMeleeAttack);

            if (m_PlayerInput.IsAttack)
            {
                m_Animator.SetTrigger(m_HashMeleeAttack);
            }

            if (m_PlayerInput.IsLeftMouseClicked && m_IsPossibleToTempoAttack)
            {
                StartCoroutine(DelayForFirstAttack());
            }

            //m_Animator.ResetTrigger(m_HashBlockAttack);
            if (m_PlayerInput.IsBlocking)
            {
                m_Animator.SetTrigger(m_HashBlockAttack);
            }

            m_Animator.ResetTrigger(m_HashRoll);
            if (m_PlayerInput.IsRoll)
            {
                m_Animator.SetTrigger(m_HashRoll);
            }

            if ((m_Animator.GetBool(m_HashJumping) == false) && (m_Animator.GetBool(m_HashFalling) == false))
            {
                m_Animator.ResetTrigger(m_HashJump);
                m_CurrentJumpingSpeed = 0;
            }

            if (m_PlayerInput.IsJump)
            {
                m_Animator.SetTrigger(m_HashJump);
                m_CurrentJumpingSpeed = 8;
            }

            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, groundDistance, whatIsGround))
            {
                m_Animator.SetBool(m_HashGrounded, true);
                m_Animator.applyRootMotion = true;
            }
            else
            {
                m_Animator.SetBool(m_HashGrounded, false);
            }
        }


        private void OnAnimatorMove()
        {
            if (m_IsRespawning) { return; }
            Vector3 movement = m_Animator.deltaPosition;
            movement += m_VerticalSpeed * Vector3.up * Time.fixedDeltaTime;
            if (m_Animator.GetBool(m_HashJumping) == true)
            {
                movement += 1 * m_ForwardSpeed * transform.forward * Time.fixedDeltaTime;
            }
            if (m_Animator.GetBool(m_HashFalling) == true)
            {
                movement += 0.5f * m_ForwardSpeed * transform.forward * Time.fixedDeltaTime;
            }
            m_CHController.Move(movement);
        }
        public void OnReceiveMessage(MessageType type, object sender, object message)
        {
            if (type == MessageType.DAMAGED)
            {
                m_HudManager.SetHealth((sender as Damageable).CurrentHitPoints);
                m_Animator.SetTrigger(m_HashGetDmgLight);
            }
            if (type == MessageType.HIGHDAMAGED)
            {
                m_HudManager.SetHealth((sender as Damageable).CurrentHitPoints);
                m_Animator.SetTrigger(m_HashGetDmgHeavy);
            }
            if (type == MessageType.DEAD)
            {
                m_IsRespawning = true;
                m_Animator.SetTrigger(m_HashDeath);
                m_Animator.SetBool(m_HashIsDead, true);
                m_HudManager.SetHealth(0);
            }
            if (type == MessageType.BLOCKED)
            {
                m_Animator.SetTrigger(m_HashBlockingHit);
            }
        }
        public void MeleeAttackStart()
        {
            if (meleeWeapon != null)
            {
                meleeWeapon.BeginAttack();
            }

        }
        public void MeleeAttackEnd()
        {
            if (meleeWeapon != null)
            {
                meleeWeapon.EndAttack();
            }

        }

        public void TempoWindowBegin()
        {
            m_IsPossibleToTempoAttack = true;
        }
        private IEnumerator DelayForFirstAttack()
        {
            yield return new WaitForSeconds(0.1f);
            if (m_PlayerInput.IsLeftMouseClicked && m_IsPossibleToTempoAttack)
            {
                m_Animator.SetTrigger(m_HashAttackTempoTrigger);
            }
        }
        public void TempoWindowEnd()
        {
            m_IsPossibleToTempoAttack = false;
        }

        public void StartRespawn()
        {
            transform.position = m_StartingPosition;
            m_HudManager.SetHealth(m_Damageable.GetComponent<PlayerStats>().maxHitPoints);
            m_Damageable.SetInitialHealth();
        }

        public void UseHealthPotion(int powerOfPotion)
        {
            m_Damageable.ApplyPotion(powerOfPotion);
            m_HudManager.SetHealth(m_Damageable.CurrentHitPoints);
        }

        public void FinishRespawn()
        {
            m_IsRespawning = false;
        }

        public void StartBlocking()
        {
            m_Damageable.BlockStance = true;
            if (meleeWeapon != null)
                meleeWeapon.blockStanceAudio.PlayRandomClip();
        }

        public void FinishBlocking()
        {
            m_Damageable.BlockStance = false;
        }

        public void UseItemFrom(InventorySlot inventorySlot)
        {
            if (meleeWeapon != null)
            {
                if (inventorySlot.itemPrefab.name == meleeWeapon.name)
                {
                    return;
                }
                else
                {
                    Destroy(meleeWeapon.gameObject);
                }
            }
            meleeWeapon = Instantiate(inventorySlot.itemPrefab, transform).GetComponent<MeleeWeapon>();
            meleeWeapon.GetComponent<FixedUpdateFollow>().SetFollowee(weaponHand);
            meleeWeapon.name = inventorySlot.itemPrefab.name;
            meleeWeapon.SetOwner(gameObject);
        }

        public void UseAbility(int abilityNumber)
        {
            switch(abilityNumber)
            {
                case 1:
                    m_PlayerInput.spellNumber = abilityNumber;
                    m_Animator.SetTrigger(m_HashSpell);
                    break;
                case 2:
                    m_PlayerInput.spellNumber = abilityNumber;
                    m_Animator.SetTrigger(m_HashSpell2);
                    break;
                case 3:
                    m_PlayerInput.spellNumber = abilityNumber;
                    m_Animator.SetTrigger(m_HashSpell3);
                    break;
                case 4:
                    m_PlayerInput.spellNumber = abilityNumber;
                    m_Animator.SetTrigger(m_HashSpell4);
                    break;
                case 5:
                    break;
                case 6:
                    break;
                default:
                    break;
            }
        }
        private void PlaySprintAudio()
        {
            float footFallCurve = m_Animator.GetFloat(m_HashFootFall);
            if (footFallCurve > 0.01f && !sprintAudio.isPlaying && sprintAudio.canPlay)
            {
                sprintAudio.isPlaying = true;
                sprintAudio.canPlay = false;
                sprintAudio.PlayRandomClip();
            }
            else if (sprintAudio.isPlaying)
            {
                sprintAudio.isPlaying = false;
            }
            else if (footFallCurve < 0.01f && !sprintAudio.canPlay)
            {
                sprintAudio.canPlay = true;
            }
        }
    }

    //-------------------------------------------------------
    //Simple moving with character body deployed
    /*
    Vector3 moveInput = m_PlayerInput.MoveInput;
    Quaternion camRotation = m_MainCamera.transform.rotation;
    Vector3 targetDirection = camRotation * moveInput;
    targetDirection.y = 0;

    m_CHController.Move(targetDirection.normalized * speed * Time.fixedDeltaTime);
    m_CHController.transform.rotation = Quaternion.Euler(0, camRotation.eulerAngles.y, 0);*/


    // -----------------------------------------------------------
    // Working camera with custom made vectors
    /*
    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");

    Vector3 dir = Vector3.zero;
    dir.x = Input.GetAxis("Horizontal");
    dir.z = Input.GetAxis("Vertical");

    if (dir == Vector3.zero)
    {
        return;
    }

    Vector3 camDirection = cam.transform.rotation * dir;
    Vector3 targetDirection = new Vector3(camDirection.x, 0, camDirection.z);

    if (dir.z >= 0)
    {
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            Quaternion.LookRotation(targetDirection),
            0.1f);
    }
    m_Rb.MovePosition(m_Rb.position + targetDirection.normalized * speed * Time.fixedDeltaTime); 




        private Animator m_Animator;
    private Rigidbody m_Rb;
    public float jumpForce = 500;
    public float groundDistance = 0.3f;
    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rb = GetComponent<Rigidbody>();
        //m_Animator.SetTrigger("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        m_Animator.SetFloat("Speed", v, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("TurningSpeed", h, 0.1f, Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_Rb.AddForce(Vector3.up * jumpForce);
            m_Animator.SetTrigger("Jump");
        }

        if (Physics.Raycast (transform.position + (Vector3.up *0.1f), Vector3.down, groundDistance, whatIsGround))
        {
            m_Animator.SetBool("Grounded", true);
            m_Animator.applyRootMotion = true;
        }
        else
        {
            m_Animator.SetBool("Grounded", false);
        }
*/

}
