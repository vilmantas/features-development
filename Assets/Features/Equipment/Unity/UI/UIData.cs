using System;
using Equipment;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Equipment.Unity
{
    public class UIData : MonoBehaviour
    {
        [HideInInspector] public string SlotType = string.Empty;

        [HideInInspector] public TextMeshProUGUI Value;

        [HideInInspector] public EquipmentButtonPressEvent EquipmentButtonPressEvent;

        private Image Background;

        private Image Blocker;

        private Button Button;

        private CanvasGroup CanvasGroup;

        private Image ItemIcon;

        private EquipmentContainerItem m_EquipmentContainer;

        private Image Placeholder;

        private void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();

            Button = GetComponentInChildren<Button>();

            Button.onClick.AddListener(OnButtonPress);

            Value = GetComponentInChildren<TextMeshProUGUI>();

            var images = GetComponentsInChildren<Image>();

            foreach (var image in images)
            {
                if (image.name.EndsWith("placeholder"))
                {
                    Placeholder = image;
                }

                if (image.name.EndsWith("item_icon"))
                {
                    ItemIcon = image;
                }

                if (image.name.EndsWith("blocker"))
                {
                    Blocker = image;
                }
            }

            Hide();
        }

        private void OnDestroy()
        {
            Button.onClick.RemoveListener(OnButtonPress);
        }

        public void SetData(EquipmentContainerItem item)
        {
            m_EquipmentContainer = item;

            if (item.Main == null)
            {
                Blocker.enabled = true;
                Placeholder.gameObject.SetActive(true);
                ItemIcon.gameObject.SetActive(false);
                Value.text = string.Empty;
            }
            else
            {
                Blocker.enabled = false;

                if (item.Main.GetAmmoText != string.Empty)
                {
                    Value.text = item.Main.GetAmmoText;
                }
                else
                {
                    Value.text = string.Empty;
                }

                if (item.Main.Sprite != null)
                {
                    ItemIcon.sprite = item.Main.Sprite;
                    ItemIcon.gameObject.SetActive(true);
                    Placeholder.gameObject.SetActive(false);
                }
                else
                {
                    ItemIcon.gameObject.SetActive(false);
                    Placeholder.gameObject.SetActive(true);
                }
            }
        }

        private void OnButtonPress()
        {
            EquipmentButtonPressEvent.Invoke(m_EquipmentContainer);
        }

        public void Hide() => CanvasGroup.alpha = 0;

        public void Show() => CanvasGroup.alpha = 1;

        public bool IsVisible() => Math.Abs(CanvasGroup.alpha - 1) < 0.1;
    }
}

[Serializable]
public class EquipmentButtonPressEvent : UnityEvent<EquipmentContainerItem>
{
}