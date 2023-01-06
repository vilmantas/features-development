using System;
using Features.Camera;
using UnityEngine;

namespace Managers
{
    public class GameplayCameraController : MonoBehaviour
    {
        private CameraManager m_CameraManager;

        private GameplayManager m_GameplayManager;
        
        private void Awake()
        {
            m_CameraManager = GameObject.Find("camera_manager").GetComponent<CameraManager>();
            
            m_GameplayManager = GameObject.Find("gameplay_manager").GetComponent<GameplayManager>();
        }

        private void Update()
        {
            
        }
    }
}