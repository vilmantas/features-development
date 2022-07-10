using Features.Buffs.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.RadialTimer;

namespace Features.Buffs.Utilities
{
    public class SimpleUIController : MonoBehaviour, IBuffUI
    {
        private Image m_Sprite;
        private TextMeshProUGUI m_StacksText;
        private RadialTimerController m_TimerController;

        public void BuffTickCallback(ActiveBuff buff)
        {
            m_TimerController.SetFillAmount(buff.Metadata.Duration, buff.DurationLeft);

            m_StacksText.text = buff.Stacks.ToString();
        }

        public void Setup(ActiveBuff buff)
        {
            m_TimerController = GetComponentInChildren<RadialTimerController>();
            m_StacksText = GetComponentInChildren<TextMeshProUGUI>();
            m_Sprite = transform.Find("sprite").GetComponent<Image>();

            m_Sprite.sprite = null;
        }

        public new GameObject gameObject => transform.gameObject;
    }
}