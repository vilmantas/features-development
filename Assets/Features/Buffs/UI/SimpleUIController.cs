using TMPro;
using UnityEngine;
using Utilities.RadialTimer;

namespace Features.Buffs.UI
{
    public class SimpleUIController : MonoBehaviour, IBuffUI
    {
        private TextMeshProUGUI m_Text;
        private RadialTimerController m_TimerController;

        private void Awake()
        {
            m_TimerController = GetComponentInChildren<RadialTimerController>();
            m_Text = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void BuffTickCallback(ActiveBuff buff)
        {
            m_TimerController.SetFillAmount(buff.Metadata.Duration, buff.DurationLeft);

            m_Text.text = buff.Stacks.ToString();
        }

        public new GameObject gameObject => transform.gameObject;
    }
}