using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAudio : MonoBehaviour
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

    public void CastingSound()
    {

    }
    public void ReadySpell()
    {

    }

    public void SpellImpact()
    {

    }
}
