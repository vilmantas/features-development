using System;
using UnityEngine;

namespace Features.Targeting
{
    public class TargetProvider : MonoBehaviour
    {
        public Action<Action<GameObject>> CharacterTargetProvider;

        public Func<Vector3> CurrentMousePositionProvider;
        
        public void GetCharacterTarget(Action<GameObject> callback)
        {
            CharacterTargetProvider.Invoke(callback);
        }

        public Vector3 GetMousePosition()
        {
            return CurrentMousePositionProvider.Invoke();
        }
    }
}