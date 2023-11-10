using System.Collections;
using System.Collections.Generic;
using RpgAdventure.Scripts.Player;
using UnityEngine;

namespace RpgAdventure.Scripts.Spells
{
    // class manage spawning correct spell prefab with proper parameters
    // also manage behaviour of the spell before going out from player
    public class SpellSpawner : MonoBehaviour
    {
        public GameObject fireball;
        public GameObject lavaball;
        public GameObject electricshockA;
        public GameObject electricshockB;
        public GameObject rainOfFire;
        public GameObject leftHand;
        public GameObject rightHand;
        public GameObject fireUp;
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
            switch ( number)
            {
                case 1:
                    StartCoroutine(WaitToCreateFireball());
                    break;
                case 2:
                    ElectricBalls();
                    break;
                case 3:
                    StartCoroutine(WaitToCreateLavaball());
                    break;
                case 4:
                    StartCoroutine(WaitToRainFire());
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
            GameObject projectile = Instantiate(lavaball,rightHand.transform);
            projectile.transform.localPosition += new Vector3(-0.3f,0.3f,0f);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            StartCoroutine(WaitToThrowLavaball(projectile, rb));
        }

        private IEnumerator WaitToThrowLavaball( GameObject lavaB, Rigidbody lavaRb)
        {
            yield return new WaitForSeconds(0.9f);
            if (lavaRb != null && lavaB !=null)
            {
                lavaRb.velocity = transform.forward * SpellSpeed;
                lavaB.transform.parent = null;
                lavaB.GetComponent<ProjectileSpell>().SpellDmg = SpellDamage;
            }
        }

        private void ElectricBalls()
        {
            StartCoroutine(WaitToCreateElectricballA());
            StartCoroutine(WaitToCreateElectricballB());
        }

        private IEnumerator WaitToCreateElectricballA()
        {
            yield return new WaitForSeconds(0.3f);
            GameObject projectileA = Instantiate(electricshockA, leftHand.transform);
            projectileA.transform.localPosition += new Vector3(0, 0.2f, 0);
            Rigidbody rb = projectileA.GetComponent<Rigidbody>();
            StartCoroutine(WaitToThrowElectric(projectileA, rb));
        }
        private IEnumerator WaitToCreateElectricballB()
        {
            yield return new WaitForSeconds(0.3f);
            GameObject projectileB = Instantiate(electricshockB, rightHand.transform);
            projectileB.transform.localPosition += new Vector3(0, 0.2f, 0);
            Rigidbody rb = projectileB.GetComponent<Rigidbody>();
            StartCoroutine(WaitToThrowElectric(projectileB, rb));
        }
        private IEnumerator WaitToThrowElectric(GameObject elecB, Rigidbody elecRb)
        {
            yield return new WaitForSeconds(0.9f);
            if (elecRb != null && elecB != null)
            {
                elecRb.velocity = transform.forward * SpellSpeed;
                elecB.transform.parent = null;
                elecB.GetComponent<ProjectileSpell>().SpellDmg = SpellDamage;
            }
        }

        private IEnumerator WaitToRainFire()
        {
            fireUp.SetActive(true);
            yield return new WaitForSeconds(0.9f);
            GameObject rainDamage = Instantiate(rainOfFire, transform);
            rainDamage.transform.parent = null;
            yield return new WaitForSeconds(0.6f);
            fireUp.SetActive(false);
        }
    }
}

