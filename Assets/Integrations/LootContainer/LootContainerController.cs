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
        
        public void SetLoot(ItemInstance item)
        {
            m_ItemInstance = item;

            Looted = false;
        }

        public void Loot(CharacterController looter)
        {
            if (Looted) return;
            
            Looted = true;
            
            var actionPayload = new ActionActivationPayload(new ActionBase(nameof(LootItem)), this, looter.gameObject);

            var pickupPayload = new LootItemActionPayload(actionPayload, m_ItemInstance);

            looter.GetComponentInChildren<ActionsController>().DoAction(pickupPayload);
            
            OnContainerLooted?.Invoke(looter, m_ItemInstance);
        }
    }
}