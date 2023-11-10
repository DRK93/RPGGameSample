using System.Collections;
using UnityEngine;

namespace RpgAdventure.Scripts.Audio
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
        public bool canPlay2;
        public bool isPlaying;
        public SoundBank soundBank = new SoundBank();
        private AudioSource m_AudioSouerce;
        private void Awake()
        {
            m_AudioSouerce = GetComponent<AudioSource>();   
        }

        private void Update()
        {
            if(canPlay2 == true && m_AudioSouerce.isPlaying == false)
            {
                PlayRandomClip();
            }
        }

        public void PlayManyRandomClips()
        {
            canPlay2 = true;
            PlayRandomClip();
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
                isPlaying = true;
            }
        }
        public void StopRandomClip()
        {
            isPlaying = false;
            canPlay2 = false;
            m_AudioSouerce.Stop();
        }
    }
}