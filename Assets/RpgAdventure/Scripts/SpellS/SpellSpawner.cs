using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class SpellSpawner : MonoBehaviour
    {
        public GameObject fireball;
        public GameObject lavaball;
        public GameObject electricshock;
        public GameObject rainOfFire;
        public GameObject leftHand;
        public GameObject rightHand;
        private int m_spellSpeed;
        private int m_spellDamage;
        public int SpellSpeed { get => m_spellSpeed; set => m_spellSpeed = value; }
        public int SpellDamage { get => m_spellDamage; set => m_spellDamage = value; }

        public void CreateSpell(int spellNumber)
        {
            m_spellDamage = GetComponent<PlayerStats>().spellDamage;
            m_spellSpeed = GetComponent<PlayerStats>().spellSpeed;
            SpellCheck(spellNumber);
        }


        private void SpellCheck(int number)
        {
            Debug.Log("Checkich which spell use");
            switch ( number)
            {
                case 1:
                    StartCoroutine(WaitToCreateFireball());
                    break;
                case 2:
                    StartCoroutine(WaitToCreateLavaball());
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default:
                    Debug.Log("Something went wrong");
                    break;
            }

        }
        private IEnumerator WaitToCreateFireball()
        {
            yield return new WaitForSeconds(0.3f);
            GameObject projectile = Instantiate(fireball, leftHand.transform);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            StartCoroutine(WaitToFly(projectile, rb));
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
        private IEnumerator WaitToCreateLavaball()
        {
            yield return new WaitForSeconds(0.3f);
            GameObject projectile = Instantiate(lavaball, leftHand.transform);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            StartCoroutine(WaitToThrowLavaball(projectile, rb));
        }

        private IEnumerator WaitToThrowLavaball( GameObject lavaB, Rigidbody lavaRb)
        {
            yield return new WaitForSeconds(0.9f);
            Debug.Log("Fly LavaBall");
            Debug.Log(lavaB.transform.parent);
            if (lavaRb != null && lavaB !=null)
            {
                lavaRb.velocity = transform.forward * SpellSpeed;
                lavaB.transform.parent = null;
                Debug.Log(lavaB.transform.parent);
                lavaB.GetComponent<ProjectileSpell>().SpellDmg = SpellDamage;
            }
        }
    }
}

