using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine
{
    public class ChannelingItemUI : MonoBehaviour
    {
        private Canvas m_Canvas;

        private Image m_Image;

        private const int MAX_WIDTH = 300;

        private void Awake()
        {
            m_Canvas = GetComponentInChildren<Canvas>();

            m_Canvas.worldCamera = Camera.main;

            m_Image = GetComponentInChildren<Image>();
            
            var sizeDelta = m_Image.rectTransform.sizeDelta;
            
            sizeDelta = new Vector2(0, sizeDelta.y);

            m_Image.rectTransform.sizeDelta = sizeDelta;
        }

        public void SetFillAmount(float current, float max)
        {
            var sizeDelta = m_Image.rectTransform.sizeDelta;
            
            sizeDelta = new Vector2(current / max * MAX_WIDTH, sizeDelta.y);
            
            m_Image.rectTransform.sizeDelta = sizeDelta;
        }
    }
}