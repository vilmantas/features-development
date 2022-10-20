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

        private Sprite PlaceholderSprite;

        public override void OnAwake()
        {
            Count = GetComponentInChildren<TextMeshProUGUI>();

            var images = GetComponentsInChildren<Image>();

            foreach (var image in images)
            {
                if (image.name.EndsWith("placeholder"))
                {
                    PlaceholderSprite = Resources.Load<Sprite>("equipment_placeholder");
                    
                    Placeholder = image;

                    if (PlaceholderSprite != null)
                    {
                        Placeholder.sprite = PlaceholderSprite;
                    }
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
                var sprite = Resources.Load<Sprite>($"equipment_{item.Slot}");
                    
                if (sprite != null)
                {
                    Placeholder.sprite = sprite;
                }
                
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

        public override void OnReset()
        {
            Placeholder.gameObject.SetActive(true);
            ItemIcon.gameObject.SetActive(false);
            Count.text = string.Empty;
            Placeholder.sprite = PlaceholderSprite;
        }
    }
}