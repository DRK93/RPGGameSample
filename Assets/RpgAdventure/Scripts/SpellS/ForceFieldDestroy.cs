using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class ForceFieldDestroy : MonoBehaviour
    {
        private GameObject forceField;
        void Start()
        {
            forceField = GameObject.Find("ForceField");
            StartCoroutine(WaitToDestroyField());
        }

        private IEnumerator WaitToDestroyField()
        {
            yield return new WaitForSeconds(2.5f);
            Destroy(forceField);
        }
    }
}

