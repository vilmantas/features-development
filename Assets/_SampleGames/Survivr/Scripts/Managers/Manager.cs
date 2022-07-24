using System;
using UnityEngine;

namespace _SampleGames.Survivr
{
    [Serializable]
    public abstract class Manager : MonoBehaviour
    {
        public void StartInit()
        {
            Initialize();
        }
        
        public abstract void Initialize();
    }
}