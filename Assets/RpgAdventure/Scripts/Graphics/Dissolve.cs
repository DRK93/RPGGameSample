using System.Collections;
using UnityEngine;

namespace RpgAdventure.Scripts.Graphics
{
    public class Dissolve : MonoBehaviour
    {
        public float dissolveTime = 6.0f;

        private void Awake()
        {
            dissolveTime += Time.time;
        }

        private void Update()
        {
            if (Time.time >= dissolveTime)
            {
                Destroy(gameObject);
            }
        }
    }
}