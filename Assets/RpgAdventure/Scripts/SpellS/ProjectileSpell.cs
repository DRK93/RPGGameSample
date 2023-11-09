using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class ProjectileSpell : MonoBehaviour
    {
        private int m_SpellDmg;
        private GameObject m_Owner;
        private float m_TimerCount = 0.0f;
        [SerializeField]
        private float m_spellLifeTime = 2.0f;
        [SerializeField]
        private int m_SpellDamageMultiply = 1;
        [SerializeField]
        private float m_spellLifeTimeMultiply = 1.0f;

        public GameObject spellToTransform;
        public GameObject impactBeam;
        public GameObject bigBoom;
        public RandomAudioPlayer castingSpellAudio;
        public RandomAudioPlayer spellImpactAudio;

        public float SpellLifeTime { get => m_spellLifeTime; set => m_spellLifeTime = value; }
        public int SpellDmg { get => m_SpellDmg; set => m_SpellDmg = value; }

        private void Start()
        {
            castingSpellAudio.PlayRandomClip();
        }
        private void Update()
        {
            m_TimerCount += Time.deltaTime;
            if(m_TimerCount >= SpellLifeTime * m_spellLifeTimeMultiply)
            {
                Destroy(this.gameObject);
            }
            if (GameObject.Find("Player").GetComponent<PlayerInput>().spellNumber == 3)
                if (transform.parent != null)
                {
                    if (transform != null)
                        transform.localScale += Time.deltaTime * transform.localScale * 0.5f;
                }
                else
                    GetComponent<SphereCollider>().isTrigger = true;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
                return;
            else
            {
                if(transform.parent==null)
                {
                    if (other.gameObject.layer == 6)
                    {
                        Damageable damageable = other.GetComponent<Damageable>();
                        m_Owner = GameObject.Find("Player");
                        if (damageable != null)
                        {
                            Damageable.DamageMessage data;
                            data.amount = SpellDmg * m_SpellDamageMultiply;
                            data.damager = this;
                            data.damageSource = m_Owner;
                            damageable.ApplyDamageFromSpell(data);
                            other.GetComponent<BanditBehaviour>().AttackedFromRange();
                        }
                    }
                    if (other.gameObject.layer == 9)
                    {
                        SpawnForceFieldImpact();
                    }
                    SpawnImpactEffect();
                }
                Destroy(this.gameObject);
            }
        }
        private void SpawnImpactEffect()
        {
            GameObject impact = Instantiate(impactBeam, spellToTransform.transform);
            impact.transform.parent = null;
            impactBeam.SetActive(true);
        }
        private void SpawnForceFieldImpact()
        {
            GameObject bigImpact = Instantiate(bigBoom, spellToTransform.transform);
            bigImpact.transform.parent = null;
            bigImpact.SetActive(true);
        }
    }
}

