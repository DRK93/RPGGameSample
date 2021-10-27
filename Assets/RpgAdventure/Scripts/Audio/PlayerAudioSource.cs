using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class PlayerAudioSource : MonoBehaviour
    {
        public RandomAudioPlayer villageAudio;
        public RandomAudioPlayer beginningAudio;
        public RandomAudioPlayer eastForrestAudio;
        public RandomAudioPlayer standardAudio;
        private bool m_eastForrestCollider1;
        private bool m_eastForrestCollider2;

        private void Start()
        {
            standardAudio.PlayRandomClip();
            m_eastForrestCollider1 = false;
            m_eastForrestCollider2 = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 11)
            {
                if (other.CompareTag("Village"))
                {
                    standardAudio.StopRandomClip();
                    villageAudio.PlayManyRandomClips();
                }
                else if (other.CompareTag("EastForrest"))
                {
                    if (m_eastForrestCollider1 == false && m_eastForrestCollider2 == false)
                    {
                        m_eastForrestCollider1 = true;
                        standardAudio.StopRandomClip();
                        eastForrestAudio.PlayManyRandomClips();
                    }
                    else if (m_eastForrestCollider1 == true && m_eastForrestCollider2 == false)
                    {
                        m_eastForrestCollider2 = true;
                    }
                    else if (m_eastForrestCollider1 == false && m_eastForrestCollider2 == true)
                    {
                        m_eastForrestCollider1 = true;
                    }
                }

                else if (other.CompareTag("GameBeginZone"))
                {
                    standardAudio.StopRandomClip();
                    beginningAudio.PlayManyRandomClips();
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 11)
            {
                if (other.CompareTag("EastForrest"))
                {
                    if (m_eastForrestCollider1 == true && m_eastForrestCollider2 == true)
                    {
                        m_eastForrestCollider1 = false;
                    }
                    else if (m_eastForrestCollider1 == true && m_eastForrestCollider2 == false)
                    {
                        m_eastForrestCollider1 = false;
                        eastForrestAudio.StopRandomClip();
                        standardAudio.PlayManyRandomClips();
                    }
                    else if (m_eastForrestCollider1 == false && m_eastForrestCollider2 == true)
                    {
                        m_eastForrestCollider2 = false;
                        eastForrestAudio.StopRandomClip();
                        standardAudio.PlayManyRandomClips();
                    }
                }
                else
                {
                    beginningAudio.StopRandomClip();
                    villageAudio.StopRandomClip();
                    standardAudio.PlayManyRandomClips();
                }
            }
        }
    }
}

