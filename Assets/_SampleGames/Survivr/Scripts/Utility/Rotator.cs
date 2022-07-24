using System;
using UnityEngine;

namespace _SampleGames.Survivr.Scripts.Utility
{
    public class Rotator : MonoBehaviour
    {
        public float Speed;

        private Transform m_Transform;
        
        private void Awake()
        {
            m_Transform = transform;
        }

        private void Update()
        {
            m_Transform.RotateAround(m_Transform.position, Vector3.up, Speed * Time.deltaTime);
        }
    }
}