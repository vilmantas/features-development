using System;
using Features.Inventory;
using Features.Inventory.UI;
using Features.Inventory.UI.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.ItemsContainer;

namespace DebugScripts
{
    public class InventoryUIDataController : BaseInventoryUIData
    {
        [HideInInspector] public TextMeshProUGUI Title;

        [HideInInspector] public TextMeshProUGUI Value;

        [HideInInspector] public Image ItemIcon;

        public override void OnAwake()
        {
            var children = GetComponentsInChildren<TextMeshProUGUI>();

            foreach (var textMeshProUGUI in children)
            {
                if (textMeshProUGUI.name.EndsWith("title"))
                {
                    Title = textMeshProUGUI;
                }

                if (textMeshProUGUI.name.EndsWith("value"))
                {
                    Value = textMeshProUGUI;
                }
            }

            var images = GetComponentsInChildren<Image>();

            foreach (var image in images)
            {
                if (image.name.EndsWith("item_icon"))
                {
                    ItemIcon = image;
                }
            }
        }

        public override void OnSetData(ContainerItem data)
        {
            if (data.IsEmpty)
            {
                Title.text = string.Empty;
                Value.enabled = false;
                ItemIcon.enabled = false;
            }
            else
            {
                Value.text = data.Item.StackableData.Max == 1
                    ? String.Empty
                    : data.Item.StackableData.Current.ToString();

                var metadata = (data.Item.Parent as IInventoryItemInstance).Metadata;

                if (metadata.Sprite == null)
                {
                    Title.enabled = true;
                    ItemIcon.enabled = false;

                    Title.text = metadata.Name;
                }
                else
                {
                    Title.enabled = false;
                    ItemIcon.enabled = true;

                    ItemIcon.sprite = metadata.Sprite;
                }
            }
        }
    }
}