using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.RadialTimer;

namespace Features.Buffs.UI.Utilities
{
    public class BuffUIDataController : BaseBuffUIData
    {
        private Image m_Sprite;
        private Transform m_StacksContainer;
        private TextMeshProUGUI m_StacksText;
        private RadialTimerController m_TimerController;

        public override void BuffTickCallback(ActiveBuff buff)
        {
            m_TimerController.SetFillAmount(buff.Metadata.Duration, buff.DurationLeft);

            m_StacksText.text = buff.Stacks.ToString();
        }

        public override void OnSetData(ActiveBuff buff)
        {
            m_TimerController = GetComponentInChildren<RadialTimerController>();
            m_StacksContainer = transform.Find("stacks_container");
            m_StacksText = GetComponentInChildren<TextMeshProUGUI>();
            m_Sprite = transform.Find("sprite").GetComponent<Image>();

            m_StacksContainer.gameObject.SetActive(buff.Stacks > 1);

            if (buff.Metadata.Sprite != null)
            {
                m_Sprite.sprite = buff.Metadata.Sprite;
            }
        }
    }
}