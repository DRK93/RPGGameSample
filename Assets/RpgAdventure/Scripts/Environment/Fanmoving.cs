using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure.Scripts.Environment
{
    public class Fanmoving : MonoBehaviour
    {
        public GameObject windmillFan;
        void Update()
        {
            windmillFan.transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * 35);
        }
    }
}

