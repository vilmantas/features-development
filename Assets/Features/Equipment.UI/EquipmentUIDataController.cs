using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Equipment.UI
{
    public class EquipmentUIDataController : BaseEquipmentUIData
    {
        [HideInInspector] public TextMeshProUGUI Count;

        private Image ItemIcon;

        private Image Placeholder;

        public override void OnAwake()
        {
            Count = GetComponentInChildren<TextMeshProUGUI>();

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
            }
        }

        public override void OnSetData(EquipmentContainerItem item)
        {
            if (item.Main == null)
            {
                Placeholder.gameObject.SetActive(true);
                ItemIcon.gameObject.SetActive(false);
                Count.text = string.Empty;
            }
            else
            {
                Count.text = item.Main.Metadata.IsStackable ? item.Main.GetAmmoText : string.Empty;

                if (item.Main.Metadata.Sprite != null)
                {
                    ItemIcon.sprite = item.Main.Metadata.Sprite;
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
    }
}