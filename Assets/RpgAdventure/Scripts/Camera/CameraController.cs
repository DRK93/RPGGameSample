using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineFreeLook freeLookCamera;

        public CinemachineFreeLook PlayerCam
        {
            get
            {
                return freeLookCamera;
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(2))
            {
                freeLookCamera.m_XAxis.m_MaxSpeed = 400;
                freeLookCamera.m_YAxis.m_MaxSpeed = 10;
            }

            if (Input.GetMouseButtonUp(2))
            {
                freeLookCamera.m_XAxis.m_MaxSpeed = 0;
                freeLookCamera.m_YAxis.m_MaxSpeed = 0;
            }
        }
    }
}


