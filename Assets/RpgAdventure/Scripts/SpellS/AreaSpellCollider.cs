using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class AreaSpellCollider : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 6)
            {
                transform.parent.gameObject.GetComponent<AreaSpell>().EnteringSpellArea(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 6)
            {
                transform.parent.gameObject.GetComponent<AreaSpell>().ExitSpellArea(other.gameObject);
            }
        }

    }
   
}

