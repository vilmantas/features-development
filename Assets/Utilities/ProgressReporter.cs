using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;

namespace Utilities
{
    public class ProgressReporter : MonoBehaviour
    {
        private TextMeshPro Text;

        private Canvas m_Canvas;
        
        // Start is called before the first frame update
        void Awake()
        {
            m_Canvas = GetComponentInChildren<Canvas>();
            
            m_Canvas.worldCamera = Camera.main;

            // Text = GetComponentInChildren<TextMeshPro>();
        }

        public void SetText(string text)
        {
            // Text.text = text;
        }
    }
}
