using System;
using Cinemachine;
using UnityEngine;

namespace _SampleGames.Survivr
{
    public class CameraManager : Manager
    {
        private GameObject m_Character;

        public CinemachineVirtualCamera m_Camera;
        
        public override void Initialize()
        {
            m_Character = GameObject.FindGameObjectWithTag("Player");

            m_Camera.Follow = m_Character.transform;
            m_Camera.LookAt = m_Character.transform;
        }
    }
}