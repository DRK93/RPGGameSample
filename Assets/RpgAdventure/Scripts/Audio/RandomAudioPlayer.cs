using System.Collections;
using UnityEngine;

namespace RpgAdventure
{
    public class RandomAudioPlayer : MonoBehaviour
    {
        [System.Serializable]
        public class SoundBank
        {
            public string name;
            public AudioClip[] clips;
        }
        public bool canPlay;
        public bool isPlaying;
        public SoundBank soundBank = new SoundBank();
        private AudioSource m_AudioSouerce;
        private void Awake()
        {
            m_AudioSouerce = GetComponent<AudioSource>();   
        }

        public void PlayRandomClip()
        {
            var clip = soundBank.clips[Random.Range(0, soundBank.clips.Length)];
            if(clip == null)
            {
                return;
            }
            else
            {
                m_AudioSouerce.clip = clip;
                m_AudioSouerce.Play();
            }

        }
    }
}