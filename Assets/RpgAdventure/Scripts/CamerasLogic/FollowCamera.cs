using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure.Scripts.CamerasLogic
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform target;


        void LateUpdate()
        {
            if (!target)
            {
                return;
            }

            float currentRotationAngle = transform.eulerAngles.y;
            float wantedRotationAngle = target.transform.eulerAngles.y;

            currentRotationAngle = Mathf.LerpAngle(
                currentRotationAngle,
                wantedRotationAngle,
                0.5f
                );

            transform.position = new Vector3(
                target.position.x,
                5.0f,
                target.position.z);

            Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            Vector3 rotatedPosition = currentRotation * Vector3.forward;

            transform.position = transform.position - rotatedPosition * 10;

            transform.LookAt(target);
        }
    }
}

