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
    
        // Start is called before the first frame update
        void Awake()
        {
            Text = GetComponentInChildren<TextMeshPro>();
        }

        public void SetText(string text)
        {
            Text.text = text;
        }
    }
}
