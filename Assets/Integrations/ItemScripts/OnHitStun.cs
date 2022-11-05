using Features.Actions;
using Integrations.Actions;
using Integrations.Items;
using UnityEngine;

namespace System.Collections.Generic
{
    public class OnHitStun
    {
        private readonly GameObject m_Owner;
        private readonly ItemInstance m_Item;

        public OnHitStun(GameObject owner, ItemInstance item)
        {
            m_Owner = owner;
            m_Item = item;
        }
    }
}