using System;
using UnityEngine;

namespace Features.Equipment
{
    [Serializable]
    public class SlotData
    {
        [HideInInspector] public GameObject Instance;

        public Transform InstanceParent;
        public string slotType;
        public bool AddHitboxTrigger;
    }
}