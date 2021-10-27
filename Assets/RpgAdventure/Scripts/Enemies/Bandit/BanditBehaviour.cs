using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RpgAdventure
{
    //main class to manage enemy behaviour and respond for Player actions
    //choose correct animations, send information to other classes that enemies use, affect other enemies
    //manage fighting based on random generated number which will give a little unpredictable fight choices
    public class BanditBehaviour : MonoBehaviour, IMessageReceiver, IAttackAnimListener
    {
        public MeleeWeapon meleeWeapon;
        public PlayerScanner playerScanner;
        public float timeToStopPuersuit = 2.0f;
        public float timeToWaitOnPursuit = 2.0f;
        public float attackDistance;

        private PlayerController m_FollowTarget;
        private EnemyController m_EnemyController;
        private EnemiesNameByUIS m_EnemyDictionary;
        private EnemiesBySaveSystemId m_EnemyAliveList;
        private UniqueId m_EnemyId;
        private EnemyHealthBar m_EnemyHealthBar;
        private Damageable m_Damagable;
        private GameObject[] alliesByTag;
        private List<GameObject> allies;
        private float m_TimeSinceLostTarget = 0.0f;
        private Vector3 m_OriginPosition;
        private Quaternion m_OriginalRotation;
        private float allyHelpDistance = 8f;
        private string m_EnemyName;
        private float m_DetectionRadiusOrig;
        private float m_DetectionAngleOrig;
        private bool m_BanditAttacking;
        public bool BanditAttacking
        {
            set { m_BanditAttacking = value; }
        }
        
        private int m_numberAttackRange;
        private int m_firstAttackRange;
        private int m_secondAttackRange;
        private int m_thirdAttackRange;
        //private int m_blockRange;
        //private int m_RepositionRange;
        private int m_situationNumber;
        public int SituationNumber
        {
            set { m_situationNumber = value; }
        }

        public bool HasFollowTarget => m_FollowTarget != null;
        public string ThisEnemyName => m_EnemyName;

        private readonly int m_HashInPursuit = Animator.StringToHash("InPursuit");
        private readonly int m_HashNearBase = Animator.StringToHash("NearBase");
        private readonly int m_HashAttack1 = Animator.StringToHash("Attack1");
        private readonly int m_HashAttack2 = Animator.StringToHash("Attack2");
        private readonly int m_HashAttack3 = Animator.StringToHash("Attack3");
        private readonly int m_HashBlockingStance = Animator.StringToHash("BlockingStance");
        private readonly int m_HashBlockedHit = Animator.StringToHash("BlockedHit");
        private readonly int m_HashHurt = Animator.StringToHash("Hurt");
        private readonly int m_HashDeath = Animator.StringToHash("Death");
        private readonly int m_HashInAttackRange = Animator.StringToHash("InAttackRange");

        private void Awake()
        {   
            m_EnemyController = GetComponent<EnemyController>();
            m_EnemyId = GetComponent<UniqueId>();
            m_EnemyName = EnemyNameCheck(m_EnemyController.name);
            m_OriginPosition = transform.position;
            m_OriginalRotation = transform.rotation;
            meleeWeapon.SetOwner(gameObject);
            m_EnemyHealthBar = GetComponent<EnemyHealthBar>();

            //Issue with load order of the scripts --- moved to Start()
            //meleeWeapon.SetTargetLayer(1 << PlayerController.Instance.gameObject.layer);
        }

        private void Start()
        {
            m_EnemyDictionary = GameObject.Find("EnemyList").GetComponent<EnemiesNameByUIS>();
            m_EnemyDictionary.AddEnemyToDictionary(m_EnemyId.Uid, m_EnemyName);
            meleeWeapon.SetTargetLayer(1 << PlayerController.Instance.gameObject.layer);
            m_Damagable = GetComponent<Damageable>();
            m_EnemyHealthBar.SetMaxHealth(m_Damagable.GetComponent<CharacterStats>().maxHitPoints);
            m_DetectionRadiusOrig = playerScanner.detectionRadius;
            m_DetectionAngleOrig = playerScanner.detectionAngle;
            m_EnemyAliveList = GameObject.Find("EnemyList").GetComponent<EnemiesBySaveSystemId>();
            m_EnemyAliveList.enmiesSaveSystem.Add(m_Damagable.GetComponent<CharacterStats>().uniqueID, m_Damagable.GetComponent<CharacterStats>().isDead);
            
            m_situationNumber = 0;
            m_BanditAttacking = false;
        }
        private void Update()
        {
            if(PlayerController.Instance.IsRespawning)
            {
                GoToOrignalSpot();
                CheckIfNearBase();
                return;
            }
            GuardPosition();
        }

        private void GuardPosition()
        {
            var detectedTarget = playerScanner.Detect(transform);
            if (m_FollowTarget == null)
            {
                if (detectedTarget != null)
                {
                    m_FollowTarget = detectedTarget;
                }
            }
            else
            {
                AttackOrFollowTarget();

                ReDetectTarget(detectedTarget);
            }
            CheckIfNearBase();
        }
        private void CheckIfNearBase()
        {
            Vector3 toBase = m_OriginPosition - transform.position;
            Vector2 toBase2 = new Vector2(toBase.x, toBase.z);
            bool nearBase = toBase2.magnitude < 2.0f;
            m_EnemyController.Animator.SetBool(m_HashNearBase, nearBase);
            if (nearBase)
            {
                Quaternion targetRotation = Quaternion.RotateTowards(
                    transform.rotation,
                    m_OriginalRotation,
                    360 * Time.deltaTime
                    );
                transform.rotation = targetRotation;
            }
            else
            {
                if( m_FollowTarget == null)
                {
                    GoToOrignalSpot();
                }
            }
        }

        private void GoToOrignalSpot()
        {
            m_FollowTarget = null;
            m_EnemyController.Animator.SetBool(m_HashInAttackRange, false);
            m_EnemyController.Animator.SetBool(m_HashInPursuit, false);
            m_EnemyController.FollowTarget(m_OriginPosition);
            m_situationNumber = 0;
            m_BanditAttacking = false;
        }

        private void ReDetectTarget(PlayerController detectedTarget)
        {
            if (detectedTarget == null)
            {
                m_TimeSinceLostTarget += Time.deltaTime;
                if (m_TimeSinceLostTarget >= timeToStopPuersuit)
                {
                    StopPursuit();
                }
            }
            else
            {
                m_TimeSinceLostTarget = 0;
            }
        }

        private void StopPursuit()
        {
            m_FollowTarget = null;
            m_EnemyController.Animator.SetBool(m_HashInPursuit, false);
            StartCoroutine(WaitOnPursuit());
        }

        private void AttackOrFollowTarget()
        {
            Vector3 toTarget = m_FollowTarget.transform.position - transform.position;
            if (toTarget.magnitude <= attackDistance)
            {
                RotateToTarget(toTarget);
                AttackTarget(toTarget);
            }
            else
            {
                RotateToTarget(toTarget);
                FollowTarget();
            }
        }

        private void RotateToTarget(Vector3 toTarget)
        {
            var toTargetRotation = Quaternion.LookRotation(toTarget);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                toTargetRotation,
                360 * Time.deltaTime
                );
        }
        private void AttackTarget(Vector3 toTarget)
        {
            m_EnemyController.Animator.SetBool(m_HashInAttackRange, true);
            m_EnemyController.StopFollowTarget();

            if (m_BanditAttacking == false)
            {
                m_BanditAttacking = true;
                AttackPossibilities();
            }
        }

        private void FollowTarget()
        {
            m_situationNumber = 0;
            m_BanditAttacking = false;
            m_EnemyController.Animator.SetBool(m_HashInAttackRange, false);
            m_EnemyController.Animator.SetBool(m_HashInPursuit, true);
            m_EnemyController.FollowTarget(m_FollowTarget.transform.position);
        }

        private void AttackPossibilities()
        {
            Debug.Log("Situation number: " + m_situationNumber);
            
            m_numberAttackRange = Random.Range(1, 20);
            SituationCheck();

            if (m_numberAttackRange <= m_firstAttackRange)
            {
                m_EnemyController.Animator.SetTrigger(m_HashAttack1);
                m_situationNumber = 1;
            }
            else if (m_numberAttackRange > m_firstAttackRange && m_numberAttackRange <= m_secondAttackRange)
            {
                m_EnemyController.Animator.SetTrigger(m_HashAttack2);
                m_situationNumber = 2;
            }
            else if (m_numberAttackRange > m_secondAttackRange && m_numberAttackRange <= m_thirdAttackRange)
            {
                m_EnemyController.Animator.SetTrigger(m_HashAttack3);
                m_situationNumber = 3;
            }
            else if (m_numberAttackRange > m_thirdAttackRange)
            {
                m_EnemyController.Animator.SetTrigger(m_HashBlockingStance);
                m_situationNumber = 0;
            }
            //else if (m_numberAttackRange > m_blockRange && m_numberAttackRange <= m_RepositionRange)
            //{
            //    m_EnemyController.Animator.SetTrigger(m_HashBlockingStance);
            //    m_situationNumber = 0;
            //}
            //else if (m_numberAttackRange > m_RepositionRange)
            //{
            //    Reposition();
            //}

        }

        private void SituationCheck()
        {
            switch(m_situationNumber)
            {
                case 0:
                    // First attack wiht no conditions aka default situation:
                    m_firstAttackRange = 10;
                    m_secondAttackRange = 12;
                    m_thirdAttackRange = 14;
                    //m_blockRange = 16;
                    //m_RepositionRange = 18;
                    break;
                case 1:
                    // after first succesfull attack
                    m_firstAttackRange = 2;
                    m_secondAttackRange = 14;
                    m_thirdAttackRange = 16;
                    //m_blockRange = 18;
                    //m_RepositionRange = 19;
                    break;
                case 2:
                    // after second succesfull attack
                    m_firstAttackRange = 2;
                    m_secondAttackRange = 4;
                    m_thirdAttackRange = 16;
                    //m_blockRange = 18;
                    //m_RepositionRange = 19;
                    break;
                case 3:
                    // after third succesfull attack
                    m_firstAttackRange = 7;
                    m_secondAttackRange = 9;
                    m_thirdAttackRange = 11;
                    //m_blockRange = 13;
                    //m_RepositionRange = 19;
                    break;
                case 4:
                    // after getting hit
                    m_firstAttackRange = 3;
                    m_secondAttackRange = 5;
                    m_thirdAttackRange = 7;
                   // m_blockRange = 9;
                    //m_RepositionRange = 18;
                    break;
                case 5:
                    // after succesfull block
                    m_firstAttackRange = 4;
                    m_secondAttackRange = 10;
                    m_thirdAttackRange = 11;
                    //m_blockRange = 12;
                    //m_RepositionRange = 19;
                    break;
                default:
                    break;
            }
        }
        public void StartBlocking()
        {
            m_Damagable.BlockStance = true;
            if (meleeWeapon != null)
                meleeWeapon.blockStanceAudio.PlayRandomClip();
        }

        public void FinishBlocking()
        {
            m_Damagable.BlockStance = false;
        }
        private void Reposition()
        {
            Debug.Log("Repositioning");
            StartCoroutine(SomeTimeToResetAttack());
            // find something other place near player and go there
            // face player again from different angle
            // create circle around player and set point on that circle to go 
            // go on straight lines to avoid collision with player, change points fast to get the circle vibe
        }

        public void MeleeAttackStart()
        {
            meleeWeapon.BeginAttack();
        }

        public void MeleeAttackEnd()
        {
            meleeWeapon.EndAttack();
        }

        public void TempoWindowBegin()
        {
        }

        public void TempoWindowEnd()
        {
        }

        public void OnReceiveMessage(MessageType type, object sender, object message)
        {
            switch (type)
            {
                case MessageType.BLOCKED:
                    BlockedHit();
                    break;
                case MessageType.DEAD:
                    OnDeath();
                    m_EnemyHealthBar.SetHealth((sender as Damageable).CurrentHitPoints);
                    m_Damagable.GetComponent<CharacterStats>().isDead = true;
                    m_EnemyAliveList.enmiesSaveSystem[m_Damagable.GetComponent<CharacterStats>().uniqueID] = m_Damagable.GetComponent<CharacterStats>().isDead;
                    break;
                case MessageType.DAMAGED:

                    OnReceiveDamage();
                    m_EnemyHealthBar.SetHealth((sender as Damageable).CurrentHitPoints);
                    break;
                case MessageType.HIGHDAMAGED:

                    OnReceiveDamage();
                    m_EnemyHealthBar.SetHealth((sender as Damageable).CurrentHitPoints);
                    break;
                default:
                    break;
            }
        }
        private void OnDeath()
        {
            m_EnemyController.StopFollowTarget();
            m_EnemyController.Animator.SetTrigger(m_HashDeath);
        }
        private void BlockedHit()
        {
            m_EnemyController.Animator.SetTrigger(m_HashBlockedHit);
            m_situationNumber = 5;
            m_BanditAttacking = false;
        }
        private void OnReceiveDamage()
        {
            m_EnemyController.Animator.SetTrigger(m_HashHurt);
            m_situationNumber = 4;
            m_BanditAttacking = false;
        }

        private string EnemyNameCheck(string enemyName)
        {
            if (enemyName.EndsWith(")"))
            {
                enemyName = enemyName.Remove(enemyName.Length - 2);
                if (enemyName.EndsWith("("))
                {
                    return enemyName.Remove(enemyName.Length - 2);
                }
                else
                {
                    enemyName = enemyName.Remove(enemyName.Length - 1);
                    if (enemyName.EndsWith("("))
                    {
                        return enemyName.Remove(enemyName.Length - 2);
                    }
                }
            }
            return enemyName;
        }
        private void AlliesList()
        {
            alliesByTag = null;
            allies = new List<GameObject>();
            alliesByTag = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var ally in alliesByTag)
            {
                Vector3 toAlly = ally.transform.position - transform.position;
                if (toAlly.magnitude <= allyHelpDistance)
                {
                    allies.Add(ally);
                }
            }
            if (allies != null)
            {
                foreach (var ally in allies)
                    ally.GetComponent<BanditBehaviour>().DetectionRadiusChange(40f);
            }
        }
        public void AttackedFromRange()
        {
            DetectionRadiusChange(40f);
            AlliesList();
        }
        public void DetectionRadiusChange(float addingNumber)
        {
            playerScanner.detectionRadius = addingNumber;
            playerScanner.detectionAngle = 360.0f;
            StartCoroutine(WaitToReturnDetectRad());
        }

        private IEnumerator WaitOnPursuit ()
        {
            yield return new WaitForSeconds(timeToWaitOnPursuit);
            m_EnemyController.FollowTarget(m_OriginPosition);
        }

        private IEnumerator WaitToReturnDetectRad()
        {
            yield return new WaitForSeconds(5.0f);
            playerScanner.detectionRadius = m_DetectionRadiusOrig;
            playerScanner.detectionAngle = m_DetectionAngleOrig;
        }

        private IEnumerator SomeTimeToResetAttack()
        {
            yield return new WaitForSeconds(0.5f);
            m_BanditAttacking = false;
        }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Color c = new Color(0.8f, 0, 0, 0.4f);
            UnityEditor.Handles.color = c;

            Vector3 rotatedForward = Quaternion.Euler(
                0,
                -playerScanner.detectionAngle * 0.5f,
                0) * transform.forward;

            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                Vector3.up,
                rotatedForward,
                playerScanner.detectionAngle,
                playerScanner.detectionRadius);

            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                Vector3.up,
                rotatedForward,
                360,
                playerScanner.meleeDetectionRadius);
        }
#endif
    }
}

