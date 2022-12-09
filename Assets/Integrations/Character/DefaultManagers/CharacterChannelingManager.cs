using System;
using System.Collections.Generic;
using System.Linq;
using Features.Cooldowns;
using Features.Skills;
using UnityEngine;

namespace Features.Character
{
    public class CharacterChannelingManager : MonoBehaviour
    {
        private GameObject Root;

        private ChannelingController m_ChannelingController;
        
        private void Start()
        {
            Root = transform.root.gameObject;

            m_ChannelingController = Root.GetComponentInChildren<ChannelingController>();
        }
    }
}