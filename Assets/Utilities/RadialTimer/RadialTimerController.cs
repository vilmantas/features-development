using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Utilities.RadialTimer
{
    public class RadialTimerController : MonoBehaviour
    {
        private Image Foreground;

        private void Awake()
        {
            Foreground = transform.Find("foreground").GetComponent<Image>();
        }

        public void SetFillAmount(float max, float current)
        {
            Foreground.fillAmount = current / max;
        }
    }

    [Serializable]
    public class OnClick : UnityEvent<RadialTimerController>
    {
    }
}