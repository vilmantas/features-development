using System;
using Features.Actions;
using Features.Items;
using Integrations.Actions;
using UnityEngine;

namespace Integrations.LootContainer
{
    public class LootContainerController : MonoBehaviour
    {
        public bool Looted;

        public Action<CharacterController, ItemInstance> OnContainerLooted;

        private ItemInstance m_ItemInstance;

        private GameObject m_ItemModel;
        
        public void SetLoot(ItemInstance item)
        {
            m_ItemInstance = item;

            Looted = false;

            if (m_ItemInstance.Metadata.ModelPrefab == null) return;

            m_ItemModel = Instantiate(m_ItemInstance.Metadata.ModelPrefab, transform);
        }

        public void Loot(CharacterController looter)
        {
            if (Looted) return;
            
            Looted = true;
            
            var actionPayload = new ActionActivationPayload(new ActionBase(nameof(LootItem)), this, looter.gameObject);

            var pickupPayload = new LootItemActionPayload(actionPayload, m_ItemInstance);

            looter.GetComponentInChildren<ActionsController>().DoAction(pickupPayload);

            if (m_ItemModel != null)
            {
                Destroy(m_ItemModel);
            }
            
            OnContainerLooted?.Invoke(looter, m_ItemInstance);
        }
    }
}