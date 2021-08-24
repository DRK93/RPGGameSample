using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RpgAdventure
{
    public class BanditBehaviour : MonoBehaviour, IMessageReceiver, IAttackAnimListener
    {
        public MeleeWeapon meleeWeapon;
        public PlayerScanner playerScanner;
        public float timeToStopPuersuit = 2.0f;
        public float timeToWaitOnPursuit = 2.0f;
        public float attackDistance;

        public bool HasFollowTarget
        {
            get
            {
                return m_FollowTarget != null;
            }
        }

        private PlayerController m_FollowTarget;
        private EnemyController m_EnemyController;
        private EnemiesNameByUIS m_EnemyDictionary;
        private UniqueId m_EnemyId;
        private EnemyHealthBar m_EnemyHealthBar;
        private Damageable m_Damagable;
        private float m_TimeSinceLostTarget = 0.0f;
        private Vector3 m_OriginPosition;
        private Quaternion m_OriginalRotation;
        private string m_EnemyName;


        public string ThisEnemyName
        {
            get
            {
                return m_EnemyName;
            }
        }

        private readonly int m_HashInPursuit = Animator.StringToHash("InPursuit");
        private readonly int m_HashNearBase = Animator.StringToHash("NearBase");
        private readonly int m_HashAttack = Animator.StringToHash("Attack");
        private readonly int m_HashHurt = Animator.StringToHash("Hurt");
        private readonly int m_HashDeath = Animator.StringToHash("Death");

        private void Awake()
        {   
            m_EnemyController = GetComponent<EnemyController>();
            m_EnemyId = GetComponent<UniqueId>();
            m_EnemyName = EnemyNameCheck(m_EnemyController.name);
            m_OriginPosition = transform.position;
            m_OriginalRotation = transform.rotation;
            meleeWeapon.SetOwner(gameObject);
            m_EnemyHealthBar = GetComponent<EnemyHealthBar>();
            //Debug.Log(m_EnemyHealthBar);

            //Issue with load order of the scripts --- moved to Start()
            //meleeWeapon.SetTargetLayer(1 << PlayerController.Instance.gameObject.layer);
        }

        private void Start()
        {
            //Debug.Log(m_EnemyHealthBar);
            m_EnemyDictionary = GameObject.Find("EnemyList").GetComponent<EnemiesNameByUIS>();
            //Debug.Log("EnemDictionary in bandit see this: " + m_EnemyDictionary);
            m_EnemyDictionary.AddEnemyToDictionary(m_EnemyId.Uid, m_EnemyName);
            meleeWeapon.SetTargetLayer(1 << PlayerController.Instance.gameObject.layer);
            m_Damagable = GetComponent<Damageable>();
            Debug.Log(m_Damagable);
            m_EnemyHealthBar.SetMaxHealth(m_Damagable.maxHitPoints);
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
            toBase.y = 0;
            bool nearBase = toBase.magnitude < 0.01f;
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
            switch(type)
            {
                case MessageType.DEAD:
                    OnDeath();
                    m_EnemyHealthBar.SetHealth((sender as Damageable).CurrentHitPoints);
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
        private void OnReceiveDamage()
        {
            
            m_EnemyController.Animator.SetTrigger(m_HashHurt);
        }

        private void StopPursuit()
        {
                 m_FollowTarget = null;
                 m_EnemyController.Animator.SetBool(m_HashInPursuit, false);
                 StartCoroutine(WaitOnPursuit());
        }

        private void GoToOrignalSpot()
        {
            m_FollowTarget = null;
            m_EnemyController.Animator.SetBool(m_HashInPursuit, false);
            m_EnemyController.FollowTarget(m_OriginPosition);
        }

        private void AttackOrFollowTarget()
        {
            Vector3 toTarget = m_FollowTarget.transform.position - transform.position;
            if (toTarget.magnitude <= attackDistance)
            {
                AttackTarget(toTarget);
            }
            else
            {
                FollowTarget();
            }
        }

        private void AttackTarget(Vector3 toTarget)
        {
            var toTargetRotation = Quaternion.LookRotation(toTarget);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                toTargetRotation,
                360 * Time.deltaTime
                );
            m_EnemyController.StopFollowTarget();
            m_EnemyController.Animator.SetTrigger(m_HashAttack);
        }

        private void FollowTarget()
        {
            m_EnemyController.Animator.SetBool(m_HashInPursuit, true);
            m_EnemyController.FollowTarget(m_FollowTarget.transform.position);
        }

        private string EnemyNameCheck (string enemyName)
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

        private IEnumerator WaitOnPursuit ()
        {
            yield return new WaitForSeconds(timeToWaitOnPursuit);
            m_EnemyController.FollowTarget(m_OriginPosition);
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

