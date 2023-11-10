using System;
using System.Collections;
using UnityEngine;

namespace RpgAdventure.Scripts.Core
{
    public class UniqueId : MonoBehaviour
    {
        [SerializeField]
        private string m_Uid = Guid.NewGuid().ToString();

        public string Uid { get { return m_Uid; } }
    }
}