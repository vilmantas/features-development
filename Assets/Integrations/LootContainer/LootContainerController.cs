using System;
using Features.Actions;
using Features.Character;
using Features.Items;
using Integrations.Actions;
using UnityEngine;

namespace Integrations.LootContainer
{
    public class LootContainerController : MonoBehaviour
    {
        public Item_SO Item;

        private GameObject m_ItemGameObject;

        private ItemInstance m_ItemInstance;

        private Transform m_ItemSpawn;

        public Action<Player, ItemInstance> OnContainerLooted;

        public bool Looted { get; set; }

        private void Awake()
        {
            m_ItemSpawn = transform.root.Find("spawn");

            if (Item == null) return;

            SetLoot(Item.MakeInstanceWithCount());
        }

        private void OnTriggerEnter(Collider other)
        {
            var character = other.transform.root.GetComponentInChildren<Player>();

            if (!character) return;

            Loot(character);
        }

        public void SetLoot(ItemInstance item)
        {
            m_ItemInstance = item;

            Looted = false;

            if (m_ItemInstance.Metadata.ModelPrefab == null) return;

            var parent = m_ItemSpawn == null ? transform : m_ItemSpawn;

            m_ItemGameObject = Instantiate(m_ItemInstance.Metadata.ModelPrefab, parent);
        }

        public void Loot(Player looter)
        {
            if (Looted) return;

            Looted = true;

            var actionPayload = new ActionActivationPayload(new ActionBase(nameof(LootItem)), this,
                looter.transform.root.gameObject);

            var pickupPayload = new LootItemActionPayload(actionPayload, m_ItemInstance);

            looter.transform.root.GetComponentInChildren<ActionsController>().DoAction(pickupPayload);

            if (m_ItemGameObject != null)
            {
                Destroy(m_ItemGameObject);
            }

            OnContainerLooted?.Invoke(looter, m_ItemInstance);
        }
    }
}