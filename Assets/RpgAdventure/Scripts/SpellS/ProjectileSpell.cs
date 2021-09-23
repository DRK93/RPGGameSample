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
        private float m_spellLifeTime = 2.0f;
        public GameObject fireballToTransform;
        public GameObject impactBeam;

        public float SpellLifeTime { get => m_spellLifeTime; set => m_spellLifeTime = value; }
        public int SpellDmg { get => m_SpellDmg; set => m_SpellDmg = value; }

        private void Update()
        {
            m_TimerCount += Time.deltaTime;
            if(m_TimerCount >= SpellLifeTime)
            {
                Destroy(this.gameObject);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
                return;
            else
            {
                if(other.gameObject.layer == 6)
                {
                    Damageable damageable = other.GetComponent<Damageable>();
                    m_Owner = GameObject.Find("Player");
                    if (damageable != null)
                    {
                        Damageable.DamageMessage data;
                        data.amount = SpellDmg;
                        data.damager = this;
                        data.damageSource = m_Owner;
                        damageable.ApplyDamage(data);
                        other.GetComponent<BanditBehaviour>().DetectionRadiusChange();
                    }
                }
                SpawnImpactEffect();
                Destroy(this.gameObject);
            }
        }
        private void SpawnImpactEffect()
        {
            GameObject impact = Instantiate(impactBeam, fireballToTransform.transform);
            impact.transform.parent = null;
            impactBeam.SetActive(true);
        }
    }
}

