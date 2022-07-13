using TMPro;
using UnityEngine.UI;
using Utilities.RadialTimer;

namespace Features.Buffs.Utilities
{
    public class BuffUIDataController : BaseBuffUIData
    {
        private Image m_Sprite;
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
            m_StacksText = GetComponentInChildren<TextMeshProUGUI>();
            m_Sprite = transform.Find("sprite").GetComponent<Image>();

            if (buff.Metadata.Sprite != null)
            {
                m_Sprite.sprite = buff.Metadata.Sprite;
            }
        }
    }
}