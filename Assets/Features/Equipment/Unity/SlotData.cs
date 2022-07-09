using System;
using UnityEngine;

namespace Equipment.Unity
{
    [Serializable]
    public class SlotData
    {
        [HideInInspector] public GameObject Instance;

        public Transform InstanceParent;
        public string slotType;
    }
}