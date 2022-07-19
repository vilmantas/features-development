using System;
using UnityEngine;
using UnityEngine.UI;

namespace _SampleGames.Survivr
{
    public class SimpleLoadingBarController : MonoBehaviour
    {
        private RectTransform m_Current;

        public void Awake()
        {
            m_Current = transform.Find("current").GetComponent<RectTransform>();
        }

        public void SetFillPercent(float percent)
        {
            m_Current.sizeDelta = new Vector2(percent, m_Current.sizeDelta.y);
        }
    }
}