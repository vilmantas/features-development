using System;
using UnityEngine;

namespace _SampleGames.Survivr
{
    [Serializable]
    public abstract class Manager : MonoBehaviour
    {
        public abstract void Initialize();
    }
}