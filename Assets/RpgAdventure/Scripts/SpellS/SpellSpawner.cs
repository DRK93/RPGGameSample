using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class SpellSpawner : MonoBehaviour
    {
        public GameObject projectile;
        public GameObject leftHand;
        private int m_spellSpeed;
        private int m_spellDamage;
        public int SpellSpeed { get => m_spellSpeed; set => m_spellSpeed = value; }
        public int SpellDamage { get => m_spellDamage; set => m_spellDamage = value; }

        public void CreateSpell()
        {
            m_spellDamage = GetComponent<PlayerStats>().spellDamage;
            m_spellSpeed = GetComponent<PlayerStats>().spellSpeed;
            StartCoroutine(WaitToCreate());
        }

        private IEnumerator WaitToCreate()
        {
            yield return new WaitForSeconds(0.3f);
            GameObject fireball = Instantiate(projectile,leftHand.transform);
            Rigidbody rb = fireball.GetComponent<Rigidbody>();
            StartCoroutine(WaitToFly(fireball, rb));
        }

        private IEnumerator WaitToFly(GameObject fball, Rigidbody rbFireball)
        {
            yield return new WaitForSeconds(0.4f);
            if(rbFireball!=null && fball!=null)
            {
                rbFireball.velocity = transform.forward * SpellSpeed;
                fball.transform.parent = null;
                fball.GetComponent<ProjectileSpell>().SpellDmg = SpellDamage;
            }
        }
    }
}

